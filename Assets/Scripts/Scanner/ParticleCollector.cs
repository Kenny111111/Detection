using Detection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleCollector : MonoBehaviour
{
    public static ParticleCollector instance;
    private const int Texture2DMaxHeight = 16384;

    private const string PositionsTextureName = "Positions";
    private const string ColorsTextureName = "Colors";
    private const string CapacityParamName = "Capacity";

    [SerializeField] public VisualEffect effectPrefab;
    [SerializeField] private Transform effectContainer;
    [SerializeField] private int maxEffectCount = 45;

    private VisualEffect currentEffect;
    private Queue<VisualEffect> effects;

    private Texture2D pointsTexture2D;
    private Color[] points;

    private Texture2D colorsTexture2D;
    private Color[] colorsAndSizes;

    private int particleCount;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        points = new Color[Texture2DMaxHeight];
        colorsAndSizes = new Color[Texture2DMaxHeight];
        effects = new Queue<VisualEffect>();
    }

    private void Start()
    {
        CreateNewEffect();
        ApplyPoints();
    }

    private void FixedUpdate()
    {
        ApplyPoints();
    }

    public void CachePoint(Vector3 position, Color color, float size)
    {
        points[particleCount] = new Color(position.x, position.y, position.z);
        colorsAndSizes[particleCount] = new Color(color.r, color.g, color.b, size);

        particleCount++;

        if (particleCount >= Texture2DMaxHeight)
        {
            CreateNewEffect();
        }
    }

    public void ClearAllPoints()
    {
        while (effects.Count > 0)
        {
            Destroy(effects.Dequeue().gameObject);
        }
    }

    private void ApplyPoints()
    {
        pointsTexture2D.SetPixels(points);
        pointsTexture2D.Apply();
        currentEffect.SetTexture(PositionsTextureName, pointsTexture2D);

        colorsTexture2D.SetPixels(colorsAndSizes);
        colorsTexture2D.Apply();
        currentEffect.SetTexture(ColorsTextureName, colorsTexture2D);

        currentEffect.Reinit();
    }

    private void CreateNewEffect()
    {
        if (effectPrefab == null)
        {
            Debug.LogError("effectPrefab is not set, cannot CreateNewEffect()");
            return;
        }

        if (effects.Count > maxEffectCount)
        {
            DeleteOldestEffect();
        }

        currentEffect = Instantiate(effectPrefab, effectContainer);
        currentEffect.SetUInt(CapacityParamName, Texture2DMaxHeight);
        effects.Enqueue(currentEffect);

        pointsTexture2D = new Texture2D(Texture2DMaxHeight, 1, TextureFormat.RGBAFloat, false);
        Color defaultColor = new Color(0, 0, 0, 0);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = defaultColor;
        }

        colorsTexture2D = new Texture2D(Texture2DMaxHeight, 1, TextureFormat.RGBAFloat, false);
        for (int i = 0; i < colorsAndSizes.Length; i++)
        {
            colorsAndSizes[i] = defaultColor;
        }
        particleCount = 0;
    }

    private void DeleteOldestEffect()
    {
        if (effects.Count == 0)
        {
            return;
        }

        var effectToDelete = effects.Dequeue();
        Destroy(effectToDelete.gameObject);
    }

    public void SetEffectPrefab(VisualEffect newEffectPrefab)
    {
        effectPrefab = newEffectPrefab;
    }
}
