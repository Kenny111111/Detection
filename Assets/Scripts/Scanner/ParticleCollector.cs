using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleCollector : MonoBehaviour
{
    public static ParticleCollector instance;
    private const int TEXTURE_2D_MAX_HEIGHT = 16384;

    private const string POSITIONS_TEXTURE_NAME = "Positions";
    private const string COLORS_TEXTURE_NAME = "Colors";
    private const string CAPACITY_PARAM_NAME = "Capacity";

    public VisualEffect effectPrefab;
    [SerializeField] private Transform effectContainer;

    private VisualEffect currentEffect;
    private Queue<VisualEffect> effects;

    private Texture2D pointsTexture2D;
    private Color[] points;

    private Texture2D colorsTexture2D;
    private Color[] colorsAndSizes;

    private int particleCount;
    [SerializeField] private int maxEffectCount = 45;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);

        points = new Color[TEXTURE_2D_MAX_HEIGHT];
        colorsAndSizes = new Color[TEXTURE_2D_MAX_HEIGHT];
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

        if (particleCount >= TEXTURE_2D_MAX_HEIGHT)
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
        CreateNewEffect();
    }

    // Apply a cached array of points (positions) to a texture 2D to be loaded into the VFX graph
    private void ApplyPoints()
    {
        // Update the texture
        pointsTexture2D.SetPixels(points);
        pointsTexture2D.Apply();
        // Update the current effect with the updated texture
        currentEffect.SetTexture(POSITIONS_TEXTURE_NAME, pointsTexture2D);

        // Update the texture
        colorsTexture2D.SetPixels(colorsAndSizes);
        colorsTexture2D.Apply();
        // Update the current effect with the updated texture
        currentEffect.SetTexture(COLORS_TEXTURE_NAME, colorsTexture2D);

        currentEffect.Reinit();
    }

    private void CreateNewEffect()
    {
        if (effectPrefab == null)
        {
            Debug.LogError("effectPrefab is not set, cant CreateNewEffect()");
            return;
        }

        // Ensure the VFX Graph is only rendering a certain number of effects..
        if (effects.Count > maxEffectCount) DeleteOldestEffect();

        currentEffect = Instantiate(effectPrefab, effectContainer);
        currentEffect.SetUInt(CAPACITY_PARAM_NAME, TEXTURE_2D_MAX_HEIGHT);
        effects.Enqueue(currentEffect);

        pointsTexture2D = new Texture2D(TEXTURE_2D_MAX_HEIGHT, 1, TextureFormat.RGBAFloat, false);
        Color defaultCol = new Color(0, 0, 0, 0);
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = defaultCol;
        }

        colorsTexture2D = new Texture2D(TEXTURE_2D_MAX_HEIGHT, 1, TextureFormat.RGBAFloat, false);
        for (int i = 0; i < colorsAndSizes.Length; i++)
        {
            colorsAndSizes[i] = defaultCol;
        }
        particleCount = 0;
    }

    private void DeleteOldestEffect()
    {
        var tempList = effects.ToList();

        if (tempList.Count > 0)
        {
            int removeIndex = 0;
            Destroy(tempList[removeIndex].gameObject);
            tempList.RemoveAt(removeIndex);

            effects = new Queue<VisualEffect>(tempList);
        }
    }
}
