using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace CmdConsole
{
    /// Checks if newly-updated arguments are valid and updates Command and Argument OptionLists
    public static class CmdParser
    {
        private static List<string> currentInput = new List<string>();
        private static List<string> newInput = new List<string>();
        private static List<string> argInput = new List<string>();

        public static void OnInputStringChange(string input)
        {
            newInput = Regex.Split(input, @"\s").Where(s => s.Length != 0).ToList<string>();
            if (new HashSet<string>(newInput).SetEquals(currentInput))
            {
                Debug.LogWarning("Nothing changed but OnInputStringChanged was called");
                return;
            }
            if (!newInput.Any())
            {
                Debug.LogWarning("Nothing being parsed");
                return;
            }
            else if (newInput[0] != currentInput.ElementAtOrDefault(0))
            {
                Debug.Log("Searching commands...");
                FindMatchingCommands(newInput[0]);
            }
            currentInput = newInput;
            newInput.RemoveAt(0);
            if (newInput.Count > 0)
            {
                ParseAllArguments(newInput);
            }
        }

        private static void ParseAllArguments(List<string> args)
        {
            for (int i = 0; i < CmdConsole.commands.GetCurrentValue().Variables.Count; i++)
            {
                CmdConsole.arguments.
            }
        }

        private static void FindMatchingCommands(string commandString)
        {
            List<Command> foundCommands = CmdRegistry.Commands
                .Where(c => c.Key.StartsWith(commandString, StringComparison.InvariantCultureIgnoreCase))
                .Select(c => c.Value)
                .ToList();

            if (foundCommands.Any())
            {
                CmdConsole.commands.Clear();
                foreach (Command c in foundCommands)
                {
                    CmdConsole.commands.Add(c.Name, c);
                }
            }
            else
            {
                CmdConsole.commands.Clear();
            }
        }
    }
}