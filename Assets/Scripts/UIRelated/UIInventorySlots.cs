using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class UIInventorySlots : MonoBehaviour
    {
        [Header("DEPENDENCIES")]
        [SerializeField] private Transform gearInventorySlotsParent;
        [SerializeField] private GameObject gearInventorySlotGO;
        private readonly List<UIGearSlot> _gearSlots = new();
        private CharacterGearInventory _gearInventory;

        #region Public References
        public CharacterGearInventory GearInventory { get => _gearInventory; private set => _gearInventory = value; }
        #endregion

        void Start() => Init();

        #region Initialization
        void Init()
        {
            GearInventory = GameManager.Instance.ActivePlayer.GetComponent<CharacterGearInventory>();
            CreateSlot();
        }

        void CreateSlot()
        {
            if (!gearInventorySlotGO || !gearInventorySlotsParent || !GearInventory)
            {
                Debug.LogError("Slot prefab or parent transform or character inventory reference is missing.", transform);
                return;
            }

            for (int i = 0; i < GearInventory.CharacterGears.Count; i++)
            {
                GameObject gearSlotInstance = Instantiate(gearInventorySlotGO, gearInventorySlotsParent);

                UIGearSlot uiGearSlot = gearSlotInstance.GetComponent<UIGearSlot>();
                uiGearSlot.SetGearInSlot(GearInventory.CharacterGears[i]);

                _gearSlots.Add(uiGearSlot);
            }
        }
        #endregion
    }
}