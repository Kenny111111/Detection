using UnityEngine;
using static Detection.ScoreManager;

namespace Detection
{
    public class ScoreUIController : MonoBehaviour
    {
        private void Awake()
        {
            if (ScoreManager.instance != null) ScoreManager.ScoreCalculated += SetScoreValues;
            else Debug.LogError("ScoreManager is null");
        }

        private void OnDestroy()
        {
            ScoreManager.ScoreCalculated -= SetScoreValues;
        }

        private void SetScoreValues()
        {
            NumberIncrementor[] incrementors = GetComponentsInChildren<NumberIncrementor>();
            foreach(NumberIncrementor numberIncrementor in incrementors)
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
    }
} // Namespace Detection