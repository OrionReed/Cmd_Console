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

        private Selection<Command> commandSelection;
        private List<string> arguments = new List<string>();

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
            List<string> previousArgs = arguments;
            arguments = Regex.Split(input, @"\s").Where(s => s.Length != 0).ToList<string>();

            if (arguments.Any() && !previousArgs.Any()) //If this is the first input
            {
                previousArgs = arguments;
                TryParseCommand();
            }
            else if (new HashSet<string>(arguments).SetEquals(previousArgs))
            {
                return;
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                if (previousArgs.ElementAtOrDefault(i) != null)
                {
                    if (arguments[i] != previousArgs[i])
                    {
                        if (i == 0)
                        {
                            TryParseCommand();
                            for (int c = 1; c < arguments.Count; c++)
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
            Debug.Log("Parsing Command <b>" + arguments[0] + "</b>");
        }

        private void TryParseArgument(int index)
        {
            Debug.Log("Parsing Arg " + index + ": <b>" + arguments[index] + "</b>");
        }

        private void ShowCompleteOptions()
        {

        }

        private void ExecuteCommand()
        {
            Debug.Log("Executed <b>" + arguments[0] + "</b>");
        }

    }
}