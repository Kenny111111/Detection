using UnityEngine;

namespace Detection
{
    public class LookScanner : MonoBehaviour, IScans, IShootsParticle
    {
        [Header("Settings")]
        public Camera cam;

        [SerializeField] private int numParticlesPerSecond;
        [SerializeField] private float sprayAngleLoudnessVariance = 30f;
        [SerializeField] private float sprayAngleX;
        [SerializeField] private float sprayAngleY;
        [SerializeField] private float maxRayDistance;
        private float timeSinceLastSpawn;
        private float intervalPerSpawn;

        private MusicAnalyzer musicAnalyzer;
        private LayerMask layerMask;

        public void Awake()
        {
            intervalPerSpawn = 1 / (float)numParticlesPerSecond;
            musicAnalyzer = FindObjectOfType<MusicAnalyzer>();
            layerMask = LayerMask.GetMask("Environment", "Weapons", "Enemies", "MiscVisible");
        }

        public void Scan(Vector3 direction)
        {
            timeSinceLastSpawn += Time.deltaTime;
            float thisSprayAngleX = sprayAngleX + (musicAnalyzer.currentAvgLoudnessNormalized * sprayAngleLoudnessVariance);
            float thisSprayAngleY = sprayAngleY + (musicAnalyzer.currentAvgLoudnessNormalized * sprayAngleLoudnessVariance);

            if (timeSinceLastSpawn > intervalPerSpawn)
            {
                int numToSpawn = (int)Mathf.Floor(timeSinceLastSpawn / intervalPerSpawn);
                for (; numToSpawn > 0; numToSpawn--)
                {
                    timeSinceLastSpawn -= intervalPerSpawn;

                    float offsetXCoord = direction.x + Random.Range(-thisSprayAngleX, thisSprayAngleX);
                    float offsetYCoord = direction.y + Random.Range(-thisSprayAngleY, thisSprayAngleY);
                    Vector2 aimDir = new Vector2(offsetXCoord, offsetYCoord);
                    Ray directionRay = cam.ScreenPointToRay(aimDir);
                    ShootAndEmitParticle(directionRay);
                }
            }
        }

        public void ShootAndEmitParticle(Ray ray)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))
            {
                // if the object we collide with is scannable, then emit a particle at that location
                var scannableObject = hit.transform.gameObject.GetComponent<IScannable>();
                if (scannableObject == null) return;

                VFXEmitArgs overrideArgs = EffectManager.instance.effectEmitArgs;
                scannableObject.EmitParticle(hit, overrideArgs);
            }
        }
    }
}