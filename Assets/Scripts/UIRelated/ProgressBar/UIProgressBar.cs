using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class UIProgressBar : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private bool isUsingFilledProgressBarType = true;
        [Tooltip("Notes that the element 0 is always the filled progress bar type, the element 1 is always the other type")]
        [SerializeField] private List<ProgressBarElements> progressBarElements = new();

        [Header("FILL IMAGE SETTINGS")]
        [SerializeField] private bool doesItFillUp = false;
        [SerializeField] private float refreshMultiplier = 1f;
        private float currentFillValue = 0f;
        private float maxFillValue = 0f;

        #region Public references
        public float CurrentFillValue { get => currentFillValue; private set => currentFillValue = Mathf.Clamp(value, 0, maxFillValue); }
        #endregion

        [System.Serializable]
        public class ProgressBarElements
        {
            [SerializeField] private string m_Name;

            [Header("SETUP")]
            [SerializeField] private GameObject progressBarContentToActivate;
            [SerializeField] private TMP_Text actionText;

            //Used when its a filled progress bar type
            [SerializeField] private Image fillImage;
            [SerializeField] private TMP_Text progressTimerText;

            //Used when its a updated image progress bar type
            [SerializeField] private Image imageToUpdate;
            [SerializeField] private Sprite[] imageSprites;

            #region Public references
            public GameObject ProgressBarContentToActivate { get => progressBarContentToActivate; }
            public TMP_Text ActionText { get => actionText; }
            public Image FillImage { get => fillImage; }
            public TMP_Text ProgressTimerText { get => progressTimerText; }
            public Image ImageToUpdate { get => imageToUpdate; }
            public Sprite[] ImageSprites { get => imageSprites; }
            #endregion
        }

        protected virtual void Update() => UpdateImageFillAmout(maxFillValue);

        protected virtual void Init(float currentValue, float maxValue, string attachedTextContent = null)
        {
            DisplayProgressBar();
            InitImageFillAmount(currentValue, maxValue);

            if (!string.IsNullOrEmpty(attachedTextContent)) 
            {
                switch (isUsingFilledProgressBarType)
                {
                    case true :
                        progressBarElements[0].ActionText.SetText(attachedTextContent);
                        break;
                    case false:
                        progressBarElements[1].ActionText.SetText(attachedTextContent);
                        break;
                }
            }
        }

        private void InitImageFillAmount(float currentValue, float maxValue)
        {
            CurrentFillValue = currentValue;
            maxFillValue = maxValue;

            progressBarElements[0].FillImage.fillAmount = currentValue / maxValue;
        }

        private void UpdateImageFillAmout(float maxValue)
        {
            if (!progressBarElements[0].ProgressBarContentToActivate.activeInHierarchy) { return; }

            Debug.Log("Update image fill amount");

            CurrentFillValue += doesItFillUp ?
                Time.deltaTime * refreshMultiplier : -Time.deltaTime * refreshMultiplier;

            progressBarElements[0].ProgressTimerText.SetText(CurrentFillValue.ToString("0.0" + "s"));

            progressBarElements[0].FillImage.fillAmount = CurrentFillValue / maxValue;
        }

        protected void DisplayProgressBar()
        {
            switch (isUsingFilledProgressBarType)
            {
                case true:
                    progressBarElements[0].ProgressBarContentToActivate.SetActive(true);
                    break;
                case false:
                    progressBarElements[1].ProgressBarContentToActivate.SetActive(true);
                    break;
            }
        }

        protected void HideProgressBar()
        {
            switch (isUsingFilledProgressBarType)
            {
                case true:
                    progressBarElements[0].ProgressBarContentToActivate.SetActive(false);
                    break;
                case false:
                    progressBarElements[1].ProgressBarContentToActivate.SetActive(false);
                    break;
            }
        }
    }
}