using UnityEngine;

namespace Khynan_Coding
{
    public class PlayerDataManager : MonoBehaviour
    {
        [Header("PLAYER")]
        private Transform playerCharacterTransform;

        [Space]
        [Header("PLAYER INFORMATIONS")]
        public Vector3 playerPosition;

        #region Singleton
        public static PlayerDataManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SetPlayerCharacterTransform();
            }
        }
        #endregion

        #region Player position
        public void SavePlayerPosition()
        {
            playerPosition = GetCharacterPosition(playerCharacterTransform);
            //Save it with PlayerPrefs
        }

        public Vector3 GetCharacterPosition(Transform observedTransform)
        {
            if (playerPosition == Vector3.zero)
            {
                return observedTransform.position;
            }

            return playerPosition;
        }

        public void SetCharacterPosition(Transform observedTransform, Vector3 newPos)
        {
            SetPlayerCharacterTransform();
            observedTransform.position = newPos;
        }
        #endregion

        public Transform GetPlayerCharacterTransform()
        {
            return playerCharacterTransform;
        }

        private void SetPlayerCharacterTransform()
        {
            playerCharacterTransform = transform;
        }

        public bool PlayerCharacterIsActive()
        {
            if (playerCharacterTransform.gameObject.activeInHierarchy)
            {
                return true;
            }

            return false;
        }
    }
}