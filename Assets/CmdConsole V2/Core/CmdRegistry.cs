using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace CmdConsole
{
    public static class CmdRegistry
    {
        public static SortedList<string, ICommand> Commands = new SortedList<string, ICommand>();
        public static void Init()
        {
            RegisterAll();
            ValidateAll();
        }
        private static void RegisterAll()
        {
            var allCommands = AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(x => x.GetTypes())
              .Where(x => typeof(ICommand).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
              .Select(x => Activator.CreateInstance(x)).ToList();

            foreach (ICommand command in allCommands)
            {
                if (command.Name == "" || command.Name == String.Empty || command.Name == null)
                {
                    Debug.LogError("Command <b>" + command + "</b> is missing a name so it has not been added.");
                    continue;
                }
                Commands.Add(command.Name, command);
            }
        }

        private static void ValidateAll()
        {
            foreach (KeyValuePair<string, ICommand> command in Commands)
            {
                // VALIDATE
            }
        }
    }
}
