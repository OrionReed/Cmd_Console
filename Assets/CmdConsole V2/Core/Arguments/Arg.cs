using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public class Arg : ArgBase
    {
        public Arg(Type type) : base(type)
        {
            Type = type;
        }

        public override void Init() { }

        public override SortedList<string, object> GetOptions()
        {
            if (InputString.CanParse(Type))
            {
                Options.Clear();
                Options.Add(InputString, InputString.Parse(Type));
                return Options;
            }
            else
            {
                Options.Clear();
                return Options;
            }
        }
    }
}