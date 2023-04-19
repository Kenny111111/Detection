using System.Collections;
using UnityEngine;
using TMPro;
using static Detection.ScoreManager;

namespace Detection
{
    public class NumberIncrementor : MonoBehaviour
    {
        public ScoreType scoreType;
        private TMP_Text textComponent;
        private int val;
        public int Value
        {
            get { return val; }
            set { UpdateValue(value); val = value; }
        }
        private Coroutine incrementRoutine = null;
        private WaitForSeconds waitTime;
        private float fps = 60.0f;                      // speed of number increment
        private float duration = 3f;                    // duration of animation


        private void Awake()
        {
            textComponent = GetComponent<TMP_Text>();
            waitTime = new WaitForSeconds(1f / fps);
        }

        private void Start()
        {
            
        }

        private void UpdateValue(int newVal)
        {
            if (incrementRoutine != null) StopCoroutine(incrementRoutine);

            incrementRoutine = StartCoroutine(Increment(newVal));
        }

        private IEnumerator Increment(int targetVal)
        {
            int prevValue = val;
            int step;

            if (targetVal - prevValue < 0) step = Mathf.FloorToInt((targetVal - prevValue) / (fps * duration));
            else step = Mathf.CeilToInt((targetVal - prevValue) / (fps * duration));

            if (prevValue < targetVal)
            {
                while (prevValue < targetVal)
                {
                    prevValue += step;
                    if (prevValue > targetVal) prevValue = targetVal;

                    textComponent.SetText(prevValue.ToString());
                    yield return waitTime;
                }
            }
            else
            {
                while (prevValue > targetVal)
                {
                    prevValue += step;
                    if (prevValue < targetVal) prevValue = targetVal;

                    textComponent.SetText(prevValue.ToString());
                    yield return waitTime;
                }
            }
        }
    }
}