using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class PlayerCharacter_AnimatorAssistant : AnimatorAssistant
    {
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