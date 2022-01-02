using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetenceUIHandler : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void SetIconImage(Sprite newIconSprite)
    {
        iconImage.sprite = newIconSprite;
    }
}