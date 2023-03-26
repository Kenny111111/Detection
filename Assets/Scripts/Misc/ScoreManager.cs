using UnityEngine;

namespace Detection
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        private int score = 0;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            EnemyManager.OnEnemyDeath += HandleEnemyDeath;
        }

        private void HandleEnemyDeath(AttackerType attackerType, IDealsDamage.Weapons weapon)
        {
            
        }
    }
}