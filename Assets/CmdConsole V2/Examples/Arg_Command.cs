using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace CmdConsole
{
    public class Arg_Command : ArgBase
    {
        public Arg_Command()
        {
            PartCount = 1;
            Type = typeof(ICommand);
        }

        public override void Init() { }

        public override SortedList<string, object> GetOptions()
        {
            Options.Clear();
            List<KeyValuePair<string, ICommand>> coms;

            coms = CmdRegistry.Commands.Where(c => c.Key.StartsWith(
                InputString, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            foreach (KeyValuePair<string, ICommand> foundCommand in coms)
            {
                Options.Add(foundCommand.Key, foundCommand.Value);
            }
            return Options;
        }
    }
}