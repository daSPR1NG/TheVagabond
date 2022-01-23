using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class RessourcesHandler : MonoBehaviour
    {
        [Header("OVERTIME INCOME SETTINGS")]
        public bool usesOvertimeIncome = false;
        public float overtimeIncome = 5f;
        public float overtimeDelay = 1f;

        [Header("RESSOURCES")]
        public List<Ressource> characterRessources = new();

        [Header("UI COMPONENTS")]
        public Transform ressourcesParentLayout;
        [SerializeField] private int instanceValue = 0;
        [SerializeField] private GameObject ressourceUICompartment;
        private List<RessourceUI> ressourceUIs = new();
        public Transform buildingButtonsHolder;

        #region Singleton - Awake
        public static RessourcesHandler Instance;

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
            //CreateAndInitializeBuildingButtons();
            SubscribeRessourceEvent();
        }

        private void OnDisable()
        {
            UnsubscribeRessourceEvent();
        }

        void Start()
        {
            InstanciateRessourcesUICompartment();

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

        private void SetRessourceValue(RessourceType ressourceType)
        {
            for (int i = 0; i < ressourceUIs.Count; i++)
            {
                if (ressourceUIs[i].ressourceType == ressourceType)
                {
                    ressourceUIs[i].UpdateRessourceValue(GetThisRessource(ressourceType).CurrentValue);
                    return;
                }
            }
        }

        private void IncreaseRessourcesOvertime()
        {
            for (int i = 0; i < characterRessources.Count; i++)
            {
                characterRessources[i].AddToCurrentValue(overtimeIncome);
            }
        }

        #region UI
        private void InstanciateRessourcesUICompartment()
        {
            for (int i = 0; i < instanceValue; i++)
            {
                GameObject prefab = Instantiate(ressourceUICompartment, ressourcesParentLayout);

                RessourceUI ressourceUI = prefab.GetComponent<RessourceUI>();
                ressourceUIs.Add(ressourceUI);
                ressourceUI.SetThisUI(
                    characterRessources[i].ressourceType,
                    characterRessources[i].CurrentValue,
                    RessourceComponentManager.Instance.GetRessourceIcon(characterRessources[i].ressourceType));
            }
        }

        public void TriggerUIFeedbackOnRessourceCollectionOrLoss(RessourceType wantedRessourceType)
        {
            for (int i = 0; i < ressourceUIs.Count; i++)
            {
                if (ressourceUIs[i].ressourceType == wantedRessourceType)
                {
                    ressourceUIs[i].PlayFeedbackAnimation();
                }
            }
        }
        #endregion

        #region Event
        private void SubscribeRessourceEvent()
        {
            for (int i = 0; i < characterRessources.Count; i++)
            {
                characterRessources[i].OnRessourceValueChanged += SetRessourceValue;
            }
        }

        private void UnsubscribeRessourceEvent()
        {
            for (int i = 0; i < characterRessources.Count; i++)
            {
                characterRessources[i].OnRessourceValueChanged -= SetRessourceValue;
            }
        }
        #endregion

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

            instanceValue = System.Enum.GetValues(typeof(RessourceType)).Length - 1;
        }
        #endregion
    }
}