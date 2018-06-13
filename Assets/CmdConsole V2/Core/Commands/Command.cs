using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CmdConsole
{
    public abstract class Command
    {
        public string Name { get; protected set; }
        public List<IVariable> Variables { get; protected set; } = new List<IVariable>();

        //public List<string> Aliases { get; protected set; }
        //public string Description { get; protected set; }

        public abstract CmdMessage Execute();
        public abstract CmdMessage ExecuteWithArgs(List<string> arguments);
    }
}
