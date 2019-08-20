using System.Collections.Generic;

namespace CmdConsole {
    public interface ICommand {
        string Name { get; }
        string Alias { get; }
        string Description { get; }
        List<IVar> Variables { get; }
        void ExecuteDefault ();
        void ExecuteWithArguments (List<object> arguments);
    }
}