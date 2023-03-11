using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HitboxData", menuName="HitboxData/Data")]
public class EnemyHitboxData : ScriptableObject
{
    [System.Serializable]
    public struct HitboxData
    {
        public string bodyPart;
        public float multiplier;
    }

    [SerializeField]
    private List<HitboxData> hitboxData = new List<HitboxData>();

    private Dictionary<string, float> multipliers = new Dictionary<string, float>();

    public void Init()
    {
        foreach (HitboxData data in hitboxData)
            multipliers[data.bodyPart] = data.multiplier;
    }

    public float GetMultiplier(string name)
    {
        return multipliers[name];
    }
}