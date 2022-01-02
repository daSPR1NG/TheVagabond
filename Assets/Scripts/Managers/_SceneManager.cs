using UnityEngine;
using UnityEngine.SceneManagement;

public class _SceneManager : MonoBehaviour
{
    [SerializeField] private string defaultSceneName;
    public string DefaultSceneName { get => defaultSceneName; }

    [SerializeField] private string combatSceneName;
    public string CombatSceneName { get => combatSceneName; }

    #region Singleton
    public static _SceneManager Instance;

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

    public void LoadASceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public bool IsThisSceneLoaded(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == sceneName)
        {
            return true;
        }

        return false;
    }
}
