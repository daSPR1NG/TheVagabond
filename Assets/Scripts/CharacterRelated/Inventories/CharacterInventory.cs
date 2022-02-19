using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    public class CharacterInventory : MonoBehaviour
    {
        public delegate void InventoryTracker(Gear gear);
        public event InventoryTracker OnAddingGear;

        [SerializeField] private Gear gearExemple;
        [SerializeField] private List<Gear> characterGears = new();

        public List<Gear> CharacterGears { get => characterGears; private set => characterGears = value; }

        protected virtual void Start()
        {
            AddGear(gearExemple);
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddGear(gearExemple);
            }
        }

        #region Gear - Add / Remove
        public virtual void AddGear(Gear gear)
        {
            CharacterGears.Add(gear);
            OnAddingGear?.Invoke(gear);
        }

        public virtual void RemoveGear(Gear gear)
        {
            if (CharacterGears.Count == 0 || !CharacterGears.Contains(gear)) { return; }

            CharacterGears.Remove(gear);
        }
        #endregion
    }
}