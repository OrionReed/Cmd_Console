using System.Collections;
using UnityEngine;

namespace CmdConsole
{
    [RequireComponent(typeof(CmdConsole))]
    public class CmdConsoleAnimator : MonoBehaviour
    {
        [SerializeField] private float toggleSpeed = 0.1f;
        [SerializeField] private KeyCode toggleConsole = KeyCode.BackQuote;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool startVisible = true;

        private bool consoleVisible = false;
        private IEnumerator CoTransition;
        private CmdConsole console;

        public void SetVisibility(bool show)
        {
            if (show) Transition(toggleSpeed, true);
            else Transition(toggleSpeed, false);
        }
        public void SetVisibilityImmediate(bool show)
        {
            if (show) Transition(0, true);
            else Transition(0, false);
        }

        private void Start()
        {
            console = GetComponent<CmdConsole>();
            SetVisibilityImmediate(startVisible);
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleConsole))
            {
                SetVisibility(!consoleVisible);
            }
        }

        private void Transition(float timeForEffect, bool setVisibility)
        {
            canvasGroup.interactable = setVisibility;
            canvasGroup.blocksRaycasts = setVisibility;
            if (setVisibility == true)
                console.inputField?.ActivateInputField();

            if (CoTransition != null) StopCoroutine(CoTransition);
            CoTransition = FadeCanvas(
                setVisibility ? 1 : 0,
                timeForEffect,
                canvasGroup);

            StartCoroutine(CoTransition);
            consoleVisible = setVisibility;
        }

        private IEnumerator FadeCanvas(float targetAlpha, float time, CanvasGroup canvas)
        {
            float startAlpha = canvas.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / Time.timeScale / time)
            {
                canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }
            canvas.alpha = targetAlpha;
        }
    }
}
