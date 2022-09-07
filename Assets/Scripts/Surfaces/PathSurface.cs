using System.Collections.Generic;
using UnityEngine;

// user input starts main scanner
// main scanner explains how we should emit particles from a ray we shoot from the player camera
// 


namespace Detection
{
    public class PathSurface : MonoBehaviour, IScannable
    {
        [SerializeField] ParticleSystem _particleSystem;
        void IScannable.EmitParticle(Vector3 position, ParticleSystem overrideParticleSystem)
        {
            var emitArgs = new ParticleSystem.EmitParams();
            emitArgs.position = position;
            emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            if (overrideParticleSystem == null)
            {
                if (_particleSystem == null) return;
                _particleSystem.Emit(emitArgs, 1);
            }
            else overrideParticleSystem.Emit(emitArgs, 1);
        }
    }
}