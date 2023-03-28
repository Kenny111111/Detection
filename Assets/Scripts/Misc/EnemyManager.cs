using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Detection
{
    public enum AttackerType { Player, Enemy }

    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;

        public static Action<AttackerType, IDealsDamage.Weapons> OnEnemyDeath;
        public static Action OnAllEnemiesDead;

        private Dictionary<Enemy, GameObject> enemies = new Dictionary<Enemy, GameObject>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            GameManager.PreGameStateChanged += HandleGameStateChange;
        }

        private void HandleEnemyDeath(Enemy enemy, IDealsDamage.Weapons weapon, AttackerType attackerType)
        {
            // Don't need to be removed if the enemies aren't destroyed when they die
            enemies.Remove(enemy);
            enemy.OnDeath -= HandleEnemyDeath;

            OnEnemyDeath?.Invoke(attackerType, weapon);

            if (enemies.Count == 0)
                OnAllEnemiesDead?.Invoke();
        }

        private void HandleGameStateChange(GameState gameState)
        {
            if(gameState == GameState.PREPARINGLEVEL)
            {
                Init();
            }
            else if(gameState == GameState.LEVELENDED)
            {
                Reset();
            }
        }

        private void Init()
        {
            Enemy[] enemyInstances = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemyInstances)
            {
                enemies.Add(enemy, enemy.gameObject);
                enemy.OnDeath += HandleEnemyDeath;
            }
        }

        private void Reset()
        {
            foreach (KeyValuePair<Enemy, GameObject> entry in enemies)
            {
                entry.Key.OnDeath -= HandleEnemyDeath;
            }

            enemies.Clear();
        }

        public List<GameObject> GetActiveEnemies()
        {
            return enemies.Values.ToList();
        }
    }
}