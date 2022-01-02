using System.Collections.Generic;
using UnityEngine;

public enum RessourceType
{
    Unassigned,
    Wood,
    Minerals,
    Food,
    Herbs,
    Leather,
}

public class RessourceComponentManager : MonoBehaviour
{
    public int ressourceTypeIndicator;

    [Space] [Header("COMPONENTS")]
    public List<RessourceComponent> ressourceComponents;

    #region Singleton
    public static RessourceComponentManager Instance;

    private void Awake()
    {
        if (Instance && Instance != this)
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

    [System.Serializable]
    public class RessourceComponent
    {
        public string ressourceName = "[TYPE HERE]";
        public RessourceType ressourceType = RessourceType.Unassigned;
        public Sprite ressourceIcon;
    }

    public Sprite GetRessourceIcon(RessourceType wantedRessourceType)
    {
        for (int i = 0; i < ressourceComponents.Count; i++)
        {
            if (ressourceComponents [ i ].ressourceType == wantedRessourceType)
            {
                return ressourceComponents [ i ].ressourceIcon;
            }
        }

        return null;
    }

    #region Editor
    private void OnValidate()
    {
        Editor_SetRessourcesDatas();
    }

    private void Editor_SetRessourcesDatas()
    {
        ressourceTypeIndicator = System.Enum.GetValues(typeof(RessourceType)).Length - 1;

        for (int i = 0; i < ressourceComponents.Count; i++)
        {
            ressourceComponents [ i ].ressourceName = ressourceComponents [ i ].ressourceType.ToString();
        }
    }
    #endregion
}