using System.Collections;
using UnityEngine;

namespace CmdConsole
{
    [RequireComponent(typeof(CmdConsole))]
    public class CmdConsoleAnimator : MonoBehaviour
    {
        [SerializeField] private float toggleSpeed = 0.15f;
        [SerializeField] private KeyCode toggleConsole = KeyCode.BackQuote;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool startVisible = true;
        private bool consoleVisible = false;
        private IEnumerator CoTransition;
        private CmdConsole console;

        private void Start()
        {
            console = GetComponent<CmdConsole>();
            SetVisibility(startVisible);
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleConsole))
            {
                SetVisibility(!consoleVisible);
            }
        }

        public void SetVisibility(bool show, bool immediate = false)
        {
            if (show) Transition(immediate ? 0 : toggleSpeed, true);
            else Transition(immediate ? 0 : toggleSpeed, false);
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
