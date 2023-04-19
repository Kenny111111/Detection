using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace Detection 
{
    public class SetParticleEffectOnAwake : MonoBehaviour
    {
        [SerializeField] private VisualEffect toSetEffectPrefab;

        void Awake()
        {
            if (toSetEffectPrefab == null)
            {
                Debug.LogError("REQUIRED FIELD NOT SET: toSetEffectPrefab not set on level index: " + SceneManager.GetActiveScene().buildIndex);
                return;
            }

            ParticleCollector.instance.effectPrefab = toSetEffectPrefab;
        }
    }
}
