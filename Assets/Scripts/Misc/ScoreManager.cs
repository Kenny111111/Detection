using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        [SerializeField] private float killPoints;                // How much kills are worth
        [SerializeField] private float specialPoints;             // How much special kills are worth
        [SerializeField] private float comboTimeFrame = 3.0f;     // Combo timer max timeout time
        public float difficultyModifier = 1.0f;
        private float score = 0;
        private float levelStartTime = 0f;
        private int currentComboCount = 0;              // number of kills in a combo
        private float currentComboScore = 0f;           // the combo the player is in
        private bool comboTimerRunning = false;
        private float runningComboTimer = 0f;           // running combo timer
        private float currentLevelScore = 0f;           // current level score (reset on level fail)

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            EnemyManager.OnEnemyDeath += HandleEnemyDeath;
            GameManager.PreGameStateChanged += HandleGameStateChange;
        }

        private void HandleEnemyDeath(AttackerType attackerType, IDealsDamage.Weapons weapon)
        {
            CheckForSpecialScoring(attackerType, weapon);
            if (comboTimerRunning)
            {
                runningComboTimer = 0f; // Reset timer on each kill
                ++currentComboCount;
            }
            else
            {
                StartCoroutine(ComboTimer());
            }
        }

        private void CheckForSpecialScoring(AttackerType attackerType, IDealsDamage.Weapons weapon)
        {
            if (attackerType == AttackerType.Enemy) // Enemy killed enemy
            {
                currentComboScore += specialPoints;
            }
            else // Player killed enemy
            {
                currentComboScore += killPoints;
            }
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch(gameState)
            {
                case GameState.PREPARINGLEVEL:
                    Reset();
                    break;
                case GameState.LEVELENDED:
                    CalculateTimeScore();
                    //SaveScore();
                    break;
            }
        }

        private IEnumerator ComboTimer()
        {
            comboTimerRunning = true;
            runningComboTimer = 0f;
            while (runningComboTimer <= comboTimeFrame)
            {
                runningComboTimer += Time.deltaTime;
                yield return null;
            }

            CalculateComboScore();

            currentComboCount = 0;
            currentComboScore = 0;
            comboTimerRunning = false;
        }

        private void CalculateComboScore()
        {
            float comboMultiplier = 1.0f + (currentComboCount * 0.1f);
            currentLevelScore += currentComboScore + (currentComboScore * comboMultiplier) * difficultyModifier;
        
            //Debug.Log(currentLevelScore);
        }

        private void CalculateTimeScore()
        {
            float totalLevelTime = Time.time - levelStartTime;

            // score based on time...
        }

        private void Reset()
        {
            comboTimerRunning = false;
            currentComboScore = 0f;
            currentLevelScore = 0f;
            currentComboCount = 0;

            // Get starting time for timer
            levelStartTime = Time.time;
        }

        private void SaveScore()
        {
            throw new NotImplementedException();
        }
    }
}