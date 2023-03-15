using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Detection
{
    public class MenuConfig : MonoBehaviour
    {
        public void StartButton()
        {
            SceneManager.LoadScene("Lab");
        }
    }
}