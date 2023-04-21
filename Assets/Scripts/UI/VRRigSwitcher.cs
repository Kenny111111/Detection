using UnityEngine;
using UnityEngine.SceneManagement;

public class VRRigSwitcher : MonoBehaviour
{
    [SerializeField] private string originalVRRigName;
    [SerializeField] private string mainMenuVRRigName;

    private GameObject originalVRRig;
    private GameObject mainMenuVRRig;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            if (originalVRRig != null) originalVRRig.SetActive(false);
            if (mainMenuVRRig != null) mainMenuVRRig.SetActive(true);
        }
        else
        {
            if (originalVRRig != null) originalVRRig.SetActive(true);
            if (mainMenuVRRig != null) mainMenuVRRig.SetActive(false);
        }
    }
}
