using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class InteractionUIWorld : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private TMP_Text keyText;
        [SerializeField] private Image interactionActionImage;
        [SerializeField] private GameObject contentGO;
        [SerializeField] private InteractionDetectionZone interactionDetectionZone;


        [Header("OFFSET SETTINGS")]
        [SerializeField] private Vector3 offsetPosition = Vector3.zero;
        [SerializeField] private Vector3 offsetRotation = Vector3.zero;

        #region Public references
        private RectTransform RectTransform => GetComponent<RectTransform>();
        private Animator Animator => GetComponent<Animator>();
        private Canvas Canvas => GetComponent<Canvas>();
        #endregion

        private void OnEnable()
        {
            interactionDetectionZone.OnTriggerEnter += DisplayContent;
            interactionDetectionZone.OnTriggerExit += HideContent;
        }

        private void OnDisable()
        {
            interactionDetectionZone.OnTriggerEnter -= DisplayContent;
            interactionDetectionZone.OnTriggerExit -= HideContent;
        }

        void Start()
        {
            Init();
        }

        private void Init()
        {
            Canvas.worldCamera = Helper.GetMainCamera();
            SetKeyText(InputsManager.Instance.GetInput("Interaction").ToString());
            //HideContent();
        }

        private void SetKeyText(string value)
        {
            if (keyText.text == value) { return; }

            keyText.SetText(value);
        }

        private void SetPosition()
        {
            if (RectTransform.localPosition == offsetPosition) { return; }

            RectTransform.localPosition = offsetPosition;
        }

        private void SetRotation()
        {
            if (RectTransform.localEulerAngles == offsetRotation) { return; }

            RectTransform.localEulerAngles = offsetRotation;
        }

        private void DisplayContent()
        {
            if (contentGO && contentGO.activeInHierarchy) { return; }

            contentGO.SetActive(true);

            AnimatorHelper.SetAnimatorBooleanParameter(Animator, "FadeOut", false);
            AnimatorHelper.SetAnimatorBooleanParameter(Animator, "IsEnabled", true);
        }

        private void HideContent()
        {
            if (contentGO && !contentGO.activeInHierarchy) { return; }

            AnimatorHelper.SetAnimatorBooleanParameter(Animator, "FadeOut", true);
            AnimatorHelper.SetAnimatorBooleanParameter(Animator, "IsEnabled", false);
        }

        #region Editor - OnValidate
        private void OnValidate()
        {
            SetPosition();
            SetRotation();
        }
        #endregion
    }
}