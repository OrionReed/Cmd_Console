using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public interface ICommand
    {
        string Name { get; }
        string Alias { get; }
        string Description { get; }
        List<IParam> Parameters { get; }
        Type DefaultType { get; }
        CmdMessage ProcessDefault();
        CmdMessage ProcessArgs(string parameter, List<string> args);
    }
}
