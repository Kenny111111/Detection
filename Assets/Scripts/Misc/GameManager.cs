using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    DEFAULT,        // Fall-back state, should never happen.
    INITIALSTART,   // The initial start of the game.
    MAINMENU,       // Player is in the main menu.
    LEVELSTARTING,  // Level start animations / intros are playing.
    PLAYINGLEVEL,   // Player is in the level and playing.
    LEVELPAUSED,    // Player is interacting with the wrist menu.
    LEVELCLEARED,   // Player has killed all enemies in the level.
    PLAYERDIED,     // Player died while playing the level.
    LEVELENDED,     // Level end animations / outros are playing.
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;
    private int currentSceneNum = 0;
    private int totalNumberOfScenes = 5;

    public void UpdateGameState(GameState newState)
    {
        if (gameState == newState) return;

        gameState = newState;

        switch (gameState)
        {
            case GameState.INITIALSTART:
                HandleInitialStart();
                break;
            case GameState.MAINMENU:
                HandleMainMenu();
                break;
            case GameState.LEVELSTARTING:
                HandleLevelStarting();
                break; 
            case GameState.PLAYINGLEVEL:
                HandlePlayingLevel();
                break;
            case GameState.LEVELPAUSED:
                HandleLevelPaused();
                break;
            case GameState.LEVELCLEARED:
                HandleLevelCleared();
                break;
            case GameState.PLAYERDIED:
                HandlePlayerDied();
                break;
            case GameState.LEVELENDED:
                HandleLevelEnded();
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
    }

    private void Update()
    {

    }

    // The initial start of the game
    private void HandleInitialStart()
    {
        throw new NotImplementedException();
    }

    // Player is in the main menu
    private void HandleMainMenu()
    {
        throw new NotImplementedException();
    }

    // Level start animations / intros are playing
    private void HandleLevelStarting()
    {
        // Try to start any text/animations..
        // Lock the VR rig so the player cant move during an animation
        // once they are done change the state to handleplayinglevel and unlock the vr rig, etc...
        throw new NotImplementedException();
    }

    // Player is in the level and playing
    private void HandlePlayingLevel()
    {
        throw new NotImplementedException();
    }

    // Player is interacting with the wrist menu
    private void HandleLevelPaused()
    {
        throw new NotImplementedException();
    }

    // Player has killed all enemies in the level
    private void HandleLevelCleared()
    {
        throw new NotImplementedException();
    }

    // Player died while playing the level
    private void HandlePlayerDied()
    {
        throw new NotImplementedException();
    }

    // Level end animations / outros are playing
    private void HandleLevelEnded()
    {
        throw new NotImplementedException();
    }

    public bool TryNextScene()
    {
        switch (gameState)
        {
            case GameState.LEVELCLEARED:
                SwitchToScene(currentSceneNum++);
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
}
