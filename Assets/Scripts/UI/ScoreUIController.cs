using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    public class ScoreUIController : MonoBehaviour
    {
        public float TimeAfterScoresToEndLevel = 5f;

        // Array to control what order the score numbers are displayed
        private NumberIncrementor[] numberIncrementors;
        private int[] scores;

        private void Awake()
        {
            if (ScoreManager.instance != null) ScoreManager.ScoreCalculated += InitializeAndStart;
            else Debug.LogError("ScoreManager is null");

            numberIncrementors = new NumberIncrementor[Enum.GetValues(typeof(ScoreManager.ScoreType)).Length];
            scores = new int[numberIncrementors.Length];
        }

        private void OnDestroy()
        {
            ScoreManager.ScoreCalculated -= InitializeAndStart;
            foreach(NumberIncrementor numberIncrementor in numberIncrementors)
            {
                numberIncrementor.OnDoneIncrementing -= StartNextOrFinish;
            }
        }

        private void InitializeAndStart()
        {
            NumberIncrementor[] incrementors = GetComponentsInChildren<NumberIncrementor>();
            foreach(NumberIncrementor numberIncrementor in incrementors)
            {
                switch (numberIncrementor.scoreType)
                {
                    // Index number is 0 since it is the first one to be triggered.
                    case ScoreManager.ScoreType.KILL:
                        numberIncrementor.displayOrderNum = 0;
                        numberIncrementors[0] = numberIncrementor;
                        numberIncrementors[0].OnDoneIncrementing += StartNextOrFinish;
                        scores[0] = ScoreManager.instance.KillScore;
                        numberIncrementor.transform.parent.gameObject.SetActive(false);
                        break;
                    case ScoreManager.ScoreType.COMBO:
                        numberIncrementor.displayOrderNum = 1;
                        numberIncrementors[1] = numberIncrementor;
                        numberIncrementors[1].OnDoneIncrementing += StartNextOrFinish;
                        scores[1] = ScoreManager.instance.ComboScore;
                        numberIncrementor.transform.parent.gameObject.SetActive(false);
                        break;
                    case ScoreManager.ScoreType.TIME:
                        numberIncrementor.displayOrderNum = 2;
                        numberIncrementors[2] = numberIncrementor;
                        numberIncrementors[2].OnDoneIncrementing += StartNextOrFinish;
                        scores[2] = ScoreManager.instance.TimeScore;
                        numberIncrementor.transform.parent.gameObject.SetActive(false);
                        break;
                    case ScoreManager.ScoreType.TOTAL:
                        numberIncrementor.displayOrderNum = 3;
                        numberIncrementors[3] = numberIncrementor;
                        numberIncrementors[3].OnDoneIncrementing += StartNextOrFinish;
                        scores[3] = ScoreManager.instance.TotalScore;
                        numberIncrementor.transform.parent.gameObject.SetActive(false);
                        break;
                    default:
                        Debug.LogError("Unknown ScoreType in " + numberIncrementor.gameObject.name);
                        break;
                }
            }

            // Start the process of displaying all the values
            DisplayScore(0);
        }

        // Gets called when it is time to start displaying the next score
        private void StartNextOrFinish(int finishedIndexNum)
        {
            // End the level when the scores are done being displayed
            if (finishedIndexNum + 1 > numberIncrementors.Length - 1)
            {
                StartCoroutine(EndStatisticsLevel()); 
            }
            else
            {
                DisplayScore(finishedIndexNum + 1);
            }
        }

        private void DisplayScore(int currentIndexNum)
        {
            numberIncrementors[currentIndexNum].transform.parent.gameObject.SetActive(true);
            numberIncrementors[currentIndexNum ].Value = scores[currentIndexNum];
        }

        private IEnumerator EndStatisticsLevel()
        {
            yield return new WaitForSeconds(TimeAfterScoresToEndLevel);
            GameManager.instance.TrySwitchToNextScene();
        }
    }
} // Namespace Detection