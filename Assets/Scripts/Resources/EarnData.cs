using UnityEngine;

namespace Khynan_Coding
{
    public class EarnData
    {
        public string Name;
        public float Value;
        public object Source;

        #region Public References

        #endregion

        public EarnData(string name, float value, object source)
        {
            Name = name;
            Value = value;
            Source = source;
        }
    }
}