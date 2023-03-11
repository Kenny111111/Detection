using UnityEngine;
using Detection;

public class Hitbox : MonoBehaviour
{
    private ITakeDamage damagable;
    private float hitboxMultipler = 1f;

    private void Start()
    {
        damagable = GetComponentInParent<ITakeDamage>();
    }

    public void Damage(float damage)
    {
        damagable.TakeDamage(damage * hitboxMultipler);
    }

    public void Init(EnemyHitboxData hitboxData)
    {
        hitboxMultipler = hitboxData.GetMultiplier(gameObject.name);
    }
}