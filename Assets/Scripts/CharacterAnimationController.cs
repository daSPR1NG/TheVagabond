using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class CharacterAnimationController : MonoBehaviour
    {
        public Animator Animator => transform.GetChild(0).GetComponent<Animator>();

        #region Trigger
        public void SetAnimationTrigger(Animator animator, string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
        #endregion

        #region Boolean - Get/Set
        public void GetAnimationTrigger(Animator animator, string booleanName)
        {
            animator.GetBool(booleanName);
        }

        public void SetAnimationBoolean(Animator animator, string booleanName, bool value)
        {
            animator.SetBool(booleanName, value);
        }
        #endregion

        #region Float - Get/Set
        public void GetAnimationFloatValue(Animator animator, string floatParameterName)
        {
            animator.GetFloat(floatParameterName);
        }

        public void SetAnimationFloatValue(Animator animator, string floatParameterName, float value, float dampTime)
        {
            animator.SetFloat(floatParameterName, value, dampTime, Time.deltaTime);
        }
        #endregion

        #region Integer - Get/Set
        public void GetAnimationIntValue(Animator animator, string intParameterName)
        {
            animator.GetInteger(intParameterName);
        }

        public void SetAnimationIntValue(Animator animator, string intParameterName, int value)
        {
            animator.SetInteger(intParameterName, value);
        }
        #endregion
    }
}