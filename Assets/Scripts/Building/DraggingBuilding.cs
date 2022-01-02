using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingBuilding : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Building buildingScript;
    public LayerMask groundLayer;

    #region Singleton
    public static DraggingBuilding Instance;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    void Update()
    {
        if (buildingPrefab) UpdateDraggedPosition();

        if (UtilityClass.IsKeyPressed(KeyCode.Escape) || UtilityClass.RightClickIsPressed())
        {
            CancelBuilding();
        }
        else if (UtilityClass.LeftClickIsPressed())
        {
            ValidateBuilding();
        }
    }

    public void SetBuildingPrefab(GameObject prefab, Building buildingScript)
    {
        buildingPrefab = prefab;
        this.buildingScript = buildingScript;
    }

    private void UpdateDraggedPosition()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            buildingPrefab.transform.position = new Vector3 (hit.point.x, 0, hit.point.z);
        }
    }

    private void ValidateBuilding()
    {
        if (buildingPrefab && CheckIfBuildingCanBeBuiltHere())
        {
            Instantiate(buildingPrefab, buildingPrefab.transform.position, buildingPrefab.transform.rotation);
            RemoveRessourcesFromPlayerAfterBuilding();
            CancelBuilding();
        }
    }

    private void RemoveRessourcesFromPlayerAfterBuilding()
    {
        foreach (Building.NeededRessourcesDatas thisNeededRessourceData in buildingScript.neededRessourcesToBuild)
        {
            if (thisNeededRessourceData.ressourceType == RessourcesHandler.Instance.GetThisRessource(thisNeededRessourceData.ressourceType).ressourceType)
            {
                RessourcesHandler.Instance.GetThisRessource(thisNeededRessourceData.ressourceType).RemoveToCurrentValue(thisNeededRessourceData.neededRessourceValue);
            }
        }
    }

    private void CancelBuilding()
    {
        CursorController.Instance.SetCursorAppearance(CursorType.Default);
        Destroy(buildingPrefab);
        buildingPrefab = null;
    }

    //NEED TO BE BUILT TOO
    private bool CheckIfBuildingCanBeBuiltHere()
    {
        return true;
    }
}