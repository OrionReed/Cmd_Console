using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;

namespace CmdConsole
{
    /*
    public class Console : MonoBehaviour
    {

        public static Console instance { get; private set; }
        public bool visible { get; private set; } = true;

        [SerializeField] private CmdStylePalette stylePalette;
        [SerializeField] private float toggleSpeed = 0.15f;
        [Space]
        [SerializeField] private KeyCode showConsole = KeyCode.Slash;
        [SerializeField] private KeyCode hideConsole = KeyCode.Escape;
        [SerializeField] private KeyCode autocomplete = KeyCode.Tab;
        [SerializeField] private KeyCode cycleOptionsForward = KeyCode.Period;
        [SerializeField] private KeyCode cycleOptionsBackward = KeyCode.Comma;

        private List<CmdMessage> internalMessageLog = new List<CmdMessage>();
        private List<CmdMessage> internalSystemMessageLog = new List<CmdMessage>();
        private List<ICommand> matchingCommands = new List<ICommand>();
        private List<IParam> matchingParameters = new List<IParam>();
        private string commandSearch = "";
        private string parameterSearch = "";
        private int targetCommand = 0;
        private int targetParameter = 0;
        private List<string> args = new List<string>();
        private IEnumerator CoTransition;

        #region Setup
        private void Start()
        {
            instance = this;
            ConsoleCommands.RegisterCommands();
            ConsoleCommands.SortCommands();
            LogSystemMessage(ConsoleCommands.ValidateCommands());
        }
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(cycleOptionsForward))
                OnCycleCommand(1); OnCycleParam(1);
            if (Input.GetKeyDown(cycleOptionsBackward))
                OnCycleCommand(-1); OnCycleParam(-1);
            // if (Input.GetKeyDown(autocomplete))
            //     if (autocompleteText.text != "")
            //        inputField.text = autocompleteText.text;

            if (Input.GetKeyDown(showConsole))
                SetVisibility(true);
            if (Input.GetKeyDown(hideConsole))
                SetVisibility(false);
        }

        #region Events
        private void OnTextInputChanged(string input)
        {
            List<string> splitInput = Regex.Split(input, @"\W+").ToList();
            if (splitInput.Count > 0)
            {
                commandSearch = splitInput[0];
                if (splitInput.Count > 1)
                {
                    parameterSearch = splitInput[1];
                    if (splitInput.Count > 2)
                    {
                        splitInput.RemoveRange(0, 2);
                        args = splitInput;
                    }
                }
                else
                {
                    parameterSearch = "";
                }
            }
            else
            {
                commandSearch = "";
                parameterSearch = "";
            }
            ClampTargets();
            OnInputChanged();
        }

        private void OnInputChanged()
        {
            if (string.IsNullOrWhiteSpace(commandSearch) == false)
            {
                matchingCommands = SearchCommands();
                if (string.IsNullOrWhiteSpace(parameterSearch) == false && matchingParameters.Any())
                {
                    matchingParameters = SearchParameters();
                }
                else
                {
                    matchingParameters.Clear();
                }
            }
            else
            {
                matchingCommands.Clear();
                matchingParameters.Clear();
            }
            ClampTargets();
            UpdateSuggestion();
        }


        private void OnCycleCommand(int increment)
        {
            targetCommand += increment;
            ClampTargets();
            OnInputChanged();
        }
        private void OnCycleParam(int increment)
        {
            targetParameter += increment;
            ClampTargets();
            OnInputChanged();
        }
        private void OnProcessCommand(string rawInput)
        {
            if (matchingCommands.Any())
            {
                if (parameterSearch.Any())
                {
                    if (args.Any())
                    {
                        LogMessage(matchingCommands[targetCommand].ProcessArgs(parameterSearch, args));
                    }
                    else
                    {
                        LogMessage(matchingCommands[targetCommand].ProcessArgs(parameterSearch, null));
                    }
                }
                else
                {
                    LogMessage(matchingCommands[targetCommand].ProcessDefault());
                }
                inputField.text = "";
            }
            else if (commandSearch.Any())
            {
                LogMessage(new CMLine(CMLineType.Warning)
                        {
                            {commandSearch, CMStyle.Emphasis},
                            {" is not a valid command", CMStyle.Default}
                        }
                );
            }
        }
        #endregion

        #region Utility
        private void ClampTargets()
        {
            targetCommand = Mathf.Clamp(targetCommand, 0, matchingCommands.Count - 1);
            targetParameter = Mathf.Clamp(targetParameter, 0, matchingParameters.Count - 1);
        }

        private void UpdateSuggestion()
        {
            if (matchingCommands.Any())
            {
                //autocompleteText.text = matchingCommands[targetCommand].Name;
                if (matchingParameters.Any())
                {
                    //autocompleteText.text += " " + matchingParameters[targetParameter].Name;
                }
            }
            else
            {
                //autocompleteText.text = "";
            }
        }

        private List<ICommand> SearchCommands()
        {
            List<ICommand> matchingCommands = new List<ICommand>(ConsoleCommands.Commands);
            for (int i = matchingCommands.Count - 1; i >= 0; i--)
                if (matchingCommands[i].Name.StartsWith(commandSearch) == false)
                    matchingCommands.RemoveAt(i);

            return matchingCommands;
        }
        private List<IParam> SearchParameters()
        {
            List<IParam> matchingParameters = new List<IParam>(matchingCommands[targetCommand].Parameters);
            for (int i = matchingCommands[targetCommand].Parameters.Count - 1; i >= 0; i--)
                if (matchingCommands[targetCommand].Parameters[i].Name.StartsWith(parameterSearch) == false)
                    matchingParameters.RemoveAt(i);

            return matchingParameters;
        }
        #endregion

        #region Log
        public void ClearLog(bool clearInternalLog = false)
        {
            logText.text = "";
            if (clearInternalLog)
                internalMessageLog.Clear();
        }

        public void LogSystemMessage(CmdMessage message)
        {
            if (message != null && message.Lines.Any())
            {
                internalSystemMessageLog.Add(message);
                for (int i = 0; i < message.Lines.Count; i++)
                {
                    logText.text +=
                        "\n" +
                        ColorString("[CmdConsole] ", stylePalette.System) +
                        FormatLine(message.Lines[i]);
                }
            }
        }

        public void LogMessage(CMLine message) => LogMessage(new CmdMessage(message));

        public void LogMessage(CmdMessage consoleMessage)
        {
            if (consoleMessage != null && consoleMessage.Lines.Any())
            {
                internalMessageLog.Add(consoleMessage);
                for (int i = 0; i < consoleMessage.Lines.Count; i++)
                {
                    string prefix = "";
                    switch (consoleMessage.Lines[i].Type)
                    {
                        case CMLineType.Default:
                            if (string.IsNullOrWhiteSpace(stylePalette.PrefixDefault) == false)
                                prefix = stylePalette.PrefixDefault.Trim() + " ";
                            break;
                        case CMLineType.Warning:
                            if (string.IsNullOrWhiteSpace(stylePalette.PrefixWarning) == false)
                                prefix = stylePalette.PrefixWarning.Trim() + " ";
                            break;
                    }
                    logText.text +=
                        "\n" +
                        ColorString(prefix, stylePalette.System) +
                        FormatLine(consoleMessage.Lines[i]);
                }
            }
        }

        private string FormatLine(CMLine line)
        {
            StringBuilder formatString = new StringBuilder();
            foreach (Pair<string, CMStyle> segment in line)
            {
                switch (segment.Value2)
                {
                    case CMStyle.Default:
                        formatString.Append(ColorString(segment.Value1, stylePalette.Default));
                        break;
                    case CMStyle.Emphasis:
                        formatString.Append(ColorString(segment.Value1, stylePalette.Emphasis));
                        break;
                    case CMStyle.Warning:
                        formatString.Append(ColorString(segment.Value1, stylePalette.Warning));
                        break;
                    case CMStyle.Confirm:
                        formatString.Append(ColorString(segment.Value1, stylePalette.Confirm));
                        break;
                    case CMStyle.Object:
                        formatString.Append(ColorString(segment.Value1, stylePalette.Object));
                        break;
                    case CMStyle.System:
                        formatString.Append(ColorString(segment.Value1, stylePalette.System));
                        break;
                }
            }
            return formatString.ToString();
        }
        private string ColorString(string stringToColor, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + stringToColor + "</color>";
        }
        #endregion

        #region Animation
        public void SetVisibility(bool show)
        {
            if (show) Transition(toggleSpeed, true);
            else Transition(toggleSpeed, false);
        }
        private void Transition(float timeForEffect, bool setVisibility)
        {
            canvasGroup.interactable = setVisibility;
            if (setVisibility == true) inputField.ActivateInputField();

            if (CoTransition != null) StopCoroutine(CoTransition);
            CoTransition = FadeCanvas(
                setVisibility ? 1 : 0,
                timeForEffect,
                canvasGroup);

            StartCoroutine(CoTransition);
            visible = setVisibility;
            //    _Input.ConsoleInput = setVisibility;
        }

        private IEnumerator FadeCanvas(float targetAlpha, float time, CanvasGroup canvas)
        {
            float startAlpha = canvas.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / Time.timeScale / time)
            {
                canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }
            canvas.alpha = targetAlpha;
        }
        #endregion
    }
    */
}