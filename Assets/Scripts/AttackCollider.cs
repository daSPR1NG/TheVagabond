using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ObjectDebug))]
    public class AttackCollider : DetectionZone
    {
        [SerializeField] private CharacterStats characterStats;

        #region Public References

        #endregion

        private void OnEnable()
        {
            Invoke(nameof(SafetyCheck), 0.33f);
        }

        private void OnDisable()
        {
            transformFound.Clear();
        }

        protected override void Start()
        {
            base.Start();
            Init();
        }

        private void OnTriggerEnter(Collider other)
        {
            CharacterStats otherStats = other.GetComponent<CharacterStats>();

            if (!otherStats || otherStats.CharacterIsDead)
            {
                Debug.LogError("No entity found");
                return;
            }

            AddFoundTransform(other.transform);
        }

        //Set the range of this collider, equal to the weapon range
        void Init()
        {
            //Set all datas that need it at the start of the game
        }

        protected override void AddFoundTransform(Transform other)
        {
            transformFound.Add(other);

            if (transformFound.Count == 0) 
            {
                Debug.Log("The list transformFound is empty.");
                return; 
            }

            for (int i = 0; i < transformFound.Count; i++)
            {
                if (transformFound[i] == characterStats.transform) { continue; }

                CharacterStats otherStats = transformFound[i].GetComponent<CharacterStats>();

                otherStats.TakeDamage(transformFound[i], characterStats.transform, 15);
            }

            gameObject.SetActive(false);
        }

        private void SafetyCheck()
        {
            if (transformFound.Count == 0)
            {
                Debug.Log("The list transformFound is empty.");
                gameObject.SetActive(false);
            }
        }
    }
}