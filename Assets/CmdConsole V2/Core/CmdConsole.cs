using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace CmdConsole
{
    public class CmdConsole : MonoBehaviour
    {

        #region Variables – Serialized Private
        [SerializeField] private CmdStylePalette stylePalette;
        [SerializeField] private KeyCode IncrementOption;
        [SerializeField] private KeyCode DecrementOption;
        [SerializeField] private KeyCode Autocomplete;
        [SerializeField] private RectTransform OptionsPanel;
        [SerializeField] private TMP_Text logText;
        [SerializeField] private RectTransform nonRuntimeHighlight;
        #endregion
        #region Variables – Private
        private TMP_Text optionText;
        private TMP_SelectionCaret caret;
        private Canvas caretCanvas;
        private CanvasGroup SuggestionPanelCanvas;
        private int lastCaretIndex = 0;
        private List<string> latestTokens = new List<string> ();
        private List<IArg> arguments = new List<IArg> ();
        private IArg command = new Arg_Command ();
        private IArg focusedArg;
        #endregion
        #region Properties – Get
        public static TMP_InputField InputField { get; private set; }
        public static Vector2 HighlightOffset { get; private set; }
        public static float charWidth = 15f;
        #endregion

        #region Methods – Unity Callbacks
        private void Start ()
        {
            InputField = GetComponentInChildren<TMP_InputField> ();
            optionText = OptionsPanel.GetComponentInChildren<TMP_Text> ();
            caret = InputField.GetComponentInChildren<TMP_SelectionCaret> ();
            caretCanvas = caret.gameObject.AddComponent<Canvas> ();
            caretCanvas.overrideSorting = true;
            caretCanvas.sortingOrder = 3;
            SuggestionPanelCanvas = OptionsPanel.GetComponent<CanvasGroup> ();
            HighlightOffset = OptionsPanel.localPosition - InputField.textComponent.rectTransform.localPosition;
            InputField.onValidateInput = OnValidateChar;
            InputField.onValueChanged.AddListener (OnInputUpdate);
            InputField.onSubmit.AddListener (OnSubmit);
            optionText.text = "";
            CmdRegistry.Init ();
            command.SetInput ("");
            UpdateArgPositions ();
            UpdateArgFocus ();
            RedrawOptionsWindow ();
            CmdLog.LogText = logText;
            CmdLog.Style = stylePalette;
        }

        private void Update ()
        {
            if (Input.GetKeyDown (IncrementOption)) { IncrementCurrent (); RedrawOptionsWindow (); }
            if (Input.GetKeyDown (DecrementOption)) { DecrementCurrent (); RedrawOptionsWindow (); }
            if (Input.GetKeyDown (Autocomplete)) AutocompleteArg ();
            if (InputField.caretPosition != lastCaretIndex)
            {
                lastCaretIndex = InputField.caretPosition;
                UpdateArgPositions ();
                UpdateArgFocus ();
                UpdateHighlights ();
                RedrawOptionsWindow ();
            }
        }
        #endregion

        #region Methods – Private
        private char OnValidateChar (string text, int charIndex, char addedChar)
        {
            if (char.IsLetterOrDigit (addedChar) || addedChar == '-' || addedChar == '.')
                return addedChar;

            if (char.IsWhiteSpace (addedChar))
            {
                if (charIndex == 0 ||
                    charIndex > 0 && char.IsWhiteSpace (text[charIndex - 1]) ||
                    charIndex < text.Length && char.IsWhiteSpace (text[charIndex]))
                {
                    return '\0';
                }
                return addedChar;
            }
            return '\0';
        }

        private void OnInputUpdate (string newInputString)
        {
            List<string> newInput = Regex.Split (newInputString, @"\s").Where (s => s.Length != 0).ToList<string> ();
            UpdateCommandInput (newInput);

            if (latestTokens != newInput)
                latestTokens = newInput;

            ICommand currentCommand = (ICommand) command.CurrentValue;

            if (currentCommand == null)
            {
                RedrawOptionsWindow ();
                return;
            }
            if (currentCommand.Variables.Count != arguments.Count)
                RebuildArgs (currentCommand);

            ValidateTypes (currentCommand);
            UpdateArgInputs (newInput);
            RedrawOptionsWindow ();
        }

        private void OnSubmit (string input)
        {
            InputField.text = "";
            if (string.IsNullOrWhiteSpace (input))
                return;

            if (command.CurrentValue == null)
                return;

            if (!arguments.Any ())
            {
                ((ICommand) command.CurrentValue).ExecuteDefault ();
                return;
            }

            try
            {
                ((ICommand) command.CurrentValue).ExecuteWithArguments (arguments.Select (a => a.CurrentValue).ToList ());
            }
            catch (NullReferenceException)
            {
                Debug.Log ("Command Args do not match command");
            }
        }

        // ---------- LIL' SUBSYSTEMS ---------- //

        private void UpdateCommandInput (List<string> newInput)
        {
            if (newInput.ElementAtOrDefault (0) == null)
                command.SetInput ("");
            else
                command.SetInput (newInput[0]);
        }

        private void UpdateArgInputs (List<string> newInput)
        {
            if (!newInput.Any ())
                return;

            newInput.RemoveAt (0);

            for (int a = 0; a < arguments.Count; a++)
            {
                if (!newInput.Any ())
                {
                    arguments[a].SetInput ("");
                    continue;
                }
                string argInput = "";
                for (int x = 0; x < arguments[a].Parts; x++)
                {
                    if (newInput.Any ())
                    {
                        argInput += newInput.First () + " ";
                        newInput.RemoveAt (0);
                    }
                    else
                        break;
                }
                arguments[a].SetInput (argInput.Trim ());
            }
        }

        private void ValidateTypes (ICommand currentCommand)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i].GetType () == currentCommand.Variables[i].Type)
                    continue;
                if (arguments[i].Type != currentCommand.Variables[i].Type)
                {
                    RebuildArgs (currentCommand);
                    break;
                }
            }
        }

        private void UpdateHighlights ()
        {
            foreach (IArg arg in arguments)
            {
                if (arg.Parts > 1)
                {
                    nonRuntimeHighlight.GetComponentInChildren<TMP_Text> ().text = arg.Input;
                    nonRuntimeHighlight.localPosition = new Vector2 (
                        InputField.textComponent.rectTransform.localPosition.x + HighlightOffset.x + (arg.Position * charWidth),
                        InputField.textComponent.rectTransform.localPosition.y + HighlightOffset.y);
                    nonRuntimeHighlight.GetComponent<CanvasGroup> ().alpha = arg.Input.Any () ? 1 : 0;
                }
            }
        }

        private void RedrawOptionsWindow ()
        {
            if (focusedArg != null)
                optionText.text = GetOptionsString (focusedArg);
            else
                optionText.text = "";

            if (optionText.text == "")
            {
                SuggestionPanelCanvas.alpha = 0;
                InputField.caretColor = stylePalette.CaretDark;
            }
            else
            {
                SuggestionPanelCanvas.alpha = 1;
                InputField.caretColor = stylePalette.CaretLight;
                OptionsPanel.localPosition = new Vector2 (
                    InputField.textComponent.rectTransform.localPosition.x + HighlightOffset.x + (focusedArg.Position * charWidth),
                    InputField.textComponent.rectTransform.localPosition.y + HighlightOffset.y);
            }
        }

        private void RedrawInputText ()
        {
            InputField.text = command.Input + " " + String.Join (" ", arguments.Select (a => a.Input).Where (i => i != "").ToArray ());
            if (InputField.text.Last () != ' ')
                InputField.text += " ";
            InputField.MoveTextEnd (false);
        }

        private void RebuildArgs (ICommand commandToBuild)
        {
            arguments.Clear ();
            for (int i = 0; i < commandToBuild.Variables.Count; i++)
            {
                if (typeof (ArgBase).IsAssignableFrom (commandToBuild.Variables[i].Type))
                {
                    arguments.Add ((IArg) Activator.CreateInstance (commandToBuild.Variables[i].Type));
                    arguments.Last ().Init ();
                }
                else
                {
                    arguments.Add (new Arg (commandToBuild.Variables[i].Type));
                    arguments.Last ().Init ();
                }
            }
        }

        private void AutocompleteArg ()
        {
            if (focusedArg.CurrentKey == null)
                return;

            focusedArg.SetInput (focusedArg.CurrentKey);
            RedrawInputText ();
            RedrawOptionsWindow ();
        }

        private void UpdateArgPositions ()
        {
            command.SetPosition (0);
            int position = command.Input.Length + 1;
            for (int i = 0; i < arguments.Count; i++)
            {
                arguments[i].SetPosition (position);
                position += arguments[i].Input.Length + 1;
            }
        }
        private void UpdateArgFocus ()
        {
            bool foundFocus = false;
            if (command.Input.Length + 1 > lastCaretIndex)
            {
                focusedArg = command;
                foundFocus = true;
            }
            for (int i = 0; i < arguments.Count; i++)
            {
                if (arguments[i].Position + arguments[i].Input.Length + 1 > lastCaretIndex && foundFocus == false)
                {
                    focusedArg = arguments[i];
                    foundFocus = true;
                }
            }
            if (foundFocus == false)
            {
                focusedArg = null;
            }
        }

        private string GetOptionsString (IArg arg)
        {
            StringBuilder options = new StringBuilder ("");
            if (arg.CurrentValue == null)
                return options.ToString ();

            for (int i = arg.GetOptions ().Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    options.Append (arg.Input.ColorString (stylePalette.Emphasis) + arg.CurrentKey.Remove (0, arg.Input.Length).ColorString (stylePalette.Autocomplete) + "\n");
                    continue;
                }
                if (arg.GetOptionAtIndex (i).Key == arg.CurrentKey)
                {
                    options.Append (arg.GetOptionAtIndex (i).Key.ColorString (stylePalette.Confirm) + "\n");
                    continue;
                }
                else
                {
                    options.Append (arg.GetOptionAtIndex (i).Key.ColorString (stylePalette.Default) + "\n");
                }
            }
            return options.ToString ();
        }

        private void IncrementCurrent () => focusedArg.IncrementOption ();
        private void DecrementCurrent () => focusedArg.DecrementOption ();
        #endregion

    }
}