using System.Collections;
using System.Collections.Generic;

namespace CmdConsole {

    public class CmdMessage : IEnumerable {
        public CmdMessageType Type { get; private set; }
        public List<KeyValuePair<string, CMStyle>> segments { get; private set; } = new List<KeyValuePair<string, CMStyle>> ();
        public CmdMessage (CmdMessageType type = CmdMessageType.Default) { Type = type; }
        public CmdMessage (string line, CmdMessageType type = CmdMessageType.Default) { Type = type; Add (line, CMStyle.Default); }
        public CmdMessage (string line, CMStyle color, CmdMessageType type = CmdMessageType.Default) { Type = type; Add (line, color); }
        public void Add (string segmentString, CMStyle segmentColor) => segments.Add (new KeyValuePair<string, CMStyle> (segmentString, segmentColor));

        public IEnumerator GetEnumerator () => segments.GetEnumerator ();
    }
    public enum CmdMessageType { Default, Warning, System }
}