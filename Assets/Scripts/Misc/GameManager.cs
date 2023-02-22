using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        DEFAULT,        // Fall-back state, should never happen
        MAINMENU,       // Player is in the main menu
        LEVELSTART,     // Level start animations / intros are playing
        PLAYINGLEVEL,   // Player is in the level and playing...
        LEVELPAUSED,    // Player is interacting with their wrist menu
        LEVELEND        // Level end animations / outros are playing
    }
    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;
    private int currentSceneNum;

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
            case GameState.LEVELSTART:
                HandleLevelStart();
                break; 
            case GameState.PLAYINGLEVEL:
                HandlePlayingLevel();
                break;
            case GameState.LEVELPAUSED:
                HandleLevelPaused();
                break;
            case GameState.LEVELEND:
                HandleLevelEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void Start()
    {
        UpdateGameState(GameState.MAINMENU);
        currentSceneNum = 0;
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

    private void HandleMainMenu()
    {
        throw new NotImplementedException();
    }

    private void HandleLevelStart()
    {
        throw new NotImplementedException();
    }

    private void HandlePlayingLevel()
    {
        throw new NotImplementedException();
    }

    private void HandleLevelPaused()
    {
        throw new NotImplementedException();
    }

    private void HandleLevelEnd()
    {
        throw new NotImplementedException();
    }
}
