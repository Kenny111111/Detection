using UnityEngine;

namespace Detection
{
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private float hitboxMultipler = 1f;
        private ITakeDamage damagable;

        private void Start()
        {
            damagable = GetComponentInParent<ITakeDamage>();
        }

        public void Damage(IDealsDamage.Weapons weapon, float damage, AttackerType attacker)
        {
            damagable.TakeDamage(weapon, damage * hitboxMultipler, attacker);
        }
    }
}