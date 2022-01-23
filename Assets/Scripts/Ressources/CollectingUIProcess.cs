using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class CollectingUIProcess : MonoBehaviour
    {
        public Transform content;

        [Space]
        [Header("UI COMPONENTS")]
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI actionText;
        public Image filledImage;

        private float currentTimerValue;
        private float maxTimer;

        private void OnEnable()
        {
            Player_InteractionHandler.OnInteraction += SetUIDatas;
            Player_InteractionHandler.OnEndOfInteraction += ResetUIs;
        }

        private void OnDisable()
        {
            Player_InteractionHandler.OnInteraction -= SetUIDatas;
            Player_InteractionHandler.OnEndOfInteraction -= ResetUIs;
        }

        private void Update()
        {
            if (content.gameObject.activeInHierarchy) ProcessTimer();
        }

        public void SetUIDatas(float currentTimerValue, float maxDuration, string action)
        {
            content.gameObject.SetActive(true);

            //Storing passed datas
            maxTimer = maxDuration;
            this.currentTimerValue = currentTimerValue;

            timerText.text = currentTimerValue.ToString("0.00");

            filledImage.fillAmount = currentTimerValue / maxDuration;

            actionText.text = action;
        }

        private void SetUIAtRuntime(float timerValue)
        {
            if (timerValue > 1) timerText.text = timerValue.ToString("0");

            if (timerValue <= 1) timerText.text = timerValue.ToString("0.0");

            filledImage.fillAmount = timerValue / maxTimer;
        }

        private void ResetUIs()
        {
            maxTimer = 0;
            currentTimerValue = 0;

            content.gameObject.SetActive(false);
        }

        private void ProcessTimer()
        {
            currentTimerValue -= Time.deltaTime;

            SetUIAtRuntime(currentTimerValue);

            if (currentTimerValue <= 0) content.gameObject.SetActive(false);
        }
    }
}