using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace Detection 
{
    public class SetParticleEffectOnStart : MonoBehaviour
    {
        [SerializeField] private VisualEffect toSetEffectPrefab;

        void Start()
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
