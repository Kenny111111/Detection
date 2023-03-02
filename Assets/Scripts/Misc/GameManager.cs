using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace Detection
{
    public enum GameState
    {
        DEFAULT,            // Fall-back state, should never happen.
        INITIALSTART,       // The initial start of the game.
        INMAINMENU,         // Player is in the main menu.
        LEVELINTRO,         // Level intro / start animations are playing
        PREPARINGLEVEL,     // Prepare the level to start playing
        PLAYINGLEVEL,       // Player is in the level and playing.
        LEVELPAUSED,        // Player is interacting with the wrist menu.
        PLAYERDIED,         // Player died while playing the level.
        LEVELCLEARED,       // Player has killed all enemies in the level.
        LEVELOUTRO,         // Level outro / end animations are playing
        LEVELSTATISTICS,    // Show statistics and prepare the next level
        LEVELENDED,         // Prepare the next level
        ENDINGCREDITS,      // Play ending credits, then go to main menu
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameState gameState;
        private int currentSceneNum = 0;
        private int totalNumberOfScenes = 5;
        private GameObject playerObject;
        private GameObject cameraObject;

        public static event Action<GameState> OnGameStateChanged;

        public void UpdateGameState(GameState newState)
        {
            if (gameState == newState) return;

            gameState = newState;

            switch (gameState)
            {
                case GameState.INITIALSTART:
                    HandleInitialStart();
                    break;
                case GameState.INMAINMENU:
                    HandleInMainMenu();
                    break;
                case GameState.LEVELINTRO:
                    HandleLevelIntro();
                    break;
                case GameState.PREPARINGLEVEL:
                    HandlePreparingLevel();
                    break;
                case GameState.PLAYINGLEVEL:
                    HandlePlayingLevel();
                    break;
                case GameState.LEVELPAUSED:
                    HandleLevelPaused();
                    break;
                case GameState.PLAYERDIED:
                    HandlePlayerDied();
                    break;
                case GameState.LEVELCLEARED:
                    HandleLevelCleared();
                    break;
                case GameState.LEVELOUTRO:
                    HandleLevelOutro();
                    break;
                case GameState.LEVELSTATISTICS:
                    HandleLevelStatistics();
                    break;
                case GameState.LEVELENDED:
                    HandleLevelEnded();
                    break;
                case GameState.ENDINGCREDITS:
                    HandleEndingCredits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }

            OnGameStateChanged?.Invoke(gameState);
        }

        private void Start() => UpdateGameState(GameState.INITIALSTART);

        private void Awake()
        {
            // Ensure only one instance exists
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(gameObject);

            playerObject = GameObject.FindWithTag("Player");
            cameraObject = GameObject.FindWithTag("MainCamera");
        }

        // The initial start of the game
        private void HandleInitialStart()
        {
            // TEMPORARY REMOVE ME... HandleInitialStart should prepare stuff then go to the mainmenu
            UpdateGameState(GameState.PREPARINGLEVEL);
        }

        // Player is in the main menu
        private void HandleInMainMenu()
        {
            throw new NotImplementedException();
        }

        // Level intro / start animations are playing
        private void HandleLevelIntro()
        {
            DisablePlayerInput();
            DisableScanner();

            // Switch to scene that plays intro?
        }

        // Level start animations / intros are playing, spawn player
        private void HandlePreparingLevel()
        {
            SpawnPlayerAtTransform(GameObject.FindWithTag("SpawnPoint").transform);

            UpdateGameState(GameState.PLAYINGLEVEL);
        }

        // Player is now in the level and playing
        private void HandlePlayingLevel()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Player is interacting with the wrist menu
        private void HandleLevelPaused()
        {
            throw new NotImplementedException();
        }

        // Player died while playing the level.
        private void HandlePlayerDied()
        {
            throw new NotImplementedException();
        }

        // Player has killed all enemies in the level
        private void HandleLevelCleared()
        {
            // trigger showing arrows

            throw new NotImplementedException();
        }

        // Level outro / end animations are playing
        private void HandleLevelOutro()
        {
            throw new NotImplementedException();
        }

        // Show statistics and prepare the next level
        private void HandleLevelStatistics()
        {
            throw new NotImplementedException();
        }

        // Prepare the next level
        private void HandleLevelEnded()
        {
            // play animations?

            TryNextScene();
        }

        // Play ending credits, then go to main menu
        private void HandleEndingCredits()
        {
            throw new NotImplementedException();
        }







        ////////////////////////////////////////////////////////////////////







        public bool TryNextScene()
        {
            switch (gameState)
            {
                case GameState.LEVELCLEARED:
                    if (currentSceneNum++ < totalNumberOfScenes)
                    {
                        SwitchToScene(currentSceneNum++);
                    }
                    return true;
                default:
                    return false;
            }
        }

        public void SwitchToScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
            SceneManager.UnloadSceneAsync(currentSceneNum);
            currentSceneNum = GetSceneIndexFromName(sceneName);
        }

        public void SwitchToScene(int sceneNum)
        {
            SceneManager.LoadSceneAsync(sceneNum);
            SceneManager.UnloadSceneAsync(currentSceneNum);
            currentSceneNum = sceneNum;
        }

        private string GetSceneNameFromIndex(int BuildIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
            int slash = path.LastIndexOf('/');
            string name = path.Substring(slash + 1);
            int dot = name.LastIndexOf('.');
            return name.Substring(0, dot);
        }

        private int GetSceneIndexFromName(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string testedScreen = GetSceneNameFromIndex(i);
                if (testedScreen == sceneName)
                    return i;
            }
            return -1;
        }

        private void SpawnPlayerAtTransform(Transform spawnPoint)
        {
            playerObject.transform.position = spawnPoint.position;
            playerObject.transform.rotation = spawnPoint.rotation;
        }

        private void DisablePlayerInput()
        {
            playerObject.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
            playerObject.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
        }

        private void EnablePlayerInput()
        {
            playerObject.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
            playerObject.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
        }

        private void DisableScanner()
        {
            cameraObject.GetComponent<InputController>().enabled = false;
            cameraObject.GetComponent<LookScanner>().enabled = false;
        }

        private void EnableScanner()
        {
            cameraObject.GetComponent<InputController>().enabled = true;
            cameraObject.GetComponent<LookScanner>().enabled = true;
        }
    }
}
