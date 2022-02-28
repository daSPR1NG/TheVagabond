using UnityEngine;

namespace Khynan_Coding
{
    public interface IDamageable
    {
        public abstract void TakeDamage(Transform target, Transform attacker, float damage);
    }
}