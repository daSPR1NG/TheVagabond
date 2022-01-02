using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetencesCompartmentManager : MonoBehaviour
{
    [Header("UI | Unit Competences")]
    [SerializeField] private Transform competencesParent;
    [SerializeField] private GameObject competenceUIPrefab;
    private List<GameObject> instanceCreated = new List<GameObject>();

    #region Singleton
    public static CompetencesCompartmentManager Instance;

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
        TurnOrderManager.OnTurnBegun += SetCompetencesUICompartment;
        TurnOrderManager.OnTurnEnded += SetCompetencesUICompartment;
    }

    private void OnDisable()
    {
        TurnOrderManager.OnTurnBegun -= SetCompetencesUICompartment;
        TurnOrderManager.OnTurnEnded -= SetCompetencesUICompartment;
    }

    void CreateCompetencesUI(Unit unit)
    {
        Debug.Log(unit.competences.Count);

        for (int i = 0; i < unit.competences.Count; i++)
        {
            GameObject instance = Instantiate(competenceUIPrefab, competencesParent);
            instanceCreated.Add(instance);

            CompetenceUIHandler competenceUIHandlerRef = instance.GetComponent<CompetenceUIHandler>();
            competenceUIHandlerRef.SetIconImage(unit.competences[ i ].icon);
        }
    }

    void DeleteAllCreatedCompetences()
    {
        if (instanceCreated.Count <= 0) { return; }

        for (int i = 0; i < instanceCreated.Count; i++)
        {
            Destroy(instanceCreated [ i ]);
        }

        instanceCreated.Clear();
    }

    private void SetCompetencesUICompartment()
    {
        Unit unitRef = TurnOrderManager.Instance.GetCurrentPlayingUnit();

        DeleteAllCreatedCompetences();

        if (unitRef.team == Team.Ennemy) { return; }

        CreateCompetencesUI(unitRef);
    }
}
