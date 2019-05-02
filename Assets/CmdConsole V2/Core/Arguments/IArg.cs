using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public interface IArg
    {
        Type Type { get; }
        int Parts { get; }
        int Position { get; }
        string Input { get; }
        string CurrentKey { get; }
        object CurrentValue { get; }
        SortedList<string, object> GetOptions();
        KeyValuePair<string, object> GetOptionAtIndex(int index);
        void Init();
        void SetInput(string input);
        void SetPosition(int position);
        void IncrementOption();
        void DecrementOption();
    }
}