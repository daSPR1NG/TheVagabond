using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class UIProgressBar : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private GameObject progressBarGO;
        [SerializeField] private Image fillImage;
        [SerializeField] private TMP_Text actionText;
        [SerializeField] private TMP_Text progressTimerText;

        [Header("FILL IMAGE SETTINGS")]
        [SerializeField] private bool doesItFillUp = false;
        [SerializeField] private float refreshMultiplier = 1f;
        private float currentFillValue = 0f;
        private float maxFillValue = 0f;

        #region Public references
        public float CurrentFillValue { get => currentFillValue; private set => currentFillValue = Mathf.Clamp(value, 0, maxFillValue); }
        #endregion

        protected virtual void Update() => UpdateImageFillAmout(maxFillValue);

        protected virtual void Init(float currentValue, float maxValue, string attachedTextContent = null)
        {
            DisplayProgressBar();
            InitImageFillAmount(currentValue, maxValue);

            if (!string.IsNullOrEmpty(attachedTextContent)) { actionText.SetText(attachedTextContent); }
        }

        private void InitImageFillAmount(float currentValue, float maxValue)
        {
            CurrentFillValue = currentValue;
            maxFillValue = maxValue;

            fillImage.fillAmount = currentValue / maxValue;
        }

        private void UpdateImageFillAmout(float maxValue)
        {
            if (!progressBarGO.activeInHierarchy) { return; }

            Debug.Log("Update image fill amount");

            CurrentFillValue += doesItFillUp ?
                Time.deltaTime * refreshMultiplier : -Time.deltaTime * refreshMultiplier;

            progressTimerText.SetText(CurrentFillValue.ToString("0.0" + "s"));

            fillImage.fillAmount = CurrentFillValue / maxValue;
        }

        protected void DisplayProgressBar()
        {
            progressBarGO.SetActive(true);
        }

        protected void HideProgressBar()
        {
            progressBarGO.SetActive(false);
        }
    }
}