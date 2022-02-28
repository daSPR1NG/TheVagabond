using UnityEngine;

namespace Khynan_Coding
{
    public interface IHealable
    {
        public abstract void Heal(Transform target, Transform ally, float healAmount);
    }
}