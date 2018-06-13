using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace CmdConsole
{
    public class ValidArgList
    {
        private List<SortedList<string, object>> Options = new List<SortedList<string, object>>(CmdConsole.maxArgs);
        public int CurrentSelectedIndex { get; private set; } = 0;

        public bool Any(int argIndex)
        {
            return Options[argIndex].Any();
        }
        public void Clear(int argIndex)
        {
            Options.Clear();
        }
        public void Add(int argIndex, string key, object value)
        {
            if (Options.ElementAtOrDefault(argIndex) != null)
                Options[argIndex].Add(key, value);
        }

        public string GetKey(int argIndex)
        {
            return Options[argIndex].Keys[CurrentSelectedIndex];
        }
        public object GetValue(int argIndex)
        {
            return Options[argIndex].Values[CurrentSelectedIndex];
        }

        public List<object> GetValueList()
        {
            List<object> valueList = new List<object>();
            for (int i = 0; i < Options.Count; i++)
            {
                valueList.Add(Options[i].Values[0]);
            }
            return valueList;
        }

        public void Increment(int argIndex)
        {
            Clamp(argIndex);
            if (CurrentSelectedIndex < Options.Count) CurrentSelectedIndex++;
        }
        public void Decrement(int argIndex)
        {
            Clamp(argIndex);
            if (CurrentSelectedIndex > 0) CurrentSelectedIndex--;
        }

        private void Clamp(int argIndex)
        {
            CurrentSelectedIndex = Mathf.Clamp(CurrentSelectedIndex, 0, Options.Count);
        }
    }
}
