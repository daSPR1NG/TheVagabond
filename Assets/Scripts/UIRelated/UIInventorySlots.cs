using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class UIInventorySlots : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private Transform gearInventorySlotsParent;
        [SerializeField] private GameObject gearInventorySlotGO;
        private readonly List<UIGearSlot> _gearSlots = new();
        private CharacterGearInventory _characterInventory;

        #region Public References
        public CharacterGearInventory CharacterInventory { get => _characterInventory; private set => _characterInventory = value; }
        #endregion

        private void OnEnable()
        {
            CharacterInventory = GameManager.Instance.ActivePlayer.GetComponent<CharacterGearInventory>();
            CharacterInventory.OnAddingGear += AddGearInASlot;
        }

        private void OnDisable()
        {
            CharacterInventory.OnAddingGear -= AddGearInASlot;
        }

        void Start()
        {
            Init();
        }

        private void Update()
        {
            HandleGearInventoryInputs(_characterInventory.InventorySlot1, 0);
            HandleGearInventoryInputs(_characterInventory.InventorySlot2, 1);
            HandleGearInventoryInputs(_characterInventory.InventorySlot3, 2);
            HandleGearInventoryInputs(_characterInventory.InventorySlot4, 3);
            HandleGearInventoryInputs(_characterInventory.InventorySlot5, 4);
            HandleGearInventoryInputs(_characterInventory.InventorySlot6, 5);
        }

        #region Initialization
        void Init()
        {
            CreateSlot();
        }

        void CreateSlot()
        {
            if (!gearInventorySlotGO || !gearInventorySlotsParent || !CharacterInventory)
            {
                Debug.LogError("Slot prefab or parent transform or character inventory reference is missing.", transform);
                return; 
            }

            for (int i = 0; i < CharacterInventory.GearHeldLimit; i++)
            {
                GameObject gearSlotInstance = Instantiate(gearInventorySlotGO, gearInventorySlotsParent);

                UIGearSlot uiGearSlot = gearSlotInstance.GetComponent<UIGearSlot>();

                _gearSlots.Add(uiGearSlot);

                //Faire en sorte que si ce n'est pas alphaNum 1, 2... le text change et ne soit pas 1, 2, 3, etc...
                uiGearSlot.SetupGearSlotOnCreation(this, (i + 1).ToString(), InputsManager.Instance.GetInputByIndex(10 + i));
            }
        }
        #endregion

        void AddGearInASlot(Gear gear)
        {
            for (int i = 0; i < _gearSlots.Count; i++)
            {
                if (!_gearSlots[i].IsSlotEmpty) { continue; }

                _gearSlots[i].SetGearInSlot(gear);

                if (!CharacterInventory.CurrentEquippedGear) 
                {
                    CharacterInventory.EquipAGear(gear);
                    _gearSlots[i].SelectButton();
                }

                return;
            }
        }

        #region Slot selection & deselection
        private void HandleGearInventoryInputs(KeyCode keyCode, int slotIndex)
        {
            if (!_gearSlots[slotIndex].GearInSlot)
            {
                Debug.Log("No gear in this slot, can't equip.");
                return;
            }

            if (Helper.IsKeyPressed(keyCode))
            {
                DeselectEachGearSlots();

                _characterInventory.EquipAGear(_characterInventory.CharacterGears[slotIndex]);
                _gearSlots[slotIndex].SelectButton();
            }
        }

        public void DeselectEachGearSlots()
        {
            if (_gearSlots.Count == 0)
            {
                Debug.LogError("Gear slots list is empty !");
                return;
            }

            for (int i = 0; i < _gearSlots.Count; i++)
            {
                _gearSlots[i].DeselectButton();
            }
        }
        #endregion
    }
}