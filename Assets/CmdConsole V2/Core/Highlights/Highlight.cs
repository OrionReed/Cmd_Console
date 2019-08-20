using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace CmdConsole {
    public class Highlight {
        public HighlightState State { get; private set; } = HighlightState.Inactive;
        public RectTransform PanelRect { get; private set; }
        public TMP_Text Text { get; private set; }
        public CanvasGroup _CanvasGroup { get; private set; }
        public Image _Image { get; private set; }

        public Highlight (RectTransform rect, TMP_Text text, CanvasGroup canvasGroup, Image image) {
            PanelRect = rect;
            Text = text;
            _CanvasGroup = canvasGroup;
            _Image = image;
        }

        public void ChangeState (HighlightState newState) {
            switch (newState) {
                case HighlightState.Inactive:
                    break;
                case HighlightState.Multipart:
                    break;
                case HighlightState.Error:
                    break;
                case HighlightState.OptionList:
                    break;

            }
        }

        public void ChangeFocus (IArg argFocus) {
            PanelRect.localPosition = new Vector2 (
                CmdConsole.InputField.textComponent.rectTransform.localPosition.x + CmdConsole.HighlightOffset.x + (argFocus.Position * CmdConsole.charWidth),
                CmdConsole.InputField.textComponent.rectTransform.localPosition.y + CmdConsole.HighlightOffset.y);
            _CanvasGroup.alpha = argFocus.Input.Any () ? 1 : 0;
        }
    }

}

public enum HighlightState {
    Inactive,
    Multipart,
    Error,
    OptionList,
}