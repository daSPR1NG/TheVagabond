using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    //A debug, voir si les events ne posent pas problème avec autres choses
    public delegate void ExperienceHandler(float currentExperience, float experienceToNextLevel);
    public static event ExperienceHandler OnExperienceGained;

    public delegate void LevelHandler(float currentLevel);
    public static event LevelHandler OnLevelUp;

    #region Singleton
    public static ExperienceManager s_Singleton;

    private void Awake()
    {
        if (s_Singleton != null && s_Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            s_Singleton = this;
        }
    }
    #endregion

    public int currentLevel = 0; //Debug
    public float currentExperience = 0f; //Debug
    public int skillPoints = 0;

    [Header("EXP PARAMETERS")]
    [SerializeField] private float experienceToNextLevel = 0;
    [SerializeField] private float experienceMultiplicator = 1.1f;
    [SerializeField] private float neededExperienceToLevelOne = 25;
    
    [Header("DEBUG")]
    [Range(5, 100)] public float experienceToAdd = 0f; //Debug

    private void Start()
    {
        CalculateNeededExperiencePerLevel();

        OnExperienceGained?.Invoke(currentExperience, experienceToNextLevel);
        OnLevelUp?.Invoke(currentLevel);
    }

    #region Summary
    /// <summary>
    /// Calculate the experience required per level.
    /// </summary>
    #endregion
    public void CalculateNeededExperiencePerLevel()
    {
        if (currentLevel == 0)
        {
            experienceToNextLevel = neededExperienceToLevelOne;
            return;
        }

        experienceToNextLevel = Mathf.Floor(Mathf.Pow((neededExperienceToLevelOne * currentLevel), experienceMultiplicator));
    }

    #region Summary
    /// <summary>
    /// Adds experience.
    /// </summary>
    /// <param name="experienceToAdd"></param>
    #endregion
    public void AddExperience(float experienceToAdd)
    {
        float remainingExperience;

        currentExperience += experienceToAdd;

        if (currentExperience >= experienceToNextLevel)
        {
            remainingExperience = currentExperience - experienceToNextLevel;

            LevelUp();

            currentExperience = 0;
            currentExperience += remainingExperience;
        }

        OnExperienceGained?.Invoke(currentExperience, experienceToNextLevel);
    }

    #region Summary
    /// <summary>
    /// Adds a level.
    /// </summary>
    #endregion
    public void LevelUp()
    {
        currentLevel++;
        skillPoints++;

        CalculateNeededExperiencePerLevel();

        OnLevelUp?.Invoke(currentLevel);

        OnExperienceGained?.Invoke(currentExperience, experienceToNextLevel); //Delete this line after debug
    }

    #region Getters
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetSkillPointsAmountl()
    {
        return skillPoints;
    }

    public ExperienceManager GetExperienceManager()
    {
        return this;
    }
    #endregion

    #region Reset method
    public void ResetParameters()
    {
        currentLevel = 0;
        currentExperience = 0;
        experienceToAdd = 5;
        experienceToNextLevel = 0;
        skillPoints = 0;

        CalculateNeededExperiencePerLevel();

        OnExperienceGained?.Invoke(currentExperience, experienceToNextLevel);
        OnLevelUp?.Invoke(currentLevel);
    }
    #endregion
}