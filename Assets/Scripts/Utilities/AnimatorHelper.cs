using UnityEngine;

namespace Khynan_Coding
{
    public static class AnimatorHelper
    {
        #region Trigger - Set
        public static void SetAnimatorTriggerParameter(Animator animator, string triggerName)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return;
            }

            animator.SetTrigger(triggerName);
        }
        #endregion

        #region Boolean - Get/Set
        public static bool GetAnimatorBooleanParameter(Animator animator, string booleanName)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return false;
            }

            return animator.GetBool(booleanName);
        }

        public static void SetAnimatorBooleanParameter(Animator animator, string booleanName, bool value)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return;
            }

            animator.SetBool(booleanName, value);
        }
        #endregion

        #region Float - Get/Set
        public static float GetAnimatorFloatParameter(Animator animator, string floatParameterName)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return 0;
            }

            return animator.GetFloat(floatParameterName);
        }

        public static void SetAnimatorFloatParameter(Animator animator, string floatParameterName, float value, float dampTime = 0)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return;
            }

            animator.SetFloat(floatParameterName, value, dampTime, Time.deltaTime);
        }
        #endregion

        #region Integer - Get/Set
        public static int GetAnimatorIntParameter(Animator animator, string intParameterName)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return 0;
            }

            return animator.GetInteger(intParameterName);
        }

        public static void SetAnimatorIntParameter(Animator animator, string intParameterName, int value)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return;
            }

            animator.SetInteger(intParameterName, value);
        }
        #endregion

        #region Layer
        public static void SetAnimatorActiveLayer(Animator animator, int layerIndex, float layerWeight)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return;
            }

            animator.SetLayerWeight(layerIndex, layerWeight);
        }
        #endregion

        #region Boolean - States
        public static bool IsThisAnimationPlaying(Animator animator, string animationName)
        {
            if (!animator)
            {
                Debug.LogError("No animator.");
                return false;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
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

        public static bool HasParameter(Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName) return true;
            }
            return false;
        }

        public static void DebugAnimationsDuration(Animator animator)
        {
            if (!animator || animator.runtimeAnimatorController.animationClips.Length == 0)
            {
                Debug.LogError("No animator or the list of animationClips is empty.");
                return;
            }

            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                Debug.Log("DEBUG | Index : " + i + " "
                + animator.runtimeAnimatorController.animationClips[i].name
                + " | Length : "
                + animator.runtimeAnimatorController.animationClips[i].length + " | AverageDuration : "
                + animator.runtimeAnimatorController.animationClips[i].length);
            }
        }

        public static float GetAnimationLength(Animator animator, int animationIndex)
        {
            return animator.runtimeAnimatorController.animationClips[animationIndex].length;
        }
    }
}