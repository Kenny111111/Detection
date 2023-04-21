using UnityEngine;

namespace Detection
{
    public struct VFXEmitArgs
    {
        public Color? color;
        public float? size;

        public VFXEmitArgs(Color? newColor, float? newSize)
        {
            color = newColor;
            size = newSize;
        }
    }
    public interface IScannable
    {
        public void EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs);
    }
}