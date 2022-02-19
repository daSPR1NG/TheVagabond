using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class AnimatorAssistant : MonoBehaviour
    {
        public List<AnimatorOverrideController> animatorOverrideControllers = new();

        protected Animator Animator => GetComponent<Animator>();

        public void SetAnimatorRunTimeController(int index)
        {
            Animator.runtimeAnimatorController = animatorOverrideControllers[index];
        }

        public void DestroyAnimatorParentGameObject()
        {
            Destroy(transform.parent);
        }

        public void DestroyAnimatorGameObject()
        {
            Destroy(gameObject);
        }
    }
}