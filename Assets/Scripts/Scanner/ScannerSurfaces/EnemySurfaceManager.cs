using System.Collections;
using UnityEngine;

namespace Detection
{
    public class EnemySurfaceManager : MonoBehaviour
    {
        public int hitCount = 0;
        public int maxHit { private set; get; } = 250;
        public int hitThreshold { private set; get; } = 25;
        public DissolveController dissolveController { private set; get; }
        public bool runningReduceHitCount { private set; get; } = false;
        private WaitForSeconds reduceHitInterval = new WaitForSeconds(0.1f);

        private void Awake()
        {
            dissolveController = GetComponent<DissolveController>();
        }

        private void Update()
        {
            if (hitCount >= hitThreshold && !runningReduceHitCount) StartCoroutine(ReduceHitCount());
        }

        public IEnumerator ReduceHitCount()
        {
            runningReduceHitCount = true;
            while (hitCount > 0)
            {
                hitCount -= 10;
                yield return reduceHitInterval;
            }
            dissolveController.Disappear();
            runningReduceHitCount = false;
        }
    }
}
