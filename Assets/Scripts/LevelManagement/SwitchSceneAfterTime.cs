using System.Collections;
using System.Collections.Generic;
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

            new WaitForSecondsRealtime(timeToWaitInSeconds);
            GameManager.instance.TrySwitchToNextScene();
        }
    }
}
