using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrameUIHandler : MonoBehaviour
{
    //Debug set to private
    public Unit unitRef;
    public Image iconImage;

    public void SetUnitRef(Unit newUnit)
    {
        unitRef = newUnit;
        SetCharacterFrameIcon(newUnit.icon);
    }

    public void SetCharacterFrameIcon(Sprite newIconSprite)
    {
        iconImage.sprite = newIconSprite;
    }

}