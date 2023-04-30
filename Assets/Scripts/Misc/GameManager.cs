using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Collections;

namespace Detection
{
    public enum GameState
    {
        DEFAULT,            // Fall-back state, should never happen.
        PLAYINGGAMEINTRO,   // The game intro is playing.
        INMAINMENU,         // Player is in the main menu.
        LEVELINTRO,         // Level intro scene is playing.
        PREPARINGLEVEL,     // Prepare the level to start playing.
        PLAYINGMISSION,       // Player is in the level and playing.
        LEVELPAUSED,        // Player is interacting with the wrist menu.
        PLAYERDIED,         // Player died while playing the level.
        MISSIONCLEARED,       // Player has killed all enemies in the level.
        LEVELOUTRO,         // Level outro scene is playing.
        MISSIONSTATISTICS,    // Show the player their statistics.
        LEVELENDED,         // Prepare before the next level.
        PLAYINGCREDITS,     // Playing ending credits.
    }

    public class GameManager : MonoBehaviour
    {    
        public static GameManager instance;

        public GameState gameState;
        private int currentSceneNum = 0;
        private int totalNumberOfScenes;
        private GameObject playerObject;
        private GameObject cameraObject;

        public static event Action<GameState> OnGameStateChanged;
        public static event Action<GameState> AfterGameStateChanged;
        
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.Log("Game Manager is null");
                return _instance;
            }
        }

        //setup for subtitle
        // public int index;
        // public GameObject blackOverlay;
        // public TMP_Text _messageText;

        // //play subtitle
        // public void PlayDialogue(Dialogue dialogue)
        // {
        //     Debug.Log("Test for dialogue function");
        //     index = 0;
        //     StartCoroutine(PlayDialogueRoutine(dialogue));
        // }

        // //displaying subtitle
        // private IEnumerator PlayDialogueRoutine(Dialogue dialogue)
        // {
        //     _messageText.transform.localPosition = dialogue.textPos;
        //     _messageText.transform.localEulerAngles = dialogue.textRotation;
        //     _messageText.color = dialogue.textColor;
        //     while(index != dialogue.subtitles.Length)
        //     {
        //         _messageText.text = dialogue.subtitles[index].message;
        //         blackOverlay.transform.localScale = dialogue.subtitles[index].blackOverlaySize;
        //         yield return new WaitForSeconds(dialogue.subtitles[index].secondsDisplayed);
        //         index++;
        //     }
        //     blackOverlay.transform.localScale = Vector3.zero;
        //     _messageText.text = "";
        // }
        
        public Dialogue dialogue;

        private void Awake()
        {
            currentSceneNum = 0;
            totalNumberOfScenes = SceneManager.sceneCountInBuildSettings;

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

            GameManager.AfterGameStateChanged += GameManagerAfterGameStateChanged;
            EnemyManager.OnAllEnemiesDead += GameManagerOnAllEnemiesDead;
        }

        private void GameManagerOnAllEnemiesDead()
        {
            UpdateGameState(GameState.MISSIONCLEARED);
        }

        private void OnDestroy()
        {
            GameManager.AfterGameStateChanged -= GameManagerAfterGameStateChanged;
            EnemyManager.OnAllEnemiesDead -= GameManagerOnAllEnemiesDead;
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public void UpdateGameState(GameState newState)
        {
            if (gameState == newState) return;

            AfterGameStateChanged?.Invoke(gameState);

            gameState = newState;

            OnGameStateChanged?.Invoke(gameState);

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
                case GameState.PLAYINGMISSION:
                    OnPlayingMission();
                    break;
                case GameState.LEVELPAUSED:
                    OnLevelPaused();
                    break;
                case GameState.PLAYERDIED:
                    OnPlayerDied();
                    break;
                case GameState.MISSIONCLEARED:
                    OnLevelCleared();
                    break;
                case GameState.LEVELOUTRO:
                    OnLevelOutro();
                    break;
                case GameState.MISSIONSTATISTICS:
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
        }

        // pre doesnt do levelended
        // on doesnt do preparing level

        private void GameManagerAfterGameStateChanged(GameState currentState)
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
                case GameState.PLAYINGMISSION:
                    AfterPlayingMission();
                    break;
                case GameState.LEVELPAUSED:
                    AfterLevelPaused();
                    break;
                case GameState.PLAYERDIED:
                    AfterPlayerDied();
                    break;
                case GameState.MISSIONCLEARED:
                    AfterMissionCleared();
                    break;
                case GameState.LEVELOUTRO:
                    AfterLevelOutro();
                    break;
                case GameState.MISSIONSTATISTICS:
                    AfterMissionStatistics();
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

        }
        
        // Player is leaving the main menu
        private void AfterInMainMenu()
        {
            // Enable the controller ray interators
            // Disable the controller direct interators
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
            // Do other preparing stuff...

            // Once we are finished preparing the level, switch gamestate to playinglevel
            UpdateGameState(GameState.PLAYINGMISSION);
        }

        // Done preparing the level
        private void AfterPreparingLevel()
        {

        }

        // Player is now in the level and playing
        private void OnPlayingMission()
        {
            EnablePlayerInput();
            EnableScanner();
        }

        // Player is done playing in the level
        private void AfterPlayingMission()
        {

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
            ReloadScene();
        }

        // Player is no longer dead
        private void AfterPlayerDied()
        {

        }

        // Player has killed all enemies in the level.
        private void OnLevelCleared()
        {
            Debug.Log("OnLevelCleared: killed all enemies");
            UIManager.Instance.PlayDialogue(dialogue);
            // PlayDialogue(dialogue);
        }

        // The level is no longer levelCleared
        private void AfterMissionCleared()
        {
            Debug.Log("AfterMissionCleared: killed all enemies");
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

        }

        // Done with the current levels statistics.
        private void AfterMissionStatistics()
        {

        }

        // The level ended
        private void OnLevelEnded()
        {
            // Do stuff before the next level is loaded
            // Clear all points
            ParticleCollector.instance.ClearAllPoints();
            // Clear the music queue
            FindObjectOfType<MusicSystem>().ResetQueue();
            TrySwitchToNextScene();
        }

        // Do stuff before 
        private void AfterLevelEnded()
        {

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
                case GameState.MISSIONSTATISTICS:
                case GameState.MISSIONCLEARED:
                    if (currentSceneNum + 1 < totalNumberOfScenes)
                    {
                        GameManager.instance.UpdateGameState(GameState.LEVELENDED);
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

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        public void SwitchToScene(string sceneName, bool updateCurrentSceneNum)
        {
            SceneManager.LoadScene(sceneName);
            //SceneManager.UnloadSceneAsync(currentSceneNum);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            if (updateCurrentSceneNum) currentSceneNum = GetSceneIndexFromName(sceneName);
        }

        public void SwitchToScene(int sceneNum, bool updateCurrentSceneNum)
        {
            SceneManager.LoadScene(sceneNum);
            //SceneManager.UnloadSceneAsync(currentSceneNum);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
