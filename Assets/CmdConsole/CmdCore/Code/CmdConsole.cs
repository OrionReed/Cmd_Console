using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;

namespace CmdConsole
{
    public class CmdConsole : MonoBehaviour
    {
        /*
        [SerializeField] private KeyCode toggleConsole = KeyCode.BackQuote;
        [SerializeField] private KeyCode autocomplete = KeyCode.Tab;
        [SerializeField] private KeyCode cycleOptionsForward = KeyCode.Period;
        [SerializeField] private KeyCode cycleOptionsBackward = KeyCode.Comma;
        [SerializeField] private int SuggestionCountLimit = 10;
        [SerializeField] private TMP_Text log;
        [SerializeField] private TMP_Text suggestions;
        private bool consoleVisible = false;
        */
        [SerializeField] private TMP_InputField inputField;

        [Header("Debug")]
        [SerializeField] private bool CommandParseDebug = false;

        private Selection<Command> commandSelection;

        private List<string> inputArgs = new List<string>();

        private void Start()
        {
            inputField.onValidateInput = OnValidateInput;
            inputField.onValueChanged.AddListener(OnValueChanged);
            inputField.onSubmit.AddListener(OnSubmit);
        }

        #region Input String Processing
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

        private void OnValueChanged(string input)
        {
            List<string> previousArgs = inputArgs;
            inputArgs = Regex.Split(input, @"\s").Where(s => s.Length != 0).ToList<string>();

            if (inputArgs.Any() && !previousArgs.Any()) //If this is the first input
            {
                previousArgs = inputArgs;
                TryParseCommand();
            }
            else if (new HashSet<string>(inputArgs).SetEquals(previousArgs))
            {
                return;
            }

            for (int i = 0; i < inputArgs.Count; i++)
            {
                if (previousArgs.ElementAtOrDefault(i) != null)
                {
                    if (inputArgs[i] != previousArgs[i])
                    {
                        if (i == 0)
                        {
                            TryParseCommand();
                            for (int c = 1; c < inputArgs.Count; c++)
                            {
                                TryParseArgument(c);
                            }
                            break;
                        }
                        TryParseArgument(i);
                    }
                    continue;
                }
                TryParseArgument(i);
            }
        }

        private void OnSubmit(string input)
        {
            ExecuteCommand();
        }
        #endregion

        private void TryParseCommand()
        {
            if (CommandParseDebug) Debug.Log("Parsing Command <b>" + inputArgs[0] + "</b>");
        }

        private void TryParseArgument(int index)
        {
            if (CommandParseDebug) Debug.Log("Parsing Arg " + index + ": <b>" + inputArgs[index] + "</b>");
        }

        private void ShowCompleteOptions()
        {

        }

        private void ExecuteCommand()
        {
            Debug.Log("Executed <b>" + inputArgs[0] + "</b>");
        }

    }
}