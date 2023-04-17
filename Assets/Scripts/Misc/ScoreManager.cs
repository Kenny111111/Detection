using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class ScoreManager : MonoBehaviour
    {
        public struct ScoreValues
        {
            public int totalScore;
            public int killScore;
            public int comboScore;
            public int timeScore;

            public ScoreValues(int total, int kill, int combo, int time)
            {
                totalScore = total;
                killScore = kill;
                comboScore = combo;
                timeScore = time;
            }
        }

        public static ScoreManager instance;

        [Header("Testing(requires PREPARINGLEVEL)")]
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

        // Cached values from the last level that was cleared
        public ScoreValues Scores { get; private set; }


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

        private void OnDestroy()
        {
            EnemyManager.OnEnemyDeath -= HandleEnemyDeath;
            GameManager.PreGameStateChanged -= HandleGameStateChange;
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
                    Reset();
                    if(testScoreSystem) StartCoroutine(TestScoreSystem());
                    break;
                case GameState.PLAYINGMISSION:
                    // Start/Restart level timer
                    levelStartTime = Time.time;
                    break;
                case GameState.LEVELENDED:
                    CalculateTimeScore(Time.time - levelStartTime);
                    CalculateFinalScore();
                    SaveScore();
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
            Scores = new ScoreValues((int)totalScore, (int)killScore, (int)comboScore, (int)timeScore);
            Debug.Log("<color=green>Final Score values:</color>");
            Debug.Log("<color=green>totalScore:</color> " + Scores.totalScore);
            Debug.Log("<color=green>killScore:</color> " +  Scores.killScore);
            Debug.Log("<color=green>comboScore:</color> " + Scores.comboScore);
            Debug.Log("<color=green>timeScore:</color> " +  Scores.timeScore);
        }

        private IEnumerator TestScoreSystem()
        {
            List<KeyValuePair<Enemy, GameObject>> enemies = EnemyManager.instance.GetActiveEnemies();

            foreach(var enemyKeyValue in enemies)
            {
                enemyKeyValue.Key.Die(IDealsDamage.Weapons.Rifle, AttackerType.Player);
                yield return new WaitForSeconds(Random.Range(minRandKillTime, maxRandKillTime));
            }

            EndOfLevelCollision levelEndHitbox = FindObjectOfType<EndOfLevelCollision>();
            Player player = FindObjectOfType<Player>();
            if(levelEndHitbox != null && player != null)
                player.transform.position = levelEndHitbox.transform.position;
        }
    }
}