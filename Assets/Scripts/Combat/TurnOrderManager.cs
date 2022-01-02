using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnOrderManager : MonoBehaviour
{
    public delegate void TurnHandler();
    public static event TurnHandler OnTurnBegun;
    public static event TurnHandler OnTurnEnded;

    //Debug
    public int currentTurnValue = 0;
    public Unit currentPlayingUnit;

    [Header("UI | Unit Competences")]
    [SerializeField] private Transform characterFramesParent;
    [SerializeField] private GameObject characterFramePrefab;
    public List<GameObject> instanceCreated = new List<GameObject>();

    #region Singleton
    public static TurnOrderManager Instance;

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
        FadingManager.OnTransition += InitTurnOrder;
    }

    private void OnDisable()
    {
        FadingManager.OnTransition -= InitTurnOrder;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeTurn();
        }
    }

    void CreateCharacterFrame(int neededAmt)
    {
        if (instanceCreated.Count <= 0)
        {
            instanceCreated.Clear();
        }

        for (int i = 0; i < neededAmt; i++)
        {
            GameObject newInstance = Instantiate(characterFramePrefab, characterFramesParent);

            CharacterFrameUIHandler characterFrameHandlerRef = newInstance.GetComponent<CharacterFrameUIHandler>();

            characterFrameHandlerRef.SetUnitRef(CombatManager.Instance.unitsInCombat [ i ]);

            instanceCreated.Add(newInstance);
        }
    }

    void UpdateCharacterTurnOrder()
    {
        //First goes at last position
        Transform firstChild = characterFramesParent.GetChild(0);
        firstChild.SetAsLastSibling();

        CharacterFrameUIHandler characterFrameHandlerRef = characterFramesParent.GetChild(0).GetComponent<CharacterFrameUIHandler>();
        SetCurrentPlayingUnit(characterFrameHandlerRef.unitRef);
    }

    private void SetCurrentPlayingUnit(Unit unit)
    {
        currentPlayingUnit = unit;
    }

    public Unit GetCurrentPlayingUnit()
    {
        return currentPlayingUnit;
    }

    public int GetCurrentTurnValue()
    {
        return currentTurnValue;
    }

    void InitTurnOrder()
    {
        int characterInCombat = CombatManager.Instance.unitsInCombat.Count;

        CreateCharacterFrame(characterInCombat);

        SetCurrentPlayingUnit(characterFramesParent.GetChild(0).GetComponent<CharacterFrameUIHandler>().unitRef);

        OnTurnBegun?.Invoke();
    }

    void UpdateCurrentTurnValue()
    {
        currentTurnValue++;
    }

    public void ChangeTurn()
    {
        UpdateCurrentTurnValue();

        UpdateCharacterTurnOrder();

        OnTurnEnded?.Invoke();
    }
}