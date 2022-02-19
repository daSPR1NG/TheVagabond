using UnityEngine;

namespace Khynan_Coding
{
    public class InputsManager : MonoBehaviour
    {
        [SerializeField] private InputsMap inputsMap;

        #region Singleton
        public static InputsManager Instance;

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
            }
        }
        #endregion

        public void ChangeInputKeyCode(string actionName, KeyCode newKeyCode)
        {
            if (!inputsMap || inputsMap.InputKeyDatas.Count == 0) 
            {
                Debug.LogError("Input map is not set or the key list is empty !");
                return; 
            }

            for (int i = 0; i < inputsMap.InputKeyDatas.Count; i++)
            {
                if (inputsMap.InputKeyDatas[i].Name != actionName) { continue; }

                inputsMap.InputKeyDatas[i].SetKeyCode(newKeyCode);
            }
        }

        public KeyCode GetInput(string actionName)
        {
            if (!inputsMap || inputsMap.InputKeyDatas.Count == 0) 
            {
                Debug.LogError("Input map is not set or the key list is empty !");
                return KeyCode.None; 
            }

            for (int i = 0; i < inputsMap.InputKeyDatas.Count; i++)
            {
                if (inputsMap.InputKeyDatas[i].Name != actionName) { continue; }

                return inputsMap.InputKeyDatas[i].KeyCode;
            }

            return KeyCode.None;
        }
    }
}