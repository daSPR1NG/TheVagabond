using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterGearInventory : MonoBehaviour
    {
        [Header("DEPENDENCIES")]
        [SerializeField] private List<Gear> characterGears = new();

        #region Public references
        private CharacterStats CharacterStats => GetComponent<CharacterStats>();
        public List<Gear> CharacterGears { get => characterGears; private set => characterGears = value; }
        #endregion

        #region Gear - Add / Remove
        private void AddGearStatModifiers(Gear gear)
        {
            if (gear.GearStatModifiers.Count == 0)
            {
                Debug.LogError("No gear stat modifiers found." + transform);
                return;
            }

            AddOrRemoveModifierToThisStat(gear, false);
        }

        private void RemoveGearStatModifiers(Gear gear)
        {
            if (gear.GearStatModifiers.Count == 0)
            {
                Debug.LogError("No gear stat modifiers found." + transform);
                return;
            }

            AddOrRemoveModifierToThisStat(gear, true);
        }

        private void AddOrRemoveModifierToThisStat(Gear gear, bool doRemove)
        {
            for (int i = 0; i < gear.GearStatModifiers.Count; i++)
            {
                if (doRemove)
                {
                    CharacterStats.GetStatByType(gear.GearStatModifiers[i].ModifiedStatType).RemoveSourceModifier(gear);
                    Debug.Log(
                        "Gear modifier(s) has/ve been added upon adding or removing | doRemove = " + doRemove + " " + gear.GearName);
                    continue;
                }

                CharacterStats.GetStatByType(gear.GearStatModifiers[i].ModifiedStatType).AddModifier(new StatModifier(
                    gear.GearStatModifiers[i].ModifierType,
                    gear.GearStatModifiers[i].ModifierValue,
                    gear,
                    gear.GearStatModifiers[i].ModifiedStatType));

                Debug.Log("Gear modifier(s) has/ve been added upon adding or removing | doRemove = " + doRemove + " " + gear.GearName);
            }
        }
        #endregion
    }
}