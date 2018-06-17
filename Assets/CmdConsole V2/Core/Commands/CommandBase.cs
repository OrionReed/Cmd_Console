using System.Collections.Generic;

namespace CmdConsole
{
    public abstract class CommandBase : ICommand
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Alias { get; protected set; }
        public List<IVar> Variables { get; protected set; } = new List<IVar>();
        public abstract CmdMessage ExecuteDefault();
        public abstract CmdMessage ExecuteWithArguments(List<object> arguments);
    }
}
