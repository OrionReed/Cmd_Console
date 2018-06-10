using UnityEngine;
using TMPro;

namespace CmdConsole
{
    [CreateAssetMenu(menuName = "CmdConsole/New Style Palette")]
    public class CmdStylePalette : ScriptableObject
    {
        public Color Default = new Color(0.729f, 0.729f, 0.729f);
        public Color Emphasis = new Color(0.901f, 0.901f, 0.901f);
        public Color Warning = new Color(0.639f, 0.149f, 0f);
        public Color Confirm = new Color(0.294f, 0.603f, 0.176f);
        public Color Object = new Color(0.882f, 0.807f, 0.717f);
        public Color System = new Color(0.341f, 0.341f, 0.341f);
        public TMP_FontAsset Font;
        public string PrefixDefault = ">";
        public string PrefixWarning = "[!]";
    }
    public enum CMStyle { Default, Emphasis, Warning, Confirm, Object, System }
}
