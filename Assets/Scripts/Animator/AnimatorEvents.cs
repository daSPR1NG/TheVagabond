using UnityEngine;

namespace Khynan_Coding
{
    public class AnimatorEvents : MonoBehaviour
    {
        private Animator Animator => GetComponent<Animator>();

        public void SetIdleActionValueToZeroEvent()
        {
            AnimatorHelper.SetAnimatorIntParameter(Animator, "IdleActionValue", 0);
        }
        
        public void ResetControllerTimeSpentInIdleEvent()
        {
            transform.root.GetComponent<CharacterController>().ResetTimeSpentInIdleValue();
        }
    }
}