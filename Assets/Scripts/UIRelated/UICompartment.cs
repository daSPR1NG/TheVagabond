using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(UICompartment_AnimatorAssistant))]
    public class UICompartment : MonoBehaviour
    {
        [SerializeField] private float displayDuration = 0.5f;
        private float _currentDisplayDuration;

        private Animator Animator => GetComponent<Animator>();

        private void OnEnable() => _currentDisplayDuration = AnimatorHelper.GetAnimationLength(Animator, 0) + displayDuration;

        private void Update() => ProcessDisplayDurationTimer();

        private void ProcessDisplayDurationTimer()
        {
            _currentDisplayDuration -= Time.deltaTime;
            Debug.Log("WAITING");
           
            if (!(_currentDisplayDuration <= 0)) { return; }

            AnimatorHelper.SetAnimatorBooleanParameter(Animator, "FadeOut", true);
            Debug.Log("FADE OUT");
        }
    }
}