using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class RessourceUI : MonoBehaviour
    {
        [Header("GENERAL SETTINGS")]
        public RessourceType ressourceType;

        #region Components
        [Space]
        [Header("COMPONENTS")]
        public TextMeshProUGUI valueText;
        public Image ressourceIconImage;
        public Animator AnimatorComponent => GetComponent<Animator>();
        #endregion

        public void SetThisUI(RessourceType ressourceType, float value, Sprite sprite)
        {
            this.ressourceType = ressourceType;
            valueText.text = value.ToString("0");
            ressourceIconImage.sprite = sprite;
        }

        public void UpdateRessourceValue(float newValue)
        {
            valueText.text = newValue.ToString("0");
        }

        public void PlayFeedbackAnimation()
        {
            if (Helper.IsAnimationPlaying(AnimatorComponent, "Anim_PlayerRessourceUI_Feedback"))
            {
                AnimatorComponent.ResetTrigger("TriggerEvent");
                Debug.Log("Reset Animation");
            }

            AnimatorComponent.SetTrigger("TriggerEvent");
        }
    }
}