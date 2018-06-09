using System;

namespace CmdConsole
{
    public interface IParam
    {
        string Name { get; }
        string Description { get; }
        string ValueString();
    }
    public interface IParam<T> : IParam
    {
        bool CanParseValue(string parseString);
        T ParseValue(string parseString);
    }
}