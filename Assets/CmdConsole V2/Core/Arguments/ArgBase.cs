using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CmdConsole
{
    public abstract class ArgBase : IArg
    {
        public int PartCount { get; set; }
        public Type Type { get; set; }
        public string InputString { get; set; } = "";
        public string CurrentKey
        {
            get
            {
                Clamp();
                return GetOptions().ElementAtOrDefault(OptionIndex).Key;
            }
        }
        public object CurrentValue
        {
            get
            {
                Clamp();
                return GetOptions().ElementAtOrDefault(OptionIndex).Value;
            }
        }

        public SortedList<string, object> Options = new SortedList<string, object>();
        private int OptionIndex = 0;

        public ArgBase()
        {
        }
        public ArgBase(Type type)
        {
            Type = type;
        }
        public void SetInputString(string input)
        {
            InputString = input;
        }

        public abstract SortedList<string, object> GetOptions();

        public abstract void Init();

        public void IncrementOption()
        {
            Clamp();
            if (OptionIndex < Options.Count - 1)
            {
                OptionIndex++;
            }
        }
        public void DecrementOption()
        {
            Clamp();
            if (OptionIndex > 0)
            {
                OptionIndex--;
            }
        }
        private void Clamp()
        {
            OptionIndex = Mathf.Clamp(OptionIndex, 0, Options.Count > 0 ? Options.Count - 1 : 0);
        }
    }
}