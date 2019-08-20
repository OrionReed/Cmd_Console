using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CmdConsole {
    [CreateAssetMenu (menuName = "CmdConsole/New Style Palette")]
    public class CmdStylePalette : ScriptableObject {
        public TMP_FontAsset Font;
        [Header ("Log Style")]
        public Color Default = new Color (0.729f, 0.729f, 0.729f);
        public Color Emphasis = new Color (0.901f, 0.901f, 0.901f);
        public Color Warning = new Color (0.639f, 0.149f, 0f);
        public Color Confirm = new Color (0.294f, 0.603f, 0.176f);
        public Color Object = new Color (0.882f, 0.807f, 0.717f);
        public Color System = new Color (0.341f, 0.341f, 0.341f);
        public string PrefixDefault = "";
        public string PrefixWarning = "[!]";

        public Color CaretDark { get; private set; } = new Color (0.2f, 0.2f, 0.2f);
        public Color CaretLight { get; private set; } = new Color (0.7f, 0.7f, 0.7f);
        public Color ParseWarning { get; private set; } = new Color (0.639f, 0.149f, 0f);
        public Color Autocomplete { get; private set; } = new Color (0.341f, 0.341f, 0.341f);
    }
    public enum CMStyle { Default, Emphasis, Warning, Confirm, Object, System }
}