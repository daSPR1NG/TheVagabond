using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class AnimatorAssistant : MonoBehaviour
    {
        public List<AnimatorOverrideController> animatorOverrideControllers = new();
        private Animator Animator => GetComponent<Animator>();

        public void SetIdleActionValueToZeroEvent()
        {
            AnimatorHelper.SetAnimatorIntParameter(Animator, "IdleActionValue", 0);
        }
        
        public void ResetControllerTimeSpentInIdleEvent()
        {
            transform.root.GetComponent<CharacterController>().ResetTimeSpentInIdleValue();
        }

        public void SetAnimatorRunTimeController(int index)
        {
            Animator.runtimeAnimatorController = animatorOverrideControllers[index];
        }
    }
}