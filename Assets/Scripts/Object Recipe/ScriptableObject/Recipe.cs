using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [CreateAssetMenu(menuName = "ScriptableObject/Recipe", fileName = "Recipe_", order = 0)]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private string _recipeName;
        [SerializeField] private Sprite _recipeSpriteIcon;
        [SerializeField] private List<NeededResourcesDatas> _neededResources = new();

        #region Public references
        public string RecipeName { get => _recipeName; set => _recipeName = value; }
        public Sprite RecipeSpriteIcon { get => _recipeSpriteIcon; set => _recipeSpriteIcon = value; }
        public List<NeededResourcesDatas> NeededRessources { get => _neededResources; private set => _neededResources = value; }
        #endregion

        [System.Serializable]
        public class NeededResourcesDatas
        {
            [SerializeField] private string _resourceName = "[TYPE HERE]";
            [SerializeField] private ResourceType _resourceType = ResourceType.Unassigned;
            [SerializeField] private float _neededResourceValue = 0f;

            #region Public references
            public string ResourceName { get => _resourceName; private set => _resourceName = value; }
            public ResourceType ResourceType { get => _resourceType; }
            public float NeededResourceValue { get => _neededResourceValue; private set => _neededResourceValue = value; }
            #endregion

            public void SetResourceName(string stringValue)
            {
                if (ResourceName == stringValue) { return; }

                ResourceName = stringValue;
            }

            public void SetNeededResourceValue(float value)
            {
                if (NeededResourceValue == value) { return; }

                NeededResourceValue = value;
            }
        }

        public NeededResourcesDatas GetThisTypeOfResourceData(ResourceType type)
        {
            if (_neededResources.Count == 0) 
            {
                Debug.LogError("The list of needed resources is empty for this recipe : " + RecipeName);
                return null; 
            }

            for (int i = 0; i < _neededResources.Count; i++)
            {
                if (_neededResources[i].ResourceType == ResourceType.Unassigned) { continue; }

                if (_neededResources[i].ResourceType == type)
                {
                    return _neededResources[i];
                }

                Debug.LogError("The type of resource (" + type + ") cannot be found : " + _neededResources[i].ResourceType);
            }
            
            return null;
        }

        #region Editor
        private void OnValidate()
        {
            Editor_SetRessourcesDatas();
        }

        private void Editor_SetRessourcesDatas()
        {
            if (NeededRessources.Count == 0) { return; }

            for (int i = 0; i < NeededRessources.Count; i++)
            {
                NeededRessources[i].SetResourceName(NeededRessources[i].ResourceType.ToString());
            }
        }
        #endregion
    }
}