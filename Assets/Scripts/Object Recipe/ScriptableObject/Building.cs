using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Building", fileName = "Building_SO_", order = 0)]
    public class Building : ScriptableObject
    {
        public string buildingName;
        public Sprite buildingIcon;
        public GameObject buildingPrefab;
        public List<NeededRessourcesDatas> neededRessourcesToBuild;

        [System.Serializable]
        public class NeededRessourcesDatas
        {
            public string ressourceName = "[TYPE HERE]";
            public RessourceType ressourceType = RessourceType.Unassigned;
            public float neededRessourceValue = 0f;
        }

        #region Editor
        private void OnValidate()
        {
            Editor_SetRessourcesDatas();
        }

        private void Editor_SetRessourcesDatas()
        {
            for (int i = 0; i < neededRessourcesToBuild.Count; i++)
            {
                neededRessourcesToBuild[i].ressourceName = neededRessourcesToBuild[i].ressourceType.ToString();
            }
        }
        #endregion
    }
}