using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterGearInventory : MonoBehaviour
    {
        public delegate void InventoryTracker(Gear gear);
        public event InventoryTracker OnAddingGear;
        public event InventoryTracker OnRemovingGear;

        [SerializeField] private Gear gearExemple;
        [SerializeField] private int gearHeldLimit = 6;
        private Gear _currentEquippedGear;
        [SerializeField] private List<Gear> characterGears = new();

        public List<Gear> CharacterGears { get => characterGears; private set => characterGears = value; }

        #region Inputs
        public KeyCode InventorySlot1 => InputsManager.Instance.GetInputByName("InventorySlot_1");
        public KeyCode InventorySlot2 => InputsManager.Instance.GetInputByName("InventorySlot_2");
        public KeyCode InventorySlot3 => InputsManager.Instance.GetInputByName("InventorySlot_3");
        public KeyCode InventorySlot4 => InputsManager.Instance.GetInputByName("InventorySlot_4");
        public KeyCode InventorySlot5 => InputsManager.Instance.GetInputByName("InventorySlot_5");
        public KeyCode InventorySlot6 => InputsManager.Instance.GetInputByName("InventorySlot_6");
        #endregion

        #region Public references
        private CharacterStats CharacterStats => GetComponent<CharacterStats>();
        public int GearHeldLimit { get => gearHeldLimit; }
        public Gear CurrentEquippedGear { get => _currentEquippedGear; private set => _currentEquippedGear = value; }
        #endregion

        protected virtual void Start()
        {
            //AddGear(gearExemple);
        }

        protected virtual void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions()) { return; }

            if (Input.GetKeyDown(KeyCode.N))
            {
                AddGear(gearExemple);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                //if (CharacterGears.Count == 0) { return; }

                //for (int i = characterGears.Count - 1; i >= 0; i--)
                //{
                //    RemoveGear(characterGears[i]);
                //}

                RemoveGear(characterGears[0]);
            }
        }

        #region Gear - Add / Remove
        public virtual void AddGear(Gear gear)
        {
            if (CharacterGears.Count >= GearHeldLimit)
            {
                //Send notification that inventory is full
                return;
            }

            CharacterGears.Add(gear);
            //AddGearStatModifiers(gear);

            OnAddingGear?.Invoke(gear);
        }

        public virtual void RemoveGear(Gear gear)
        {
            if (CharacterGears.Count == 0 || !CharacterGears.Contains(gear)) { return; }

            CharacterGears.Remove(gear);
            RemoveGearStatModifiers(gear);

            OnRemovingGear?.Invoke(gear);
        }

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

        public void EquipAGear(Gear gear)
        {
            if (characterGears.Count >= 1)
            {
                for (int i = 0; i < characterGears.Count; i++)
                {
                    RemoveGearStatModifiers(characterGears[i]);
                }
            }

            //if (CurrentEquippedGear) { RemoveGearStatModifiers(CurrentEquippedGear); }

            CurrentEquippedGear = gear;
            AddGearStatModifiers(gear);
        }
    }
}