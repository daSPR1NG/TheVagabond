using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("COMBAT INFORMATIONS")]
    public int combatAmount = 2;
    [Space] public List<Unit> unitsInCombat;

    #region Singleton
    public static CombatManager Instance;

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
        GameManager.OnEnteringCombat += EnterCombat;
        GameManager.OnExitingCombat += ExitCombat;
    }

    private void OnDisable()
    {
        GameManager.OnEnteringCombat -= EnterCombat;
        GameManager.OnExitingCombat -= ExitCombat;
    }

    #region Combat transition
    private void EnterCombat()
    {
        GetAllUnitsPresentInCombat();
    }

    private void ExitCombat()
    {
        RemoveAllUnitsInCombat();
    }
    #endregion

    #region Player character activation state | Saving datas before transition | Reset/Set datas after transition
    public void TogglePlayerCharacterActivation()
    {
        if (!PlayerDataManager.Instance.PlayerCharacterIsActive())
        {
            ActivatePlayerCharacter();
            return;
        }

        DesactivatePlayerCharacter();
    }

    private void ActivatePlayerCharacter()
    {
        PlayerDataManager playerDataManager = PlayerDataManager.Instance;
        GameObject playerCharacter = playerDataManager.GetPlayerCharacterTransform().gameObject;
        
        playerCharacter.SetActive(true);

        playerDataManager.SetCharacterPosition(playerCharacter.transform, playerDataManager.GetCharacterPosition(playerCharacter.transform));
    }

    private void DesactivatePlayerCharacter()
    {
        PlayerDataManager playerDataManager = PlayerDataManager.Instance;
        GameObject playerCharacter = playerDataManager.GetPlayerCharacterTransform().gameObject;

        playerDataManager.SavePlayerPosition();

        playerCharacter.SetActive(false);
    }

    private void GetAllUnitsPresentInCombat()
    {
        for (int i = 0; i < TeamManager.Instance.currentPlayerTeam.Count; i++)
        {
            unitsInCombat.Add(TeamManager.Instance.currentPlayerTeam[ i ]);
        }

        for (int i = 0; i < TeamManager.Instance.currentEnemyTeam.Count; i++)
        {
            unitsInCombat.Add(TeamManager.Instance.currentEnemyTeam [ i ]);
        }

        unitsInCombat = unitsInCombat.OrderByDescending(unitsInCombat => unitsInCombat.initiative).ToList();
    }

    private void RemoveAllUnitsInCombat()
    {
        unitsInCombat.Clear();
    }
    #endregion
}