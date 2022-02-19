using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [CreateAssetMenu(menuName = "ScriptableObject/Gear", fileName = "Gear_", order = 1)]
    public class Gear : ScriptableObject
    {
        [SerializeField] private string _gearName;
        [SerializeField] private Sprite _gearSpriteIcon;
        [SerializeField] private Recipe _gearRecipe;
        [SerializeField] private List<Stat> _gearStats = new();
        

        #region Public references
        public string GearName { get => _gearName; set => _gearName = value; }
        public Sprite GearSpriteIcon { get => _gearSpriteIcon; set => _gearSpriteIcon = value; }
        public Recipe GearRecipe { get => _gearRecipe; set => _gearRecipe = value; }
        public List<Stat> GearStats { get => _gearStats; }
        #endregion

        #region Editor
        private void OnValidate()
        {
            if (_gearStats.Count == 0) { return; }

            for (int i = 0; i < _gearStats.Count; i++)
            {
                _gearStats[i].SetStatName(_gearStats[i].Type.ToString());
                _gearStats[i].SetStatCurrentValue(_gearStats[i].MaxValue);
            }
        }
        #endregion
    }
}