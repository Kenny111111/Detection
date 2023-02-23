using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        DEFAULT,        // Fall-back state, should never happen
        MAINMENU,       // Player is in the main menu
        LEVELSTARTING,  // Level start animations / intros are playing
        PLAYINGLEVEL,   // Player is in the level and playing
        LEVELPAUSED,    // Player is interacting with the wrist menu
        LEVELCLEARED,   // Player has killed all enemies in the level
        PLAYERDIED,     // Player died while playing the level
        LEVELENDED,     // Level end animations / outros are playing
    }
    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;
    private int currentSceneNum = 0;
    private int totalNumberOfScenes = 5;

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameState.DEFAULT:
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

    private void Start()
    {
        UpdateGameState(GameState.MAINMENU);
    }

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

    // Player is in the main menu
    private void HandleMainMenu()
    {
        throw new NotImplementedException();
    }

    // Level start animations / intros are playing
    private void HandleLevelStarting()
    {
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
                ForceNextScene();
                return true;
            default:
                return false;
        }
    }

    private void ForceNextScene()
    {
        currentSceneNum++;

        if (currentSceneNum > totalNumberOfScenes)
        {
            currentSceneNum = 0;
        }

        SceneManager.LoadSceneAsync(currentSceneNum);
    }
}
