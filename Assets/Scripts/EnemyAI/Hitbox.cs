using UnityEngine;

namespace Detection
{
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private float hitboxMultipler = 1f;
        private ITakeDamage damagable;

        private void Start()
        {

            ITakeDamage inParent = GetComponentInParent<ITakeDamage>();
            ITakeDamage inRoot = GetComponent<ITakeDamage>();
            if (inParent != null) damagable = inParent;
            else if (inRoot != null) damagable = inRoot;
        }

        public void Damage(IDealsDamage.Weapons weapon, float damage, AttackerType attacker)
        {
            damagable.TakeDamage(weapon, damage * hitboxMultipler, attacker);
        }
    }
}