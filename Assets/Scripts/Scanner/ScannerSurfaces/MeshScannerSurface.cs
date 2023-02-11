using UnityEngine;

namespace Detection
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshCollider))]
    public class MeshScannerSurface : MonoBehaviour, IScannable
    {
        [SerializeField] private Color defaultColor = new Color(255, 0, 0);
        [SerializeField] private float defaultLifetime = 3f;
        [SerializeField] private float defaultSize = 0.015f;

        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            Color color = defaultColor;
            float lifetime = defaultLifetime;
            float size = defaultSize;

            if (overrideArgs.color != null) color = (Color)overrideArgs.color;
            else
            {
                // Get the color at the specific point on the mesh texture
                Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
                Texture2D texture2D = renderer.material.mainTexture as Texture2D;

                // sometimes not able to get texture2d from texture? in this case we use default color
                if (texture2D)
                {
                    Vector2 pCoord = hit.textureCoord;
                    pCoord.x *= texture2D.width;
                    pCoord.y *= texture2D.height;

                    Vector2 tiling = renderer.material.mainTextureScale;
                    int x = Mathf.FloorToInt(pCoord.x * tiling.x);
                    int y = Mathf.FloorToInt(pCoord.y * tiling.y);

                    color = texture2D.GetPixel(x, y);
                }
            }

            if (overrideArgs.lifetime != null) lifetime = (float)overrideArgs.lifetime;
            if (overrideArgs.size != null) size = (float)overrideArgs.size;

            ParticleSpawner.spawner.Spawn(color, hit.point, lifetime, size);
        }
    }
}