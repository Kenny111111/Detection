using UnityEngine;

namespace Detection
{
    public class EnemyScannerSurface : MonoBehaviour, IScannable, IRevealable
    {
        private EnemySurfaceManager surfaceManager;

        private void Awake()
        {
            surfaceManager = GetComponentInParent<EnemySurfaceManager>();
        }

        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            if (surfaceManager.hitCount < surfaceManager.maxHit) surfaceManager.hitCount++;
            if (surfaceManager.hitCount > surfaceManager.hitThreshold) Reveal();
        }

        public void Reveal()
        {
            surfaceManager.dissolveController.Appear();
        }
    }
}