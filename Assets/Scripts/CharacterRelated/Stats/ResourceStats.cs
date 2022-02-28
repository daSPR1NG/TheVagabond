using UnityEngine;

namespace Khynan_Coding
{
    public class ResourceStats : CharacterStats
    {
        #region Public References

        #endregion

        protected override void Start() => base.Start();

        protected override void Update() => base.Update();

        protected override void CalculateHealthPercentage(float current, float max)
        {
            base.CalculateHealthPercentage(current, max);
        }

        protected override bool IsCloseToDie()
        {
            return base.IsCloseToDie();
        }

        protected override void OnDeath()
        {
            base.OnDeath();
        }

        public override void TakeDamage(Transform target, Transform attacker, float damage)
        {
            base.TakeDamage(target, attacker, damage);
        }
    }
}