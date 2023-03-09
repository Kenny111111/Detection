using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class SetGameStateOnAwake : MonoBehaviour
    {
        public GameState setGameManagerGameStateTo;

        void Awake()
        {
            GameManager.instance.UpdateGameState(setGameManagerGameStateTo);
        }
    }
}