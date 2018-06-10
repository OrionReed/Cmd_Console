using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public abstract class BaseCommand : ICommand
    {
        public string Name { get; protected set; }
        public string Alias { get; protected set; }
        public string Description { get; protected set; }
        public List<IParam> Parameters { get; protected set; } = new List<IParam>();
        public Type DefaultType { get; protected set; }

        public abstract CmdMessage ProcessArgs(string parameter, List<string> args);
        public abstract CmdMessage ProcessDefault();
    }
}
