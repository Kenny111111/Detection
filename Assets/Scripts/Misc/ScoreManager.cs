using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class ScoreManager : MonoBehaviour
    {
        public enum ScoreType
        {
            TOTAL, KILL, COMBO, TIME
        }

        public static ScoreManager instance;

        [Header("TESTING ONLY(requires PREPARINGLEVEL)")]
        public bool testScoreSystem;
        public float minRandKillTime;
        public float maxRandKillTime;

        [Header("Point values")]
        [SerializeField] private int killPoints = 1000;             // How much kills are worth
        [SerializeField] private int specialKillPoints = 2000;      // How much special kills are worth
        [SerializeField] private int maxLevelTimePoints = 20000;    // Amount of points to give player with fastest speed
        [Header("Time values (seconds)")]
        [SerializeField] private float comboTimeFrame = 3.0f;       // Combo timer timeout(seconds)
        [SerializeField] private float minTimeInLevel = 1f;         // The minimum amount of time(seconds) it takes to complete a level
        [SerializeField] private float maxTimeInLevel = 300f;       // The maximum amount of time(seconds) it takes to complete a level
        private float difficultyModifier = 1.0f;

        private int runningComboCount = 0;                          // Number of kills in a combo
        private float runningComboTimer = 0f;                       // Running combo timer
        private float runningComboScore = 0f;                       // Score of the combo player is in (reset on combo timer)
        private float levelStartTime = 0f;                          // Reset on GameState.PLAYINGMISSION; used when player finishes the level
        private bool comboTimerRunning = false;

        // Final values at the end of the level
        private float totalScore = 0f;                              // Final score at the end of the level
        private float killScore = 0f;                               // Final kill score at the end of the level
        private float comboScore = 0f;                              // Final combo score
        private float timeScore = 0f;                               // Score based on the time taken to beat level

        public int TotalScore { get; private set; } = 0;
        public int KillScore { get; private set; } = 0;
        public int ComboScore { get; private set; } = 0;
        public int TimeScore { get; private set; } = 0;

        GameState prevGameState = GameState.DEFAULT;
        public static Action ScoreCalculated;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);

            EnemyManager.OnEnemyDeath += HandleEnemyDeath;
            GameManager.OnGameStateChanged += HandleGameStateChange;
        }

        private void OnDestroy()
        {
            EnemyManager.OnEnemyDeath -= HandleEnemyDeath;
            GameManager.OnGameStateChanged -= HandleGameStateChange;
        }

        private void HandleEnemyDeath(AttackerType attackerType, IDealsDamage.Weapons weapon)
        {
            CheckForSpecialScoring(attackerType, weapon);
            if (comboTimerRunning)
            {
                runningComboTimer = 0f; // Reset timer on each kill
                ++runningComboCount;
                CalculateComboScore();
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
                runningComboScore += specialKillPoints;
                killScore += specialKillPoints;
            }
            else // Player killed enemy
            {
                runningComboScore += killPoints;
                killScore += killPoints;
            }
        }

        private void HandleGameStateChange(GameState gameState)
        {
            switch(gameState)
            {
                case GameState.PREPARINGLEVEL:
                    if (testScoreSystem) StartCoroutine(TestScoreSystem());
                    break;
                case GameState.PLAYINGMISSION:
                    // Start/Restart level timer
                    levelStartTime = Time.time;
                    break;
                case GameState.MISSIONSTATISTICS:
                    CalculateFinalScore();
                    SaveScore();
                    ScoreCalculated?.Invoke();
                    break;
                case GameState.LEVELENDED:
                    if(prevGameState == GameState.MISSIONCLEARED)
                    {
                        CalculateTimeScore(Time.time - levelStartTime);
                    }
                    else if(prevGameState == GameState.MISSIONSTATISTICS)
                    {
                        Reset();
                    }
                    break;
                default:
                    break;
            }

            prevGameState = gameState;
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

            runningComboCount = 0;
            runningComboScore = 0;
            comboTimerRunning = false;
        }

        private void CalculateComboScore()
        {
            float comboMultiplier = 1.0f + (runningComboCount * 0.1f);
            comboScore += (runningComboScore + runningComboScore) * comboMultiplier * difficultyModifier;
        }

        private void CalculateTimeScore(float secondsInLevel)
        {
            if (secondsInLevel <= minTimeInLevel)
            {
                timeScore += maxLevelTimePoints;
            }
            else if (secondsInLevel >= maxTimeInLevel)
            {
                timeScore += 0;
            }
            else
            {
                timeScore += maxLevelTimePoints * Mathf.Pow(maxTimeInLevel - secondsInLevel, 2) / Mathf.Pow(maxTimeInLevel, 2);
            }
        }

        private void CalculateFinalScore()
        {
            totalScore = killScore + comboScore + timeScore;
        }

        private void Reset()
        {
            comboTimerRunning = false;
            runningComboCount = 0;
            runningComboScore = 0f;
            runningComboTimer = 0f;
          
            totalScore = 0f;
            killScore = 0f;
            comboScore = 0f;
            timeScore = 0f;
        }

        private void SaveScore()
        {
            TotalScore = (int)totalScore;
            KillScore = (int)killScore;
            ComboScore = (int)comboScore;
            TimeScore = (int)timeScore;

            if (testScoreSystem)
            {
                Debug.Log("<color=green>Final Score values:</color>");
                Debug.Log("<color=green>totalScore:</color> " + TotalScore);
                Debug.Log("<color=green>killScore:</color> " + KillScore);
                Debug.Log("<color=green>comboScore:</color> " + ComboScore);
                Debug.Log("<color=green>timeScore:</color> " + TimeScore);
            }
        }

        private IEnumerator TestScoreSystem()
        {
            List<KeyValuePair<Enemy, GameObject>> enemies = EnemyManager.instance.GetActiveEnemies();
            while(enemies.Count == 0)
            {
                yield return new WaitForSeconds(1);
                enemies = EnemyManager.instance.GetActiveEnemies();
            }

            foreach(var enemyKeyValue in enemies)
            {
                // 0=none, 1=knife, 2=pistol, 3=dbshotgun, 4=rifle, 5=grenade
                IDealsDamage.Weapons weapon = (IDealsDamage.Weapons)UnityEngine.Random.Range(2, 5);
                AttackerType attackerType;
                if (UnityEngine.Random.value < 0.5f) attackerType = AttackerType.Player;
                else                                 attackerType = AttackerType.Enemy;

                enemyKeyValue.Key.Die(weapon, attackerType);
                yield return new WaitForSeconds(UnityEngine.Random.Range(minRandKillTime, maxRandKillTime));
            }

            //yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 10f)); // add some time after all dead for time score

            EndOfLevelCollision levelEndHitbox = FindObjectOfType<EndOfLevelCollision>();
            Player player = FindObjectOfType<Player>();

            if(levelEndHitbox != null && player != null)
                player.transform.position = levelEndHitbox.transform.position;
        }
    }
}