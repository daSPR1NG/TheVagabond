using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Unassigned,
    Warrior,
    Assassin,
    Mage,
    Paladin,
    Monk,
    Priest,
}

public enum Team
{
    Unassigned,
    Ally,
    Ennemy,
    Neutral,
}

public class TeamManager : MonoBehaviour
{
    [Header("PLAYER TEAM")]
    public int maxUnitsInPlayerTeam = 3;
    public List<Unit> currentPlayerTeam;

    [Space] [Header("ENEMY TEAM")]
    public int maxUnitsInEnemyTeam = 3;
    public List<Unit> currentEnemyTeam;

    #region Singleton
    public static TeamManager Instance;

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
        ArenaManager.OnEnteringArena += PositionTeamUnits;
    }

    private void OnDisable()
    {
        ArenaManager.OnEnteringArena -= PositionTeamUnits;
    }

    private void PositionTeamUnits()
    {
        SetPlayerUnitsPosition();
        SetEnemyUnitsPosition();
    }

    private void SetPlayerUnitsPosition()
    {
        ArenaManager.Arena activeArena = ArenaManager.Instance.GetActiveArena();

        for (int i = 0; i < currentPlayerTeam.Count; i++)
        {
            DefineSpecificPosition(i, currentPlayerTeam, activeArena.playerUnitsPos);
        }
    }

    private void SetEnemyUnitsPosition()
    {
        ArenaManager.Arena activeArena = ArenaManager.Instance.GetActiveArena();

        for (int i = 0; i < currentEnemyTeam.Count; i++)
        {
            DefineSpecificPosition(i, currentEnemyTeam, activeArena.enemiesUnitsPos);
        }
    }

    private void DefineSpecificPosition(int index, List<Unit> team, List<Transform> unitsPositions)
    {
        switch (team.Count)
        {
            case 1:
                _ = Instantiate(team [ index ].prefab, unitsPositions [ 1 ].position, unitsPositions [ 1 ].rotation, unitsPositions [ 1 ]);
                break;
            case 2:
                GameObject instance = Instantiate(team [ index ].prefab);

                if (index == 1) index = 2;

                InstantiateAndSetInstanceParent(instance, unitsPositions [ index ].position, unitsPositions [ index ].rotation, unitsPositions [ index ]);
                break;
            case >= 3:
                _ = Instantiate(team [ index ].prefab, unitsPositions [ index ].position, unitsPositions [ index ].rotation, unitsPositions [ index ]);
                break;
        }
    }

    private void InstantiateAndSetInstanceParent(GameObject instance, Vector3 pos, Quaternion rotation, Transform parent)
    {
        instance.transform.SetPositionAndRotation(pos, rotation);
        instance.transform.SetParent(parent);
    }

    #region Add | Remove from a team
    public void AddToATeam(List<Unit> team, int unitsMaxNumber, Unit unit, bool doClearTeam = false)
    {
        if (doClearTeam) team.Clear();

        if (IsThisTeamFull(team, unitsMaxNumber)) return;

        team.Add(unit);
    }

    public void RemoveFromATeam(List<Unit> team, int unitID)
    {
        if (IsThisTeamEmpty(team)) return;

        team.Remove(GetAUnitFromCurrentTeam(team, unitID));
    }
    #endregion

    #region Team states
    private bool IsThisTeamEmpty(List<Unit> team)
    {
        if (team.Count <= 0)
        {
            return true;
        }

        return false;
    }

    private bool IsThisTeamFull(List<Unit> team, int comparedMaxValue)
    {
        if (team.Count >= comparedMaxValue)
        {
            return true;
        }

        return false;
    }
    #endregion

    public Unit GetAUnitFromCurrentTeam(List<Unit> team, int unitID)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (team [ i ].ID == unitID)
            {
                return team [ i ];
            }
        }

        return null;
    }
}