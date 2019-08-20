using System;
using System.Collections.Generic;

namespace CmdConsole {
    public class Arg : ArgBase {
        public Arg (Type type) : base (type) {
            Type = type;
        }

        public override void Init () { }

        public override SortedList<string, object> GetOptions () {
            if (Input.CanParse (Type)) {
                Options.Clear ();
                Options.Add (Input, Input.Parse (Type));
                return Options;
            } else {
                Options.Clear ();
                return Options;
            }
        }
    }
}