using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace CmdConsole
{
    public class ValidCommandList
    {
        private SortedList<string, Command> Options = new SortedList<string, Command>();
        public int CurrentSelectedIndex { get; private set; } = 0;

        public bool Any()
        {
            return Options.Any();
        }
        public void Clear()
        {
            Options.Clear();
        }
        public void Add(string key, Command value)
        {
            Options.Add(key, value);
        }

        public string GetCurrentKey()
        {
            return Options.Keys[CurrentSelectedIndex];
        }
        public Command GetCurrentValue()
        {
            return Options.Values[CurrentSelectedIndex];
        }

        public void Increment()
        {
            Clamp();
            if (CurrentSelectedIndex < Options.Count) CurrentSelectedIndex++;
        }
        public void Decrement()
        {
            Clamp();
            if (CurrentSelectedIndex > 0) CurrentSelectedIndex--;
        }

        private void Clamp()
        {
            CurrentSelectedIndex = Mathf.Clamp(CurrentSelectedIndex, 0, Options.Count);
        }
    }
}
