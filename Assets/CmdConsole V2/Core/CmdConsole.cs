using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Text;

namespace CmdConsole
{
    public class CmdConsole : MonoBehaviour
    {
        public TMP_InputField inputField { get; private set; }

        [SerializeField] CmdStylePalette stylePalette;
        [SerializeField] private KeyCode IncrementOption;
        [SerializeField] private KeyCode DecrementOption;
        [SerializeField] private KeyCode Autocomplete;
        [SerializeField] private RectTransform OptionsPanel;
        [SerializeField] private TMP_Text logText;

        private float originalX;
        private float originalY;

        private const float charWidth = 15f;
        private TMP_Text optionText;
        private TMP_SelectionCaret caret;
        private Canvas caretCanvas;
        private CanvasGroup SuggestionPanelCanvas;
        private int caretIndex = 0;
        private List<string> latestInput = new List<string>();
        private List<IArg> arguments = new List<IArg>();
        private IArg focusedArg;
        private int focusCharOffset;
        private IArg command = new Arg_Command();

        private void Start()
        {
            inputField = GetComponentInChildren<TMP_InputField>();
            optionText = OptionsPanel.GetComponentInChildren<TMP_Text>();
            caret = inputField.GetComponentInChildren<TMP_SelectionCaret>();
            caretCanvas = caret.gameObject.AddComponent<Canvas>();
            caretCanvas.overrideSorting = true;
            caretCanvas.sortingOrder = 3;
            SuggestionPanelCanvas = OptionsPanel.GetComponent<CanvasGroup>();
            originalX = OptionsPanel.localPosition.x;
            originalY = OptionsPanel.localPosition.y;
            inputField.onValidateInput = OnValidate;
            inputField.onValueChanged.AddListener(OnInputUpdate);
            inputField.onSubmit.AddListener(OnSubmit);
            optionText.text = "";


            CmdRegistry.Init();
            command.SetInput("");
            RedrawOptionsWindow();
        }
        private void Update()
        {
            if (inputField.caretPosition != caretIndex)
            {
                caretIndex = inputField.caretPosition;
                UpdateFocus();
                RedrawOptionsWindow();
            }
            if (Input.GetKeyDown(IncrementOption))
            {
                IncrementCurrent();
                RedrawOptionsWindow();
            }
            if (Input.GetKeyDown(DecrementOption))
            {
                DecrementCurrent();
                RedrawOptionsWindow();
            }
            if (Input.GetKeyDown(Autocomplete))
                TryAutocomplete();
        }

        private char OnValidate(string text, int charIndex, char addedChar)
        {
            if (char.IsLetterOrDigit(addedChar) || addedChar == '-' || addedChar == '.')
            {
                return addedChar;
            }
            if (char.IsWhiteSpace(addedChar))
            {
                if (charIndex == 0 ||
                    charIndex > 0 && char.IsWhiteSpace(text[charIndex - 1]) ||
                    charIndex < text.Length && char.IsWhiteSpace(text[charIndex]))
                {
                    return '\0';
                }
                return addedChar;
            }
            return '\0';
        }

        private void OnInputUpdate(string newInputString)
        {
            List<string> newInput = Regex.Split(newInputString, @"\s").Where(s => s.Length != 0).ToList<string>();

            if (newInput.ElementAtOrDefault(0) == null)
                command.SetInput("");
            else if (newInput[0] != command.Input)
                command.SetInput(newInput[0]);

            if (latestInput != newInput)
                latestInput = newInput;

            ICommand currentCommand = (ICommand)command.CurrentValue;

            if (currentCommand != null)
            {
                if (currentCommand.Variables.Count != arguments.Count)
                {
                    RebuildArgs(currentCommand);
                }
                else
                {
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        if (arguments[i].Type != currentCommand.Variables[i].Type)
                        {
                            if (arguments[i].GetType() == currentCommand.Variables[i].Type)
                                continue;
                            RebuildArgs(currentCommand);
                            break;
                        }
                    }
                }
                if (newInput.Any())
                {
                    newInput.RemoveAt(0);
                    for (int a = 0; a < arguments.Count; a++)
                    {
                        if (!newInput.Any())
                        {
                            arguments[a].SetInput("");
                            continue;
                        }
                        string argInput = "";
                        for (int x = 0; x < arguments[a].Parts; x++)
                        {
                            if (newInput.Any())
                            {
                                argInput += newInput.First() + " ";
                                newInput.RemoveAt(0);
                            }
                            else
                                break;
                        }
                        arguments[a].SetInput(argInput.Trim());
                    }
                }
            }
            RedrawOptionsWindow();
        }

        private void OnSubmit(string s)
        {
            if (command.CurrentValue == null)
                return;

            if (arguments.Any())
            {
                try
                {
                    ((ICommand)command.CurrentValue).ExecuteWithArguments(arguments.Select(a => a.CurrentValue).ToList());
                }
                catch (NullReferenceException)
                {
                    Debug.Log("Command Args do not match command");
                }
                inputField.text = "";
                return;
            }
            ((ICommand)command.CurrentValue).ExecuteDefault();
            inputField.text = "";
        }

        private void RedrawOptionsWindow()
        {
            if (focusedArg != null)
                optionText.text = GetOptionsList(focusedArg);
            else
                optionText.text = "";

            SuggestionPanelCanvas.alpha = optionText.text == "" ? 0 : 1;
            inputField.caretColor = optionText.text == "" ? stylePalette.CaretDark : stylePalette.CaretLight;
            OptionsPanel.localPosition = new Vector2(originalX + (focusCharOffset * charWidth), originalY);
        }

        private void RedrawInputText()
        {
            inputField.text = command.Input + " " + String.Join(" ", arguments.Select(a => a.Input).Where(i => i != "").ToArray());
            if (inputField.text.Last() != ' ')
                inputField.text += " ";
            inputField.MoveTextEnd(false);
        }

        private string GetOptionsList(IArg arg)
        {
            StringBuilder options = new StringBuilder("");
            if (arg.CurrentValue == null)
                return options.ToString();

            for (int i = arg.GetOptions().Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    options.Append(arg.Input.ColorString(stylePalette.Emphasis) + arg.CurrentKey.Remove(0, arg.Input.Length).ColorString(stylePalette.Autocomplete) + "\n");
                    continue;
                }
                if (arg.GetOptionAtIndex(i).Key == arg.CurrentKey)
                {
                    options.Append(arg.GetOptionAtIndex(i).Key.ColorString(stylePalette.Confirm) + "\n");
                    continue;
                }
                else
                {
                    options.Append(arg.GetOptionAtIndex(i).Key.ColorString(stylePalette.Default) + "\n");
                }
            }
            return options.ToString();
        }

        private void RebuildArgs(ICommand commandToBuild)
        {
            arguments.Clear();
            for (int i = 0; i < commandToBuild.Variables.Count; i++)
            {
                if (typeof(ArgBase).IsAssignableFrom(commandToBuild.Variables[i].Type))
                {
                    arguments.Add((IArg)Activator.CreateInstance(commandToBuild.Variables[i].Type));
                    arguments.Last().Init();
                }
                else
                {
                    arguments.Add(new Arg(commandToBuild.Variables[i].Type));
                    arguments.Last().Init();
                }
            }
        }

        private void TryAutocomplete()
        {
            if (focusedArg.CurrentKey != null)
            {
                focusedArg.SetInput(focusedArg.CurrentKey);
                RedrawInputText();
                RedrawOptionsWindow();
            }

        }

        private void UpdateFocus()
        {
            int counter = command.Input.Length + 1;
            if (counter > caretIndex)
            {
                focusedArg = command;
                focusCharOffset = 0;
                return;
            }
            focusCharOffset = counter;
            for (int i = 0; i < arguments.Count; i++)
            {
                counter += arguments[i].Input.Length + 1;
                if (counter > caretIndex)
                {
                    focusedArg = arguments[i];
                    return;
                }
                focusCharOffset = counter;
            }
            focusedArg = null;
        }

        private void IncrementCurrent() => focusedArg.IncrementOption();
        private void DecrementCurrent() => focusedArg.DecrementOption();
    }
}