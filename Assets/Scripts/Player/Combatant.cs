using UnityEngine;

namespace Detection
{
    public class Combatant : MonoBehaviour, ITakeDamage
    {
        protected float health;
        protected float maxHealth;

        public void TakeDamage(IDealsDamage.Weapons weapon, float damage, AttackerType attacker)
        {
            health -= damage;
            if (health <= 0)
                Die(weapon, attacker);
        }

        public virtual void Die(IDealsDamage.Weapons weapon, AttackerType attacker)
        {

        }
    }
}
