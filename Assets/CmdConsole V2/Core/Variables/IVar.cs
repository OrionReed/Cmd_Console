using System;

namespace CmdConsole
{
    public interface IVar
    {
        string Name { get; set; }
        Type Type { get; set; }
    }
}