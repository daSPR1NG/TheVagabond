using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class RessourcesManager : MonoBehaviour
    {
        [Header("OVERTIME INCOME SETTINGS")]
        public bool usesOvertimeIncome = false;
        public float overtimeIncome = 5f;
        public float overtimeDelay = 1f;

        [Header("RESSOURCES")]
        public List<Ressource> characterRessources = new();

        #region Singleton - Awake
        public static RessourcesManager Instance;

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

        void Start()
        {
            if (usesOvertimeIncome) InvokeRepeating(nameof(IncreaseRessourcesOvertime), 1, overtimeDelay);
        }

        private void Update()
        {
            //Debug
            if (Input.GetKeyDown(KeyCode.M))
            {
                GetThisRessource(RessourceType.Minerals).AddToCurrentValue(600);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetThisRessource(RessourceType.Minerals).RemoveToCurrentValue(600);
            }
        }

        public Ressource GetThisRessource(RessourceType wantedRessourceType)
        {
            Ressource ressource;

            foreach (Ressource thisRessource in characterRessources)
            {
                if (thisRessource.ressourceType == wantedRessourceType)
                {
                    ressource = thisRessource;
                    return ressource;
                }
            }

            return null;
        }

        private void IncreaseRessourcesOvertime()
        {
            for (int i = 0; i < characterRessources.Count; i++)
            {
                characterRessources[i].AddToCurrentValue(overtimeIncome);
            }
        }

        #region Editor
        private void OnValidate()
        {
            Editor_SetRessourcesDatas();
        }

        private void Editor_SetRessourcesDatas()
        {
            for (int i = 0; i < characterRessources.Count; i++)
            {
                characterRessources[i].ressourceName = characterRessources[i].ressourceType.ToString();
                characterRessources[i].CurrentValue = characterRessources[i].startingValue;
            }
        }
        #endregion
    }
}