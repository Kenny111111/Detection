using System.Collections;
using UnityEngine;
using Detection;

public class EnemySurfaceManager : MonoBehaviour, IRevealable
{
    public int hitCount = 0;
    public int maxHit = 200;
    private int hitThreshold = 40;
    private DissolveController dissolveController;
    private bool runningReduceHitCount = false;
    private WaitForSeconds reduceHitInterval = new WaitForSeconds(0.1f);

    private void Update()
    {
        if (hitCount > hitThreshold && !dissolveController.revealed) Reveal();
        if (!runningReduceHitCount) StartCoroutine(ReduceHitCount());
    }

    private void Awake()
    {
        dissolveController = GetComponent<DissolveController>();
    }

    private IEnumerator ReduceHitCount()
    {
        runningReduceHitCount = true;
        while(hitCount > 0)
        {
            hitCount -= 25;
            yield return reduceHitInterval;
        }
        dissolveController.Disappear();
        runningReduceHitCount = false;
    }

    public void Reveal()
    {
        dissolveController.Appear();
    }
}
