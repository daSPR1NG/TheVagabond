using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Play,
    Pause,
}
public enum PlayerCombatState
{
    OutOfCombat,
    InCombat,
}

public class GameManager : MonoBehaviour
{
    public delegate void GameStateHandler();
    public static event GameStateHandler OnGameStateChanged;

    public GameState gameState = GameState.Play;

    public delegate void CombatStatusHandler();
    public static event CombatStatusHandler OnEnteringCombat;
    public static event CombatStatusHandler OnExitingCombat;

    public PlayerCombatState playerGameState = PlayerCombatState.OutOfCombat;

    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    void Start()
    {
        SetGameStateToPlayingMod();
    }

    void Update()
    {
        if (UtilityClass.IsKeyPressed(KeyCode.Escape))
        {
            ToggleGameState();
        }

        if (UtilityClass.IsKeyPressed(KeyCode.C))
        {
            EnterCombat();
        }

        if (UtilityClass.IsKeyPressed(KeyCode.V))
        {
            ExitCombat();
        }
    }

    #region Game states methods
    private void ToggleGameState()
    {
        if (GameIsPlaying())
        {
            SetGameStateToPause();
        }
        else
        {
            SetGameStateToPlayingMod();
        }

        OnGameStateChanged?.Invoke();
    }

    public void SetGameStateToPause()
    {
        gameState = GameState.Pause;
        SetTimeScaleTo(0);
    }

    public void SetGameStateToPlayingMod()
    {
        gameState = GameState.Play;
        SetTimeScaleTo(1);
    }

    public bool PlayerCanUseActions()
    {
        if (GameIsPaused() || IsInCombat() 
            || !PlayerDataManager.Instance.PlayerCharacterIsActive())
        {
            return false;
        }

        return true;
    }

    public void QuitTheGame()
    {
        UtilityClass.DebugMessage("QUIT THE GAME !");
        Application.Quit();
    }

    public bool GameIsPaused()
    {
        if (gameState == GameState.Pause)
        {
            return true;
        }

        return false;
    }

    public bool GameIsPlaying()
    {
        if (gameState == GameState.Play)
        {
            return true;
        }

        return false;
    }
    #endregion

    #region Combat game states methods
    public void EnterCombat()
    {
        SetCombatState();
        OnEnteringCombat?.Invoke();
    }

    public void ExitCombat()
    {
        SetOutOfCombatState();
        OnExitingCombat?.Invoke();
    }

    private void SetOutOfCombatState()
    {
        playerGameState = PlayerCombatState.OutOfCombat;
    }

    private void SetCombatState()
    {
        playerGameState = PlayerCombatState.InCombat;
    }

    public bool IsInCombat()
    {
        if (playerGameState == PlayerCombatState.InCombat)
        {
            return true;
        }

        return false;
    }
    #endregion

    public void SetTimeScaleTo(float newValue)
    {
        Time.timeScale = newValue;
    }
}