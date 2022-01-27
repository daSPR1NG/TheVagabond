using UnityEngine;

namespace Khynan_Coding
{
    public static class AnimatorHelper
    {
        #region Trigger - Set
        public static void SetAnimatorTriggerParameter(Animator animator, string triggerName)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.SetTrigger(triggerName);
        }
        #endregion

        #region Boolean - Get/Set
        public static void GetAnimatorBooleanParameter(Animator animator, string booleanName)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.GetBool(booleanName);
        }

        public static void SetAnimatorBooleanParameter(Animator animator, string booleanName, bool value)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.SetBool(booleanName, value);
        }
        #endregion

        #region Float - Get/Set
        public static void GetAnimatorFloatParameter(Animator animator, string floatParameterName)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.GetFloat(floatParameterName);
        }

        public static void SetAnimatorFloatParameter(Animator animator, string floatParameterName, float value, float dampTime)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.SetFloat(floatParameterName, value, dampTime, Time.deltaTime);
        }
        #endregion

        #region Integer - Get/Set
        public static void GetAnimatorIntParameter(Animator animator, string intParameterName)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.GetInteger(intParameterName);
        }

        public static void SetAnimatorIntParameter(Animator animator, string intParameterName, int value)
        {
            if (Helper.IsNull(ref animator)) { return; }

            animator.SetInteger(intParameterName, value);
        }
        #endregion

        #region Layer
        public static void SetAnimatorActiveLayer(Animator animator, int layerIndex, float layerWeight)
        {
            if (Helper.IsNull(ref animator)) { return; }

            //Current active layer
            animator.SetLayerWeight(layerIndex, layerWeight);
        }
        #endregion

        #region Boolean - States
        public static bool IsThisAnimationPlaying(Animator animator, string animationName)
        {
            if (Helper.IsNull(ref animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                return true;
            }

            return false;
        }
        #endregion

        public static void PlayThisAnimationOnThisLayer(Animator animator, int layerIndex, float layerWeight, string booleanName, bool value)
        {
            SetAnimatorActiveLayer(animator, layerIndex, layerWeight);
            SetAnimatorBooleanParameter(animator, booleanName, value);
        }
    }
}