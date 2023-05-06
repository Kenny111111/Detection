using UnityEngine;

namespace Detection
{
    public class BasicScannerSurface : MonoBehaviour, IScannable
    {
        [SerializeField] private Color defaultColor = new Color(255, 255, 255);
        [SerializeField] private float defaultSize = 0.015f;

        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            Color color = defaultColor;
            float size = defaultSize;
            if (overrideArgs.color != null) color = (Color)overrideArgs.color;
            if (overrideArgs.size != null) size = (float)overrideArgs.size;

            ParticleCollector.instance.CachePoint(hit.point, color, size);
        }
    }
}