using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum GameState
    {
        Play, Pause,
    }

    public class GameManager : MonoBehaviour
    {
        public delegate void GameStateHandler();
        public static event GameStateHandler OnGameStateChanged;

        public GameState GameState = GameState.Play;
        public Transform ActivePlayer;

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

                if (!ActivePlayer)
                {
                    ActivePlayer = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
        }
        #endregion

        void Start()
        {
            SetGameStateToPlayingMod();
        }

        void Update()
        {
            if (Helper.IsKeyPressed(InputsManager.Instance.GetInputByName("Pause")))
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
            GameState = GameState.Pause;
            SetTimeScaleTo(0);
        }

        public void SetGameStateToPlayingMod()
        {
            GameState = GameState.Play;
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
            if (GameState == GameState.Pause)
            {
                return true;
            }

            return false;
        }

        public bool GameIsPlaying()
        {
            if (GameState == GameState.Play)
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