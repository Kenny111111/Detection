using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class ChangeSceneOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameManager.instance.UpdateGameState(GameState.LEVELENDED);
            GameManager.instance.SwitchToScene("Statistics", false);
        }
    }
}

