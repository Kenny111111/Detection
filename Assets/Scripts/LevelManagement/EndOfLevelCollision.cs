using UnityEngine;

namespace Detection
{
    public class EndOfLevelCollision : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            GameManager.instance.TrySwitchToNextScene();
        }
    }
}
