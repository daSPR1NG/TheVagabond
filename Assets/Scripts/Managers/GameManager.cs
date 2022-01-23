using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum GameState
    {
        Play,
        Pause,
    }

    public class GameManager : MonoBehaviour
    {
        public delegate void GameStateHandler();
        public static event GameStateHandler OnGameStateChanged;

        public GameState gameState = GameState.Play;

        public delegate void CombatStatusHandler();
        public static event CombatStatusHandler OnEnteringCombat;
        public static event CombatStatusHandler OnExitingCombat;

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
            if (Helper.IsKeyPressed(KeyCode.Escape))
            {
                ToggleGameState();
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
            if (GameIsPaused() || !PlayerDataManager.Instance.PlayerCharacterIsActive())
            {
                return false;
            }

            return true;
        }

        public void QuitTheGame()
        {
            Helper.DebugMessage("QUIT THE GAME !");
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

        public void SetTimeScaleTo(float newValue)
        {
            Time.timeScale = newValue;
        }
    }
}