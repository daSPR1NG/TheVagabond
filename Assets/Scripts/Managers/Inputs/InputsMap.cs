using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [CreateAssetMenu(menuName = "Utilities/InputsMap", fileName = "InputsMap_", order = 0)]
    public class InputsMap : ScriptableObject
    {
        [SerializeField] private List<InputKeyData> inputKeyDatas = new();

        #region Public references
        public List<InputKeyData> InputKeyDatas { get => inputKeyDatas; private set => inputKeyDatas = value; }
        #endregion

        [System.Serializable]
        public class InputKeyData
        {
            [SerializeField] private string m_Name;
            [SerializeField] private KeyCode keyCode;

            public string Name { get => m_Name; }
            public KeyCode KeyCode { get => keyCode; private set => keyCode = value; }

            public void SetKeyCode(KeyCode newKeyCode)
            {
                KeyCode = newKeyCode;
            }
        }
    }
}