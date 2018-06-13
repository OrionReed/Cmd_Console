using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace CmdConsole
{
    /// Holds variables and sets up CmdConsole parts.
    public class CmdConsole : MonoBehaviour
    {
        public const int maxArgs = 15;
        public static CmdConsole instance;
        public static ValidCommandList commands = new ValidCommandList();
        public static ValidArgList arguments = new ValidArgList();
        public static int caretPos;

        public CanvasGroup SuggestionPanel;
        public TMP_Text suggestionText;
        public TMP_Text autocompleteText;
        public TMP_Text logText;
        public TMP_InputField inputField;

        //        [SerializeField] private KeyCode cycleOptionsForward = KeyCode.Period;
        //        [SerializeField] private KeyCode cycleOptionsBackward = KeyCode.Comma;
        //        [SerializeField] private KeyCode autocomplete = KeyCode.Tab;
        //        [SerializeField] private int suggestionsLimit = 10;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.Log("There can only be one CmdConsole in the scene");
                Destroy(this);
            }
            inputField = GetComponentInChildren<TMP_InputField>();
            inputField.onValidateInput = OnValidateInput;
            inputField.onValueChanged.AddListener(OnInputUpdate);
            inputField.onSubmit.AddListener(OnSubmit);
            CmdRegistry.RegisterCommands();
        }

        private void Update()
        {
            if (caretPos != inputField.caretPosition)
            {
                caretPos = inputField.caretPosition;
                CmdSuggestions.UpdateFocus();
                //CmdSuggestions.UpdateSuggestionWindow();
            }
        }

        private char OnValidateInput(string text, int charIndex, char addedChar)
        {
            if (char.IsLetterOrDigit(addedChar) || addedChar == '-')
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

        private void OnInputUpdate(string i)
        {
            CmdParser.OnInputStringChange(i);
        }

        private void OnSubmit(string s)
        {
            // IF all args are parsed
            CmdCommandRunner.Execute(commands.GetCurrentValue(), arguments.GetValueList());
        }
    }
}