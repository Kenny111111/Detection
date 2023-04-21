using UnityEngine.UI;
using UnityEngine;

namespace Detection
{
    public class Timer : MonoBehaviour
    {
        private Text timerText;
        private float startTime;
        private bool finished = false;

        // Start is called before the first frame update
        void Start()
        {
            timerText = GameObject.FindWithTag("WristTimerText").GetComponent<Text>();
            if (timerText == null) Debug.LogError("Could not find WristTimerText component Text");
            startTime = Time.time; 
        }

        // Update is called once per frame
        void Update()
        {
            if (finished) return;

            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f3");

            timerText.text = minutes + ":" + seconds;
        }

        public void Finished()
        {
            finished = true;
            timerText.color = Color.yellow;
        }
    }
}