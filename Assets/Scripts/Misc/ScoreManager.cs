using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        [Header("Point values")]
        [SerializeField] private int killPoints = 1000;             // How much kills are worth
        [SerializeField] private int specialPoints = 2000;          // How much special kills are worth
        [SerializeField] private int maxLevelTimePoints = 20000;    // Amount of points to give player with fastest speed
        [Header("Time values (seconds)")]
        [SerializeField] private float comboTimeFrame = 3.0f;       // Combo timer timeout(seconds)
        [SerializeField] private float minTimeInLevel = 1f;         // The minimum amount of time(seconds) it takes to complete a level
        [SerializeField] private float maxTimeInLevel = 300f;       // The maximum amount of time(seconds) it takes to complete a level

        private float difficultyModifier = 1.0f;
        private int currentComboCount = 0;                          // Number of kills in a combo
        private float runningComboTimer = 0f;                       // Running combo timer
        private float score = 0f;                                   // Final score at the end of the level
        private float currentComboScore = 0f;                       // Score of the combo player is in (reset on combo timer)
        private float killScore = 0f;                               // Running score for kills (reset on level fail)
        private float timeScore = 0f;                               // Score based on the time taken to beat level
        private bool comboTimerRunning = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            EnemyManager.OnEnemyDeath += HandleEnemyDeath;
            GameManager.AfterGameStateChanged += HandleGameStateChange;
        }

        private void OnDestroy()
        {
            EnemyManager.OnEnemyDeath -= HandleEnemyDeath;
            GameManager.AfterGameStateChanged -= HandleGameStateChange;
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
                    CalculateTimeScore(Time.timeSinceLevelLoad);
                    CalculateFinalScore();
                    break;
                default:
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

            if (currentComboCount >= 1)
                killScore += currentComboScore + currentComboScore * comboMultiplier * difficultyModifier;
            else
                killScore += currentComboScore * comboMultiplier * difficultyModifier;
        }

        private void CalculateTimeScore(float secondsInLevel)
        {
            if (secondsInLevel <= minTimeInLevel)
            {
                timeScore = maxLevelTimePoints;
            }
            else if (secondsInLevel >= maxTimeInLevel)
            {
                timeScore = 0;
            }
            else
            {
                timeScore = maxLevelTimePoints * Mathf.Pow(maxTimeInLevel - secondsInLevel, 2) / Mathf.Pow(maxTimeInLevel, 2);
            }
        }

        private void CalculateFinalScore()
        {
            score = killScore + timeScore;
            Debug.Log("Level Score: " + (int)score);
        }

        private void Reset()
        {
            comboTimerRunning = false;
            currentComboCount = 0;
            currentComboScore = 0f;
            killScore = 0f;
            timeScore = 0f;
            score = 0f;
        }

        private void SaveScore()
        {
            throw new NotImplementedException();
        }
    }
}