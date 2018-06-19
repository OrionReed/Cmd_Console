using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace CmdConsole
{
    public class CmdConsole : MonoBehaviour
    {
        public TMP_InputField inputField { get; private set; }
        [SerializeField] private CanvasGroup SuggestionPanel;
        [SerializeField] private TMP_Text suggestionText;
        [SerializeField] private TMP_Text autocompleteText;
        [SerializeField] private TMP_Text logText;
        [SerializeField] private KeyCode IncrementOption;
        [SerializeField] private KeyCode DecrementOption;
        [SerializeField] private KeyCode Autocomplete;
        [SerializeField] private Color ColHighlight;
        [SerializeField] private Color ColFailedParse;
        [SerializeField] private Color ColClear;

        private int focus = 0;
        private int caretIndex = 0;
        private List<string> latestInput = new List<string>();
        private IArg command = new Arg_Command();
        private List<IArg> arguments = new List<IArg>();

        private void Start()
        {
            inputField = GetComponentInChildren<TMP_InputField>();
            inputField.onValidateInput = OnValidateInput;
            inputField.onValueChanged.AddListener(OnInputUpdate);
            inputField.onSubmit.AddListener(OnSubmit);
            CmdRegistry.Init();
            command.SetInputString("");
            UpdateOptionsWindow();
        }
        private void Update()
        {
            if (inputField.caretPosition != caretIndex)
            {
                caretIndex = inputField.caretPosition;
                UpdateFocusByCaret();
            }
            if (Input.GetKeyDown(IncrementOption))
                Increment();
            if (Input.GetKeyDown(DecrementOption))
                Decrement();
            if (Input.GetKeyDown(Autocomplete))
                TryAutocomplete();
        }

        private char OnValidateInput(string text, int charIndex, char addedChar)
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
                command.SetInputString("");
            else if (newInput[0] != command.InputString)
                command.SetInputString(newInput[0]);

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
                            arguments[a].SetInputString("");
                            continue;
                        }
                        string argInput = "";
                        for (int x = 0; x < currentCommand.Variables[a].Parts; x++)
                        {
                            if (newInput.Any())
                            {
                                argInput += newInput.First() + " ";
                                newInput.RemoveAt(0);
                            }
                            else
                                break;
                        }
                        arguments[a].SetInputString(argInput.Trim());
                    }
                }
            }
            UpdateOptionsWindow();
        }

        private void UpdateOptionsWindow()
        {
            suggestionText.text = "";
            if (focus == 0)
            {
                suggestionText.text = GetOptionsList(command);
            }
            else
            {
                int argFocus = focus - 1;
                if (arguments.ElementAtOrDefault(argFocus) != null)
                {
                    suggestionText.text = GetOptionsList(arguments[argFocus]);
                }
            }
            SuggestionPanel.alpha = suggestionText.text == "" ? 0 : 1;
        }

        private string GetOptionsList(IArg arg)
        {
            string options = "";
            if (arg.CurrentValue == null)
                return options;
            for (int i = arg.GetOptions().Count - 1; i >= 0; i--)
            {
                if (arg.GetOptions().ElementAt(i).Value == arg.CurrentValue)
                {
                    options += arg.GetOptions().ElementAt(i).Key.ColorString(ColHighlight) + "\n";
                }
                else if (i == 0)
                {
                    options += arg.GetOptions().ElementAt(i).Key.ColorString(ColClear) + "\n";
                }
                else
                {
                    options += arg.GetOptions().ElementAt(i).Key + "\n";
                }
            }
            return options;
        }

        private void RebuildArgs(ICommand forCommand)
        {
            arguments.Clear();
            for (int i = 0; i < forCommand.Variables.Count; i++)
            {
                if (typeof(ArgBase).IsAssignableFrom(forCommand.Variables[i].Type))
                {
                    arguments.Add((IArg)Activator.CreateInstance(forCommand.Variables[i].Type));
                    arguments.Last().Init();
                }
                else
                {
                    arguments.Add(new Arg(forCommand.Variables[i].Type));
                    arguments.Last().Init();
                }
            }
            Debug.Log("<b>Rebuilding Command Args:</b>");
            foreach (IArg arg in arguments)
            {
                Debug.Log("<b>Type: </b>" + arg.Type);
                if (!String.IsNullOrEmpty(arg.InputString))
                    Debug.Log("<b>With input: </b>" + arg.InputString);
            }
        }

        private void TryAutocomplete()
        {
            if (focus <= 0)
            {
                if (command.CurrentKey != null)
                    command.SetInputString(command.CurrentKey);
            }
            else
            {
                if (arguments[focus - 1].CurrentKey != null)
                    arguments[focus - 1].SetInputString(arguments[focus - 1].CurrentKey);
            }
            UpdateSyntaxHighlights();
        }

        private void UpdateSyntaxHighlights()
        {
            string inputWithHighlights = "";
            if (String.Compare(command.InputString, command.CurrentKey, true) == 0)
            {
                inputWithHighlights += command.InputString.ColorString(ColFailedParse) + " ";
            }
            else
            {
                inputWithHighlights += command.InputString + " ";
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                if (String.Compare(arguments[i].InputString, arguments[i].CurrentKey, true) == 0)
                {
                    inputWithHighlights += arguments[i].InputString.ColorString(ColFailedParse) + " ";
                }
                else
                {
                    inputWithHighlights += arguments[i].InputString + " ";
                }
            }
            inputField.text = inputWithHighlights;
        }

        private void OnSubmit(string s)
        {
            if (command.GetOptions().Any())
            {
                ICommand com = (ICommand)command.CurrentValue;
                if (arguments.Any())
                {
                    List<object> args = new List<object>();
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        args.Add(arguments[i].CurrentValue);
                    }
                    com.ExecuteWithArguments(args);
                }
                com.ExecuteDefault();
            }
        }

        private void UpdateFocusByCaret()
        {
            if (caretIndex <= command.InputString.Length)
            {
                Debug.Log("Focusing on Command");
                focus = 0;
                return;
            }
            int counter = command.InputString.Length + 1;
            for (int i = 0; i < arguments.Count; i++)
            {
                counter += arguments[i].InputString.Length + 1;
                if (counter > caretIndex)
                {
                    focus = i + 1;
                    Debug.Log("Focusing on Arg: " + (focus - 1));
                    UpdateOptionsWindow();
                    return;
                }
            }
        }

        private void Increment()
        {
            if (focus <= 0)
            {
                command.IncrementOption();
            }
            else
            {
                arguments.ElementAtOrDefault(focus - 1).IncrementOption();
            }
            UpdateOptionsWindow();
        }

        private void Decrement()
        {
            if (focus <= 0)
            {
                command.DecrementOption();
            }
            else
            {
                arguments.ElementAtOrDefault(focus - 1).DecrementOption();
            }
            UpdateOptionsWindow();
        }
    }
}