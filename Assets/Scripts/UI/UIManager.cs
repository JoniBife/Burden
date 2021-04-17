using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        private Canvas _canvas;
        public GameObject FadeInPrefab;
        public GameObject FadeOutPrefab;

        // Use this for initialization
        private void Start()
        {
            _canvas = FindObjectOfType<Canvas>();
        }

        public void  PlayFadeOut(string text)
        {
            GameObject fadeOut = Instantiate(FadeOutPrefab, _canvas.transform);
            
            // If there is text then we place it on the screen
            if (!string.IsNullOrWhiteSpace(text)) {
                Text fadeOutText = fadeOut.GetComponentInChildren<Text>();
                fadeOutText.text = text;
            }
        }

        public void PlayFadeIn(string text)
        {
            GameObject fadeIn = Instantiate(FadeInPrefab, _canvas.transform);

            // If there is text then we place it on the screen
            if (!string.IsNullOrWhiteSpace(text))
            {
                Text fadeInText = fadeIn.GetComponentInChildren<Text>();
                fadeInText.text = text;
            }
        }
    }
}