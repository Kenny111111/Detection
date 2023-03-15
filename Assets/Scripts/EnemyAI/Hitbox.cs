using UnityEngine;
using Detection;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float hitboxMultipler = 1f;
    private ITakeDamage damagable;

    private void Start()
    {
        damagable = GetComponentInParent<ITakeDamage>();
    }

    public void Damage(float damage)
    {
        damagable.TakeDamage(damage * hitboxMultipler);
    }
}