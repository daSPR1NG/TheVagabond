using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class UIGearSlot : UIButton
    {
        [Header("GEAR SLOT SETTINGS")]
        [SerializeField] private Gear gearInSlot;
        [SerializeField] private Image gearInSlotIconImage;
        [SerializeField] private TMP_Text inputText;
        private KeyCode _SlotKeycode;
        private UIInventorySlots _UIInventorySlots;

        #region Public References
        public Gear GearInSlot { get => gearInSlot; set => gearInSlot = value; }
        public bool IsSlotEmpty => !gearInSlot;
        #endregion

        public void SetupGearSlotOnCreation(UIInventorySlots uiInventorySlots, string keyCodeString, KeyCode keyCode)
        {
            //Set keybind text
            SetParentSlotScriptReference(uiInventorySlots);
            SetGearSlotInput(keyCodeString);
            _SlotKeycode = keyCode;
        }

        public void SetParentSlotScriptReference(UIInventorySlots uiInventorySlots)
        {
            _UIInventorySlots = uiInventorySlots;
        }

        public void SetGearSlotInput(string keyCodeString)
        {
            if (!inputText)
            {
                Debug.LogError("Input text is missing.");
                return;
            }

            inputText.SetText(keyCodeString);
        }

        public void SetGearInSlot(Gear gear)
        {
            gearInSlot = gear;
            SetGearInSlotImageIcon(gear);
        }

        private void SetGearInSlotImageIcon(Gear gear)
        {
            gearInSlotIconImage.color = Color.white;
            gearInSlotIconImage.sprite = gear.GearSpriteIcon;
        }

        #region Pointer events - Enter / Exit / Click
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!GearInSlot) { return; }

            _UIInventorySlots.DeselectEachGearSlots();
            _UIInventorySlots.CharacterInventory.EquipAGear(gearInSlot);

            base.OnPointerClick(eventData);
            //Envoyé l'info au combatsystem > currentEquippedGear set
        }
        #endregion
    }
}