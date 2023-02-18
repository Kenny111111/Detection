using UnityEngine;

namespace Detection
{
    public class LookScanner : MonoBehaviour, IScans, IShootsParticle
    {
        [Header("Settings")]
        public Camera cam;

        [SerializeField] private int numParticlesPerSecond;
        [SerializeField] private float sprayAngleX;
        [SerializeField] private float sprayAngleY;
        [SerializeField] private float maxRayDistance;
        private float timeSinceLastSpawn;

        public void Scan(Vector3 direction)
        {
            float intervalPerSpawn = 1 / (float)numParticlesPerSecond;
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn > intervalPerSpawn)
            {
                int numToSpawn = (int)Mathf.Floor(timeSinceLastSpawn / intervalPerSpawn);
                for (; numToSpawn > 0; numToSpawn--)
                {
                    timeSinceLastSpawn -= intervalPerSpawn;

                    float offsetXCoord = direction.x + Random.Range(-sprayAngleX, sprayAngleX);
                    float offsetYCoord = direction.y + Random.Range(-sprayAngleY, sprayAngleY);
                    Vector2 aimDir = new Vector2(offsetXCoord, offsetYCoord);
                    Ray directionRay = cam.ScreenPointToRay(aimDir);
                    ShootAndEmitParticle(directionRay);
                }
            }
        }

        public void ShootAndEmitParticle(Ray ray)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                // if the object we collide with is scannable, then emit a particle at that location
                var scannableObject = hit.transform.gameObject.GetComponent<IScannable>();
                if (scannableObject == null) return;

                VFXEmitArgs overrideArgs = EffectSystem.effectSystem.effectEmitArgs;
                scannableObject.EmitParticle(hit, overrideArgs);
            }
        }
    }
}