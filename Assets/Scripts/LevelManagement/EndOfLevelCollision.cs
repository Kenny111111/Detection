using UnityEngine;

namespace Detection
{
    public class EndOfLevelCollision : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player")) {
                GameManager.instance.TrySwitchToNextScene();
             }
        }
    }
}
