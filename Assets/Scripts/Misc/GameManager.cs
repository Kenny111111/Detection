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
        PLAYINGGAMEINTRO,   // The game intro is playing.
        INMAINMENU,         // Player is in the main menu.
        LEVELINTRO,         // Level intro scene is playing.
        PREPARINGLEVEL,     // Prepare the level to start playing.
        PLAYINGLEVEL,       // Player is in the level and playing.
        LEVELPAUSED,        // Player is interacting with the wrist menu.
        PLAYERDIED,         // Player died while playing the level.
        LEVELCLEARED,       // Player has killed all enemies in the level.
        LEVELOUTRO,         // Level outro scene is playing.
        LEVELSTATISTICS,    // Show the player their statistics.
        LEVELENDED,         // Prepare before the next level.
        PLAYINGCREDITS,     // Playing ending credits.
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private GameState gameState;
        private int currentSceneNum = 0;
        private const int totalNumberOfScenes = 5;
        private GameObject playerObject;
        private GameObject cameraObject;

        public static event Action<GameState> OnGameStateChanged;

        //private void Start() => UpdateGameState(GameState.INITIALSTART);

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
            if (playerObject == null) Debug.LogError("Unable to find an object with tag 'Player'. playerObject is null.");

            cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject == null) Debug.LogError("Unable to find an object with tag 'MainCamera'. cameraObject is null.");
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public void UpdateGameState(GameState newState)
        {
            if (gameState == newState) return;

            gameState = newState;

            switch (gameState)
            {
                case GameState.INITIALSTART:
                    HandleInitialStart();
                    break;
                case GameState.PLAYINGGAMEINTRO:
                    HandlePlayingGameIntro();
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
                case GameState.PLAYINGCREDITS:
                    HandlePlayingCredits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }

            OnGameStateChanged?.Invoke(gameState);
        }

        // The initial start of the game
        private void HandleInitialStart()
        {
            // Prepare any pre game start stuff...

            // Then continue the game by going to the next scene
            TrySwitchToNextScene();
        }

        // The game intro is playing
        private void HandlePlayingGameIntro()
        {
            DisablePlayerInput();
            DisableScanner();
        }

        // Player is in the main menu
        private void HandleInMainMenu()
        {
            // run any code we want to prepare playing in the main menu
            throw new NotImplementedException();
        }

        // Level intro scene is playing.
        private void HandleLevelIntro()
        {
            DisablePlayerInput();
            DisableScanner();
        }

        // Prepare the level to start playing.
        private void HandlePreparingLevel()
        {
            // Try to spawn the player in the level
            Transform spawnPointTransform = GameObject.FindWithTag("SpawnPoint").transform;
            if (spawnPointTransform == null) Debug.LogError("Unable to find an object with tag 'SpawnPoint'. spawnPointTransform is null.");
            SpawnPlayerAtTransform(spawnPointTransform);

            // Do other preparing stuff...

            // Once we are finished preparing the level, switch gamestate to playinglevel
            UpdateGameState(GameState.PLAYINGLEVEL);
        }

        // Player is now in the level and playing
        private void HandlePlayingLevel()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Player is interacting with the wrist menu.
        private void HandleLevelPaused()
        {
            throw new NotImplementedException();
        }

        // Player died while playing the level.
        private void HandlePlayerDied()
        {
            throw new NotImplementedException();
        }

        // Player has killed all enemies in the level.
        private void HandleLevelCleared()
        {
            // trigger showing arrows

            throw new NotImplementedException();
        }

        // Level outro scene is playing.
        private void HandleLevelOutro()
        {
            DisablePlayerInput();
            DisableScanner();
        }

        // Show the player their statistics.
        private void HandleLevelStatistics()
        {
            throw new NotImplementedException();
        }

        // Prepare before the next level
        private void HandleLevelEnded()
        {
            // Do stuff before the next level is loaded

            TrySwitchToNextScene();
        }

        // Play ending credits, then go to main menu
        private void HandlePlayingCredits()
        {
            throw new NotImplementedException();
        }
        
        
        ////////////////////////////////////////////////////////////////////
        
        
        public bool TrySwitchToNextScene()
        {
            switch (gameState)
            {
                case GameState.PLAYINGGAMEINTRO:
                case GameState.INITIALSTART:
                case GameState.LEVELCLEARED:
                    if (currentSceneNum + 1 < totalNumberOfScenes)
                    {
                        SwitchToScene(currentSceneNum + 1, true);
                    }
                    return true;
                case GameState.LEVELSTATISTICS:
                    if (currentSceneNum + 1 < totalNumberOfScenes)
                    {
                        SwitchToScene(currentSceneNum + 1, false);
                    }
                    return true;
                case GameState.PLAYINGCREDITS:
                    SwitchToScene(0, true);
                    return true;
                default:
                    return false;
            }
        }

        public void SwitchToScene(string sceneName, bool updateCurrentSceneNum)
        {
            SceneManager.LoadSceneAsync(sceneName);
            SceneManager.UnloadSceneAsync(currentSceneNum);
            if (updateCurrentSceneNum) currentSceneNum = GetSceneIndexFromName(sceneName);
        }

        public void SwitchToScene(int sceneNum, bool updateCurrentSceneNum)
        {
            SceneManager.LoadSceneAsync(sceneNum);
            SceneManager.UnloadSceneAsync(currentSceneNum);
            if (updateCurrentSceneNum) currentSceneNum = sceneNum;
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
