using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Detection
{
    public class RestartButton : MonoBehaviour
    {
        public void RestartLevel()
        {
            GameManager.instance.ReloadScene();
        }
    }
}
