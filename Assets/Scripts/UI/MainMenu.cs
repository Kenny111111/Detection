using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Detection
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject settingsMenu;

        public void StartButton()
        {
            SceneManager.LoadScene("Lab");
        }

        public void QuitButton()
        {
            Application.Quit();
            Debug.Log(message: "Quit Game");
        }

        public void SettingsButton()
        {
            if (settingsMenu.activeInHierarchy == true)
                settingsMenu.SetActive(false);
            else
                settingsMenu.SetActive(true);
        }
    }
}