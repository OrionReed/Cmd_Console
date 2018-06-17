using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public interface IArg
    {
        Type Type { get; }
        string InputString { get; }
        string CurrentKey { get; }
        object CurrentValue { get; }
        SortedList<string, object> GetOptions();
        void Init();
        void SetInputString(string input);
        void IncrementOption();
        void DecrementOption();
    }
}