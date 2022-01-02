using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Competence", fileName = "Competence_SO_", order = 2)]
public class Competence : ScriptableObject
{
    public string competenceName;
    public Sprite icon;
}