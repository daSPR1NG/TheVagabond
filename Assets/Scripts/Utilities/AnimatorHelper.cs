using UnityEngine;

namespace Khynan_Coding
{
    public static class AnimatorHelper
    {
        #region Trigger - Set
        public static void SetAnimatorTriggerParameter(Animator animator, string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
        #endregion

        #region Boolean - Get/Set
        public static bool GetAnimatorBooleanParameter(Animator animator, string booleanName)
        {
            return animator.GetBool(booleanName);
        }

        public static void SetAnimatorBooleanParameter(Animator animator, string booleanName, bool value)
        {
            animator.SetBool(booleanName, value);
        }
        #endregion

        #region Float - Get/Set
        public static float GetAnimatorFloatParameter(Animator animator, string floatParameterName)
        {
            return animator.GetFloat(floatParameterName);
        }

        public static void SetAnimatorFloatParameter(Animator animator, string floatParameterName, float value, float dampTime = 0)
        {
            animator.SetFloat(floatParameterName, value, dampTime, Time.deltaTime);
        }
        #endregion

        #region Integer - Get/Set
        public static int GetAnimatorIntParameter(Animator animator, string intParameterName)
        {
            return animator.GetInteger(intParameterName);
        }

        public static void SetAnimatorIntParameter(Animator animator, string intParameterName, int value)
        {
            animator.SetInteger(intParameterName, value);
        }
        #endregion

        #region Layer
        public static void SetAnimatorActiveLayer(Animator animator, int layerIndex, float layerWeight)
        {
            animator.SetLayerWeight(layerIndex, layerWeight);
        }
        #endregion

        #region Boolean - States
        public static bool IsThisAnimationPlaying(Animator animator, string animationName)
        {
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


        //Change it to be a function that returns all the animationClips length of an animator
         //for (int i = 0; i<interactionActorInteractionHandler.GetComponent<CharacterController>().Animator.runtimeAnimatorController.animationClips.Length; i++)
         //   {
         //       Debug.Log("DEBUG | Index : " + i + " "
         //       + interactionActorInteractionHandler.GetComponent<CharacterController>().Animator.runtimeAnimatorController.animationClips[i].name
         //       + " | Length : "
         //       + interactionActorInteractionHandler.GetComponent<CharacterController>().Animator.runtimeAnimatorController.animationClips[i].length + " | AverageDuration : "
         //       + interactionActorInteractionHandler.GetComponent<CharacterController>().Animator.runtimeAnimatorController.animationClips[i].length);
         //   }
}
}