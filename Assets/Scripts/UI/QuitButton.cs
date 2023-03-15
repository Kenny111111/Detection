using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class QuitButton : MonoBehaviour
    {
        public void Quit()
        {
            Application.Quit();
            Debug.Log(message: "Quit Game");
        }
    }
}
