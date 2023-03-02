using UnityEngine;

namespace Detection
{
    public class Hitbox : MonoBehaviour
    {
        private ITakeDamage damagable;

        private void Start()
        {
            damagable = GetComponentInParent<ITakeDamage>();
        }

        public void Damage(float damage)
        {
            damagable.TakeDamage(damage);
        }
    }
}
