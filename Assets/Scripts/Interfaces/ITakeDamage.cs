
namespace Detection
{
    public interface ITakeDamage
    {
        public void TakeDamage(IDealsDamage.Weapons weapon, float damage, AttackerType attacker);
        public void Die(IDealsDamage.Weapons weapon, AttackerType attacker);
    }
}
