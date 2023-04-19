using System.Collections;
using UnityEngine;
using static Detection.ScoreManager;

namespace Detection
{
    public class ScoreUIController : MonoBehaviour
    {

        //private void Awake()
        //{
        //    if (ScoreManager.instance != null) ScoreManager.ScoreCalculated += SetScoreValues;
        //    else Debug.LogError("ScoreManager is null");
        //}

        private void Start()
        {
            //if (ScoreManager.instance != null) StartCoroutine(SetScores());
            if (ScoreManager.instance != null) ScoreManager.ScoreCalculated += SetScoreValues;
            else Debug.LogError("ScoreManager is null");
        }

        private void SetScoreValues()
        {
            foreach (var numberIncrementor in GetComponentsInChildren<NumberIncrementor>())
            {
                switch (numberIncrementor.scoreType)
                {
                    case ScoreManager.ScoreType.TOTAL:
                        numberIncrementor.Value = ScoreManager.instance.TotalScore;
                        break;
                    case ScoreManager.ScoreType.KILL:
                        numberIncrementor.Value = ScoreManager.instance.KillScore;
                        break;
                    case ScoreManager.ScoreType.COMBO:
                        numberIncrementor.Value = ScoreManager.instance.ComboScore;
                        break;
                    case ScoreManager.ScoreType.TIME:
                        numberIncrementor.Value = ScoreManager.instance.TimeScore;
                        break;
                    default:
                        Debug.LogError("Unknown ScoreType in " + numberIncrementor.gameObject.name);
                        break;
                }
            }
        }

        //private IEnumerator SetScores()
        //{
        //    // This is here because function execution order gets messed up when switching scenes...
        //    // ScoreManager should be calculating scores before scene change, but doesn't...
        //    while(!ScoreManager.instance.ScoresUpdated)
        //    {
        //        yield return null;
        //    }

        //    foreach (var numberIncrementor in GetComponentsInChildren<NumberIncrementor>())
        //    {
        //        switch (numberIncrementor.scoreType)
        //        {
        //            case ScoreManager.ScoreType.TOTAL:
        //                numberIncrementor.Value = ScoreManager.instance.TotalScore;
        //                break;
        //            case ScoreManager.ScoreType.KILL:
        //                numberIncrementor.Value = ScoreManager.instance.KillScore;
        //                break;
        //            case ScoreManager.ScoreType.COMBO:
        //                numberIncrementor.Value = ScoreManager.instance.ComboScore;
        //                break;
        //            case ScoreManager.ScoreType.TIME:
        //                numberIncrementor.Value = ScoreManager.instance.TimeScore;
        //                break;
        //            default:
        //                Debug.LogError("Unknown ScoreType in " + numberIncrementor.gameObject.name);
        //                break;
        //        }
        //    }
        //}
    }
} // Namespace Detection