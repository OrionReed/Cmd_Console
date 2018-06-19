using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;

namespace CmdConsole
{
    public class CmdLog
    {
        public static TMP_Text LogText;
        public static CmdStylePalette Style;
        private static int maxMessages = 30;
        private static List<CmdMessage> messagesInternal = new List<CmdMessage>();
        private static List<string> messages = new List<string>();

        public static void Log(CmdMessage message)
        {
            messagesInternal.Add(message);

            messages.Add(FormatLine(message));

            if (messages.Count >= maxMessages)
                messages.RemoveRange(0, maxMessages - messages.Count);

            LogText.text = String.Join("\n", messages);
        }

        private static string FormatLine(CmdMessage line)
        {
            StringBuilder formatString = new StringBuilder();
            foreach (KeyValuePair<string, CMStyle> segment in line)
            {
                switch (segment.Value)
                {
                    case CMStyle.Default:
                        formatString.Append(segment.Key.ColorString(Style.Default));
                        break;
                    case CMStyle.Emphasis:
                        formatString.Append(segment.Key.ColorString(Style.Emphasis));
                        break;
                    case CMStyle.Warning:
                        formatString.Append(segment.Key.ColorString(Style.Warning));
                        break;
                    case CMStyle.Confirm:
                        formatString.Append(segment.Key.ColorString(Style.Confirm));
                        break;
                    case CMStyle.Object:
                        formatString.Append(segment.Key.ColorString(Style.Object));
                        break;
                    case CMStyle.System:
                        formatString.Append(segment.Key.ColorString(Style.System));
                        break;
                }
            }
            return formatString.ToString();
        }

    }
}
