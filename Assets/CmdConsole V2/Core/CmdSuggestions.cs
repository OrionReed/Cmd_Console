using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace CmdConsole
{
    public class CmdSuggestions
    {
        public static int focusedArg = -1;

        public static void UpdateFocus()
        {
            if (CmdConsole.commandOptions.GetCurrentKey().Length + 1 >= CmdConsole.caretPos)
            {
                focusedArg = 0;
            }
            else
            {
                int newFocus = CmdConsole.commandOptions.GetCurrentKey().Length + 1;
                for (int i = 0; i < CmdConsole.argOptions.Count; i++)
                {
                    newFocus += CmdConsole.argOptions[i].GetCurrentKey().Length + 1;
                    if (newFocus >= CmdConsole.caretPos)
                    {
                        focusedArg = i + 1;
                    }
                }
            }
            Debug.Log("Suggestion Focus Changed to Arg <b>" + focusedArg + "</b>");
        }

        /*  public static void UpdateSuggestionWindow()
         {
             if (focusedArg < 0)
             {
                 CmdConsole.instance.SuggestionPanel.alpha = 0;
             }
             if (focusedArg == 0 && CmdConsole.commandOptions.Any()) //Update for command
             {
                 CmdConsole.instance.suggestionText.text = string.Empty;
                 for (int i = 0; i < CmdConsole.commandOptions.Options.Count; i++)
                 {

                 }
             }

             if (CmdConsole.argOptions.Any())
             {
                 CmdConsole.instance.suggestionText.text = "Found Options...";
                 CmdConsole.instance.SuggestionPanel.alpha = 1;
             }
             else
             {
                 CmdConsole.instance.suggestionText.text = "No Options Found...";
             }
         } */
    }
}
