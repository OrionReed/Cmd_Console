using System.Collections.Generic;

namespace CmdConsole
{
    public interface ICommand
    {
        string Name { get; }
        string Alias { get; }
        string Description { get; }
        List<IVar> Variables { get; }
        CmdMessage ExecuteDefault();
        CmdMessage ExecuteWithArguments(List<object> arguments);
    }
}
