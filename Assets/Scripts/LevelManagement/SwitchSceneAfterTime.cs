using System.Collections;
using UnityEngine;

namespace Detection
{
    public class SwitchSceneAfterTime : MonoBehaviour
    {
        public float timeToWaitInSeconds;

        // Start is called before the first frame update
        void Start()
        {
            if (timeToWaitInSeconds == 0) timeToWaitInSeconds = 1;

            StartCoroutine(SwitchAfterTime(timeToWaitInSeconds));
        }

        private static IEnumerator SwitchAfterTime(float timeToWait)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeToWait);
                GameManager.instance.TrySwitchToNextScene();
                break;
            }      
        }
    }
}
