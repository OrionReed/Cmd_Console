using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public interface IArg
    {
        Type Type { get; }
        int Parts { get; }
        string Input { get; }
        string CurrentKey { get; }
        object CurrentValue { get; }
        SortedList<string, object> GetOptions();
        void Init();
        void SetInput(string input);
        void IncrementOption();
        void DecrementOption();
    }
}