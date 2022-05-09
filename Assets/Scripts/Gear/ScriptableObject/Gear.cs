using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum GearType
    {
        Unassigned, Tool
    }

    public enum GearSubType
    {
        Unassigned, Axe, Hammer, Pickaxe, Sickle
    }

    public enum GearTier
    {
        Unassigned, Tier0, Tier1, Tier2, Tier3
    }

    [CreateAssetMenu(menuName = "ScriptableObject/Gear", fileName = "Gear_", order = 1)]
    public class Gear : ScriptableObject
    {
        [SerializeField] private string gearName;
        [SerializeField] private GearType gearType;
        [SerializeField] private GearSubType gearSubType;
        [SerializeField] private GearTier gearTier;
        [SerializeField] private AnimatorOverrideController gearAOC;
        [SerializeField] private Sprite gearSpriteIcon;
        [SerializeField] private Recipe gearRecipe;

        [Header("STATS SECTION")]
        [SerializeField] private List<StatModifier> gearStatModifiers = new();

        #region Public references
        public string GearName { get => gearName; set => gearName = value; }
        public GearType GearType { get => gearType; }
        public GearSubType GearSubType { get => gearSubType; }
        public GearTier GearTier { get => gearTier; private set => gearTier = value; }
        public Sprite GearSpriteIcon { get => gearSpriteIcon; set => gearSpriteIcon = value; }
        public Recipe GearRecipe { get => gearRecipe; set => gearRecipe = value; }
        public List<StatModifier> GearStatModifiers { get => gearStatModifiers; }
        public AnimatorOverrideController GearAOC { get => gearAOC; }
        #endregion

        public void SetGearTier(GearTier newGearTier)
        {
            if (GearTier == newGearTier) 
            {
                Debug.Log("This gear is already of this tier");
                return; 
            }

            GearTier = newGearTier;
        }
    }
}