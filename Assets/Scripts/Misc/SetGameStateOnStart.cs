using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class SetGameStateOnStart : MonoBehaviour
    {
        public GameState setGameManagerGameStateTo;

        void Start()
        {
            GameManager.instance.UpdateGameState(setGameManagerGameStateTo);
        }
    }
}