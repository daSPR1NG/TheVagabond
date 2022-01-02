using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExperienceManager))]
public class ExperienceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExperienceManager experienceManager = (ExperienceManager) target;

        GUILayout.Space(5);

        if (GUILayout.Button(new GUIContent("ADD EXP", "Adds " + experienceManager.experienceToAdd.ToString() + " experience.")))
        {
            experienceManager.AddExperience(experienceManager.experienceToAdd);
            Debug.Log("Experience added : " + experienceManager.experienceToAdd.ToString());
        }

        GUILayout.Space(2);

        if (GUILayout.Button(new GUIContent("LEVEL UP", "")))
        {
            experienceManager.LevelUp();
            Debug.Log("Level Up");
        }

        GUILayout.Space(2);

        if (GUILayout.Button(new GUIContent("RESET ALL PARAMETERS", "")))
        {
            experienceManager.ResetParameters();
            Debug.Log("Reset !");
        }
    }
}
