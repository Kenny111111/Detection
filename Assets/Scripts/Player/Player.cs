using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Detection
{
    public class Player : Combatant
    {
        private float difficultyModifier = 1f;
        private float regenPerTick = 1f;
        private WaitForSeconds healTick;

        private void Start()
        {
            health = 200 * difficultyModifier;
            maxHealth = health;
            healTick = new WaitForSeconds(1f);

            StartCoroutine(RegenOverTime());
        }

        public override void Die(IDealsDamage.Weapons weapon, AttackerType attacker)
        {
            StopCoroutine(RegenOverTime());
            GameManager.instance.UpdateGameState(GameState.PLAYERDIED);
        }

        public void InstantHeal()
        {
            health = maxHealth;
        }

        private IEnumerator RegenOverTime()
        {
            while (true)
            {
                if (health < maxHealth)
                    health = Mathf.Clamp(health + regenPerTick, health, maxHealth);

                yield return healTick;
            }
        }
    }
}