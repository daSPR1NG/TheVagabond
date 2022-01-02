using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Unit", fileName = "Unit_SO_", order = 1)]
public class Unit : ScriptableObject
{
    public string unitName;
    public int ID = 0;
    public Sprite icon;
    public int initiative = 50;

    [Space]

    public Team team = Team.Unassigned;
    public UnitType type = UnitType.Unassigned;

    [Space]

    public GameObject prefab;
    public List<Competence> competences;

    //Every getter needed
}