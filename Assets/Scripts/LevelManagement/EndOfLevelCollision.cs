using UnityEngine;
public class EndOfLevelCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameManager.instance.TryNextScene();
    }
}