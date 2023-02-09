using System.Collections;
using UnityEngine;

namespace Detection
{
    public class EnemyScannerSurface : MonoBehaviour, IScannable
    {
        private EnemySurfaceManager surfaceManager;

        private void Awake()
        {
            surfaceManager = GetComponentInParent<EnemySurfaceManager>();
        }

        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            if(surfaceManager.hitCount < surfaceManager.maxHit)
                surfaceManager.hitCount++;
        }
    }
}