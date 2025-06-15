using System;
using TMPro;
using UnityEngine;

namespace VG.UI
{
    public class Typewriter : MonoBehaviour
    {
        [Tooltip("The text component to apply the typewriter effect to")]
        [SerializeField] private TMP_Text textComponent;

        [Tooltip("The speed of the typewriter effect in characters per second")]
        [SerializeField] [Min(0)] private float speed = 30;
        private int index;
        private Action onComplete;

        private string text;
        private float time;
        public bool IsTyping { get; private set; }

        private void LateUpdate()
        {
            if (IsTyping)
            {
                time += Time.deltaTime;
                var characters = (int)(time * speed);
                if (characters > 0)
                {
                    index += characters;
                    if (index >= text.Length)
                    {
                        index = text.Length;
                        IsTyping = false;

                        onComplete?.Invoke();
                        onComplete = null;
                    }

                    textComponent.text = text[..index];
                    time = 0;
                }
            }
        }

        public void Type(string text, bool displayFirstCharacterImmediately = true, Action onComplete = null)
        {
            this.text = text;
            this.onComplete = onComplete;

            index = 0;
            if (displayFirstCharacterImmediately)
            {
                time = 1f / speed;
                if (time < Time.deltaTime)
                    time = 0;
                else
                    time -= Time.deltaTime;
            }
            else
            {
                time = 0;
            }


            IsTyping = true;
        }

        public void Skip()
        {
            index = text.Length;
            textComponent.text = text;
            IsTyping = false;

            onComplete?.Invoke();
            onComplete = null;
        }

        public void Clear()
        {
            textComponent.text = "";
            IsTyping = false;

            onComplete = null;
        }
    }
}