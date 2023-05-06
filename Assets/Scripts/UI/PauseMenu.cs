using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Detection
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject wristMenuSettings;
        public GameObject rightRayController;
        public GameObject leftController;
        public GameObject rightController;
        public bool pauseActive;

        public AudioClip pauseClip; 

        private AudioSource audioSource; 

        private XRDirectInteractor leftDirectInteractor;

        void Start()
        {
            pauseMenu.SetActive(false);
            rightRayController.SetActive(false);
            leftDirectInteractor = leftController.GetComponent<XRDirectInteractor>();
            audioSource = GetComponent<AudioSource>(); 
        }
        public void pauseButtonPressed(InputAction.CallbackContext menuButton)
        {
            if (menuButton.performed)
            {
                // Play pause audio clip
                if (pauseClip != null)
                {
                    audioSource.PlayOneShot(pauseClip);
                }

                showPauseMenu();
            }
        }

        public void showPauseMenu()
        {
            if (pauseActive)
            {
                pauseMenu.SetActive(false);
                wristMenuSettings.SetActive(false);
                // Enable Ray Interactor
                rightRayController.SetActive(false);
                // Set the rightController as active
                rightController.SetActive(true);

                // Enable the XR Direct Interactor in the left hand
                leftDirectInteractor.enabled = true;

                pauseActive = false;
                Time.timeScale = 1;

            }
            else if (!pauseActive)
            {
                pauseMenu.SetActive(true);
                rightRayController.SetActive(true);
                // Set the rightController as inactive
                rightController.SetActive(false);

                // Disable the XR Direct Interactor in the left hand
                leftDirectInteractor.enabled = false;

                
                pauseActive = true;
                Time.timeScale = 0;
            }
        }

        public void resumeButton()
        {
            // Call showPauseMenu to resume the game and handle hand logic
            showPauseMenu();
        }
    }
}

