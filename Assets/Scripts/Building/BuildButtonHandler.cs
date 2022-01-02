using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Building associatedBuilding;
    private GameObject buildingInstance;
    private bool canBeBuilt = false;

    [Header("UI COMPONENTS")]
    public Image detectionFeedbackImage;
    public Image darkerImage;
    public Image disponibilityFeedbackImage;
    public Image buildingIconImage;
    Color color = Color.white;

    private void Start()
    {
        buildingIconImage.sprite = associatedBuilding.buildingIcon;

        color.a = 0f;
        detectionFeedbackImage.color = color;
    }

    public void InstantiateBuildingAtCursorPos()
    {
        buildingInstance = Instantiate(associatedBuilding.buildingPrefab, UtilityClass.GetCursorClickedPosition(LayerMask.NameToLayer("Ground")), associatedBuilding.buildingPrefab.transform.rotation);
    }

    public void CheckIfTheBuildingCanBeBuilt()
    {
        int requirementMet = 0;

        if (associatedBuilding.neededRessourcesToBuild.Count == 0) return;

        foreach (Building.NeededRessourcesDatas thisNeededRessourceData in associatedBuilding.neededRessourcesToBuild)
        {
            if (thisNeededRessourceData.neededRessourceValue == RessourcesHandler.Instance.GetThisRessource(thisNeededRessourceData.ressourceType).CurrentValue)
            {
                Debug.Log(
                    thisNeededRessourceData.neededRessourceValue.ToString("0") + " "
                    + thisNeededRessourceData.ressourceType.ToString() + " / "
                    + RessourcesHandler.Instance.GetThisRessource(thisNeededRessourceData.ressourceType).CurrentValue.ToString() + " "
                    + RessourcesHandler.Instance.GetThisRessource(thisNeededRessourceData.ressourceType).ressourceType.ToString());

                requirementMet++;
            }
        }

        if (requirementMet == associatedBuilding.neededRessourcesToBuild.Count)
        {
            Debug.Log("Requirements are met !");
            requirementMet = associatedBuilding.neededRessourcesToBuild.Count;
            SetThisButtonToDisponibleAppearance();
        }
        else
        {
            SetThisButtonToIndisponibleAppearance();
        }
    }

    private void SetThisButtonToDisponibleAppearance()
    {
        darkerImage.gameObject.SetActive(false);
        disponibilityFeedbackImage.color = Color.green;
        canBeBuilt = true;
    }

    private void SetThisButtonToIndisponibleAppearance()
    {
        darkerImage.gameObject.SetActive(true);
        disponibilityFeedbackImage.color = Color.red;
        canBeBuilt = false;
    }

    #region UI Event - Pointer
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        color.a = 0.15f;
        detectionFeedbackImage.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        color.a = 0f;
        detectionFeedbackImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canBeBuilt)
        {
            //Throw feedback message - need to be built (ui message feedback)
            Debug.Log("CANT BE BUILT, RESSOURCES REQUIREMENTS ARE NOT MET !");
            return;
        }

        Debug.Log("Click on this button");

        CursorController.Instance.SetCursorAppearance(CursorType.Building);
        InstantiateBuildingAtCursorPos();

        DraggingBuilding.Instance.SetBuildingPrefab(buildingInstance, associatedBuilding);
    }
    #endregion
}