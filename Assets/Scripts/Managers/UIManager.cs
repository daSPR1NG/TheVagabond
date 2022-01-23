using UnityEngine;

namespace Khynan_Coding
{
    public class UIManager : MonoBehaviour
    {
        [Header("PRINCIPAL UI COMPONENTS")]
        [SerializeField] private GameObject pauseMenuComponent;

        #region Singleton
        public static UIManager Instance;

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

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += TogglePauseMenu;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= TogglePauseMenu;
        }

        private void TogglePauseMenu()
        {
            if (!IsThisComponentDisplayed(pauseMenuComponent))
            {
                DisplayThisUIComponent(pauseMenuComponent);
                return;
            }

            HideThisUIComponent(pauseMenuComponent);
        }

        #region General methods
        public void DisplayThisUIComponent(GameObject component)
        {
            if (component is not null)
            {
                component.SetActive(true);
            }
        }

        public void HideThisUIComponent(GameObject component)
        {
            if (component is not null)
            {
                component.SetActive(false);
            }
        }

        private bool IsThisComponentDisplayed(GameObject component)
        {
            if (component.activeInHierarchy)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}