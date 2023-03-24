using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace Detection
{
    public enum GameState
    {
        DEFAULT,            // Fall-back state, should never happen.
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

        public GameState gameState;
        private int currentSceneNum = 0;
        private const int totalNumberOfScenes = 5;
        private GameObject playerObject;
        private GameObject cameraObject;

        public static event Action<GameState> OnGameStateChanged;
        public static event Action<GameState> PreGameStateChanged;

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

            GameManager.PreGameStateChanged += GameManagerPreGameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.PreGameStateChanged -= GameManagerPreGameStateChanged;
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public void UpdateGameState(GameState newState)
        {
            if (gameState == newState) return;

            PreGameStateChanged?.Invoke(gameState);

            gameState = newState;

            switch (gameState)
            {
                case GameState.PLAYINGGAMEINTRO:
                    OnPlayingGameIntro();
                    break;
                case GameState.INMAINMENU:
                    OnInMainMenu();
                    break;
                case GameState.LEVELINTRO:
                    OnLevelIntro();
                    break;
                case GameState.PREPARINGLEVEL:
                    OnPreparingLevel();
                    break;
                case GameState.PLAYINGLEVEL:
                    OnPlayingLevel();
                    break;
                case GameState.LEVELPAUSED:
                    OnLevelPaused();
                    break;
                case GameState.PLAYERDIED:
                    OnPlayerDied();
                    break;
                case GameState.LEVELCLEARED:
                    OnLevelCleared();
                    break;
                case GameState.LEVELOUTRO:
                    OnLevelOutro();
                    break;
                case GameState.LEVELSTATISTICS:
                    OnLevelStatistics();
                    break;
                case GameState.LEVELENDED:
                    OnLevelEnded();
                    break;
                case GameState.PLAYINGCREDITS:
                    OnPlayingCredits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
            }

            OnGameStateChanged?.Invoke(gameState);
        }

        private void GameManagerPreGameStateChanged(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.DEFAULT:
                    break;
                case GameState.PLAYINGGAMEINTRO:
                    AfterPlayingGameIntro();
                    break;
                case GameState.INMAINMENU:
                    AfterInMainMenu();
                    break;
                case GameState.LEVELINTRO:
                    AfterLevelIntro();
                    break;
                case GameState.PREPARINGLEVEL:
                    AfterPreparingLevel();
                    break;
                case GameState.PLAYINGLEVEL:
                    AfterPlayingLevel();
                    break;
                case GameState.LEVELPAUSED:
                    AfterLevelPaused();
                    break;
                case GameState.PLAYERDIED:
                    AfterPlayerDied();
                    break;
                case GameState.LEVELCLEARED:
                    AfterLevelCleared();
                    break;
                case GameState.LEVELOUTRO:
                    AfterLevelOutro();
                    break;
                case GameState.LEVELSTATISTICS:
                    AfterLevelStatistics();
                    break;
                case GameState.LEVELENDED:
                    AfterLevelEnded();
                    break;
                case GameState.PLAYINGCREDITS:
                    AfterPlayingCredits();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentState), currentState, null);
            }
        }

        // The game intro is playing
        private void OnPlayingGameIntro()
        {
            DisablePlayerInput();
            DisableScanner();
        }

        // The game intro is finished playing
        private void AfterPlayingGameIntro()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Player is in the main menu
        private void OnInMainMenu()
        {
            // Try to spawn the player in the level
            Transform spawnPointTransform = GameObject.FindWithTag("SpawnPoint").transform;
            if (spawnPointTransform == null) Debug.LogError("Unable to find an object with tag 'SpawnPoint'. spawnPointTransform is null.");
            SpawnPlayerAtTransform(spawnPointTransform);
        }
        
        // Player is leaving the main menu
        private void AfterInMainMenu()
        {
            // Enable the controller ray interators
            // Disable the controller direct interators
            throw new NotImplementedException();
        }

        // Level intro scene is playing.
        private void OnLevelIntro()
        {
            DisablePlayerInput();
            DisableScanner();
        }

        // Level intro scene is done playing.
        private void AfterLevelIntro()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Prepare the level to start playing.
        private void OnPreparingLevel()
        {
            // Try to spawn the player in the level
            Transform spawnPointTransform = GameObject.FindWithTag("SpawnPoint").transform;
            if (spawnPointTransform == null) Debug.LogError("Unable to find an object with tag 'SpawnPoint'. spawnPointTransform is null.");
            SpawnPlayerAtTransform(spawnPointTransform);

            // Do other preparing stuff...

            // Once we are finished preparing the level, switch gamestate to playinglevel
            UpdateGameState(GameState.PLAYINGLEVEL);
        }

        // Done preparing the level
        private void AfterPreparingLevel()
        {

        }

        // Player is now in the level and playing
        private void OnPlayingLevel()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Player is done playing in the level
        private void AfterPlayingLevel()
        {
            throw new NotImplementedException();
        }

        // Player is interacting with the wrist menu.
        private void OnLevelPaused()
        {
            // Enable the controller ray interators
            // Disable the controller direct interators
            throw new NotImplementedException();
        }

        // Player is done interacting with the wrist menu
        private void AfterLevelPaused()
        {
            // Disable the controller ray interators
            // Enable the controller direct interators
            throw new NotImplementedException();
        }

        // Player died while playing the level.
        private void OnPlayerDied()
        {
            throw new NotImplementedException();
        }

        // Player is no longer dead
        private void AfterPlayerDied()
        {
            throw new NotImplementedException();
        }

        // Player has killed all enemies in the level.
        private void OnLevelCleared()
        {
            // trigger showing arrows

            throw new NotImplementedException();
        }

        // The level is no longer levelCleared
        private void AfterLevelCleared()
        {
            // trigger showing arrows

            throw new NotImplementedException();
        }

        // Level outro scene is playing.
        private void OnLevelOutro()
        {
            DisablePlayerInput();
            DisableScanner();
        }

        // Level outro scene is done playing.
        private void AfterLevelOutro()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Show the player their statistics.
        private void OnLevelStatistics()
        {
            throw new NotImplementedException();
        }

        // Done with the current levels statistics.
        private void AfterLevelStatistics()
        {
            throw new NotImplementedException();
        }

        // The level ended
        private void OnLevelEnded()
        {
            // Do stuff before the next level is loaded

            TrySwitchToNextScene();
        }

        // Do stuff before 
        private void AfterLevelEnded()
        {
            throw new NotImplementedException();
        }

        // Play ending credits, then go to main menu
        private void OnPlayingCredits()
        {
            throw new NotImplementedException();
        }

        // Done playing ending credits
        private void AfterPlayingCredits()
        {
            throw new NotImplementedException();
        }


        ////////////////////////////////////////////////////////////////////


        public bool TrySwitchToNextScene()
        {
            switch (gameState)
            {
                case GameState.PLAYINGGAMEINTRO:
                case GameState.LEVELSTATISTICS:
                case GameState.LEVELCLEARED:
                    if (currentSceneNum + 1 < totalNumberOfScenes)
                    {
                        SwitchToScene(currentSceneNum + 1, true);
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
            SceneManager.LoadScene(sceneName);
            //SceneManager.UnloadSceneAsync(currentSceneNum);
            if (updateCurrentSceneNum) currentSceneNum = GetSceneIndexFromName(sceneName);
        }

        public void SwitchToScene(int sceneNum, bool updateCurrentSceneNum)
        {
            SceneManager.LoadScene(sceneNum);
            //SceneManager.UnloadSceneAsync(currentSceneNum);
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
            //playerObject.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
            //playerObject.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
        }

        private void EnablePlayerInput()
        {
            //playerObject.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
            //playerObject.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
        }

        private void DisableScanner()
        {
            //cameraObject.GetComponent<InputController>().enabled = false;
            //cameraObject.GetComponent<LookScanner>().enabled = false;
        }

        private void EnableScanner()
        {
            //cameraObject.GetComponent<InputController>().enabled = true;
            //cameraObject.GetComponent<LookScanner>().enabled = true;
        }
    }
}
