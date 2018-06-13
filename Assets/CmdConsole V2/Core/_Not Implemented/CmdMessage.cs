using System.Collections;
using System.Collections.Generic;

namespace CmdConsole
{

    public class CmdMessage
    {
        public List<CMLine> Lines { get; private set; } = new List<CMLine>();

        #region Constructors
        public CmdMessage() { }
        public CmdMessage(CMLine firstLine) { AddLine(firstLine); }
        public CmdMessage(string firstLine, CMLineType type = CMLineType.Default) { AddLine(firstLine, type); }
        public CmdMessage(string firstLine, CMStyle color, CMLineType type = CMLineType.Default) { AddLine(firstLine, type); }
        #endregion

        #region Methods
        public void AddLine(CMLine line) => Lines.Add(line);
        public void AddLine(string line, CMLineType type = CMLineType.Default) => Lines.Add(new CMLine(line, type));
        public void AddLine(string line, CMStyle color, CMLineType type = CMLineType.Default) => Lines.Add(new CMLine(line, color, type));
        public void AddLineBreak() => Lines.Add(new CMLine(""));
        #endregion
    }

    public enum CMLineType { Default, Warning }

    public class CMLine : IEnumerable
    {
        public CMLineType Type { get; private set; }
        public CMLine(CMLineType type = CMLineType.Default) { Type = type; }
        public CMLine(string line, CMLineType type = CMLineType.Default)
        {
            Type = type;
            Add(line, CMStyle.Default);
        }
        public CMLine(string line, CMStyle color, CMLineType type = CMLineType.Default)
        {
            Type = type;
            Add(line, color);
        }
        private List<Pair<string, CMStyle>> _list = new List<Pair<string, CMStyle>>();

        public void Add(string segmentString, CMStyle segmentColor)
        {
            _list.Add(new Pair<string, CMStyle>(segmentString, segmentColor));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
    public class Pair<T1, T2>
    {
        public Pair(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
    }
}