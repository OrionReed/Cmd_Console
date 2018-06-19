using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CmdConsole
{
    public abstract class ArgBase : IArg
    {
        public int Parts { get; protected set; }
        public Type Type { get; protected set; }
        public string Input { get; protected set; } = "";
        public string CurrentKey { get { Clamp(); return GetOptions().ElementAtOrDefault(OptionIndex).Key; } }
        public object CurrentValue { get { Clamp(); return GetOptions().ElementAtOrDefault(OptionIndex).Value; } }
        public SortedList<string, object> Options { get; protected set; } = new SortedList<string, object>();

        private int OptionIndex = 0;

        public ArgBase() { }
        public ArgBase(Type type) { Type = type; Parts = CmdUtility.GetTypeParts(Type); }

        public abstract void Init();
        public abstract SortedList<string, object> GetOptions();
        public SortedList<string, object> GetOptions(string newInput) { Input = newInput; return GetOptions(); }
        public void SetInput(string input) { Input = input; }
        public void IncrementOption() { Clamp(); if (OptionIndex < Options.Count - 1) OptionIndex++; }
        public void DecrementOption() { Clamp(); if (OptionIndex > 0) OptionIndex--; }


        private void Clamp() => OptionIndex = Mathf.Clamp(OptionIndex, 0, Options.Count > 0 ? Options.Count - 1 : 0);
    }
}