using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class UIGearSlot : UIButton
    {
        [Header("DEPENDENCIES")]
        [SerializeField] private Image gearInSlotIconImage;

        #region Public References
        [field: SerializeField] public Gear GearInSlot { get; set; }
        public bool IsSlotEmpty => !GearInSlot;
        #endregion

        public void SetGearInSlot(Gear gear)
        {
            GearInSlot = gear;
            SetIcon(gear);
        }

        private void SetIcon(Gear gear)
        {
            gearInSlotIconImage.color = Color.white;
            gearInSlotIconImage.sprite = gear.GearSpriteIcon;
        }

        #region Pointer events - Enter / Exit / Click
        public override void OnPointerEnter(PointerEventData eventData)
        {
            //Show tooltip
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            //Hide tooltip
            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            //Show tooltip faster
        }
        #endregion
    }
}