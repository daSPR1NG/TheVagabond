using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadingManager : MonoBehaviour
{
    public delegate void FadeTransitionStatusHandler();
    public static event FadeTransitionStatusHandler OnTransition;

    [Header("UI | Combat crossfade")]
    [SerializeField] private GameObject contentObject;
    [SerializeField] private GameObject fadingObject;
    [SerializeField] private float fadeDuration = .75f;
    [SerializeField] private float fadeTransitionDuration = .75f;
    [SerializeField] private Image ImageRef => fadingObject.GetComponentInChildren<Image>();

    #region Singleton
    public static FadingManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void OnEnable()
    {
        GameManager.OnEnteringCombat += EnterCombat;
        GameManager.OnExitingCombat += ExitCombat;
    }

    private void OnDisable()
    {
        GameManager.OnEnteringCombat -= EnterCombat;
        GameManager.OnExitingCombat -= ExitCombat;
    }

    void Start() 
    {
        ToggleContentObject();
        UIManager.Instance.HideThisUIComponent(fadingObject);
    }

    private void EnterCombat()
    {
        StartCoroutine(FadeInCoroutine(ImageRef, 1, 0, fadeDuration, _SceneManager.Instance.CombatSceneName));
    }

    private void ExitCombat()
    {
        StartCoroutine(FadeInCoroutine(ImageRef, 1, 0, fadeDuration, _SceneManager.Instance.DefaultSceneName));
    }

    #region Cross Fade methods
    #region Summary
    /// <summary>
    /// Execute a fade transition between the current scene and the wanted scene we want to load.
    /// </summary>
    /// <param name="imageRef"></param>
    /// <param name="midAlphaValue"></param>
    /// <param name="endAlphaValue"></param>
    /// <param name="fadeDuration"></param>
    /// <param name="sceneToLoad"></param>
    /// <returns></returns>
    #endregion
    IEnumerator FadeInCoroutine(Image imageRef, float midAlphaValue, float endAlphaValue, float fadeDuration, string sceneToLoad)
    {
        yield return new WaitUntil(() => GameManager.Instance.GameIsPlaying());

        UIManager.Instance.DisplayThisUIComponent(fadingObject);

        yield return imageRef.DOFade(midAlphaValue, fadeDuration).WaitForCompletion();

        CombatManager.Instance.TogglePlayerCharacterActivation();
        ToggleContentObject();

        yield return new WaitForEndOfFrame();

        _SceneManager.Instance.LoadASceneByName(sceneToLoad);

        yield return new WaitUntil(() => _SceneManager.Instance.IsThisSceneLoaded(_SceneManager.Instance.CombatSceneName));

        OnTransition?.Invoke();

        yield return new WaitForSeconds(fadeTransitionDuration);
        yield return imageRef.DOFade(endAlphaValue, fadeDuration).WaitForCompletion();

        UIManager.Instance.HideThisUIComponent(fadingObject);
    }

    private void ToggleContentObject()
    {
        if (GameManager.Instance.IsInCombat() && !contentObject.activeInHierarchy)
        {
            UIManager.Instance.DisplayThisUIComponent(contentObject);
            return;
        }

        UIManager.Instance.HideThisUIComponent(contentObject);
    }
    #endregion
}