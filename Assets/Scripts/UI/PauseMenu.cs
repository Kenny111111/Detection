using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Detection
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject rightRayController;
        public GameObject leftController;
        public GameObject rightController;
        public static bool pauseActive;

        private XRDirectInteractor leftDirectInteractor;

        void Start()
        {
            pauseMenu.SetActive(false);
            rightRayController.SetActive(false);
            leftDirectInteractor = leftController.GetComponent<XRDirectInteractor>();
        }
        public void pauseButtonPressed(InputAction.CallbackContext menuButton)
        {
            if (menuButton.performed)
                showPauseMenu();
        }

        public void showPauseMenu()
        {
            if (pauseActive)
            {
                // Enable Ray Interactor
                pauseMenu.SetActive(false);
                rightRayController.SetActive(false);
                pauseActive = false;
                Time.timeScale = 1;

                // Enable the XR Direct Interactor in the left hand
                leftDirectInteractor.enabled = true;

                // Set the rightController as active
                rightController.SetActive(true);
            }
            else if (!pauseActive)
            {
                pauseMenu.SetActive(true);
                rightRayController.SetActive(true);
                pauseActive = true;
                Time.timeScale = 0;

                // Disable the XR Direct Interactor in the left hand
                leftDirectInteractor.enabled = false;

                // Set the rightController as inactive
                rightController.SetActive(false);
            }
        }

        public void resumeButton()
        {
            // Call showPauseMenu to resume the game and handle hand logic
            showPauseMenu();
        }
    }
}
