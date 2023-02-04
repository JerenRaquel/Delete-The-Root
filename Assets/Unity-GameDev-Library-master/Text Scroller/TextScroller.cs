using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextSystem {
    public class TextScroller : MonoBehaviour {
        public TMPro.TextMeshProUGUI textBox;
        public FontData defaultFont;
        public float speedModifier;
        public TextData[] textChunks;
        public bool IsFinished { get { return this.isDone && this.index >= this.textChunks.Length; } }
        public bool IsWorking { get { return this.isWorking; } }

        private int index = 0;
        private bool isDone = false;
        private bool isWorking = false;

        public void Read() {
            if (!isDone && !isWorking && index < this.textChunks.Length) {
                isWorking = true;
                StartCoroutine(Scroll(this.textChunks[index]));
            }
        }

        public void Skip() {
            if (!isDone) {
                isDone = true;
                DisplayAll();
                isWorking = false;
            }
        }

        private void DisplayAll() {
            StopCoroutine(Scroll(this.textChunks[index]));
            TextData data = this.textChunks[index];
            string buffer = "";
            for (int i = 0; i < data.parts.Length; i++) {
                buffer += TextParser.ApplyEffectsAsString(data.parts[i], this.defaultFont);
                if (i < data.parts.Length) {
                    buffer += " ";
                }
            }
            this.textBox.text = buffer;
        }

        private IEnumerator Scroll(TextData data) {
            string buffer = "";
            for (int i = 0; i < data.parts.Length; i++) {
                TextParser.TextParts textFormatedPart
                    = TextParser.ApplyEffects(data.parts[i], this.defaultFont);

                string result = "";
                for (int textLength = 0; textLength < textFormatedPart.text.Length; textLength++) {
                    result = textFormatedPart.before;
                    result += textFormatedPart.text.Substring(0, textLength + 1);
                    result += textFormatedPart.after;

                    this.textBox.text = buffer + result;
                    yield return new WaitForSeconds(data.parts[i].charDelay * this.speedModifier);
                }
                buffer += result + " ";
            }
            isWorking = false;
            this.index++;
            if (this.index >= this.textChunks.Length) isDone = true;
        }
    }
}
