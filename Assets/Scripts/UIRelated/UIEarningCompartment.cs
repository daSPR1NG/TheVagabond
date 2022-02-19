using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public class UIEarningCompartment : UICompartment
    {
        [Header("SETUP")]
        [SerializeField] private TMP_Text earnText;
        [SerializeField] private Image iconImage;

        public void SetCompartment(Resource resource, Gear gear)
        {
            Debug.Log("Set Compartment");

            if (resource != null)
            {
                earnText.SetText(/*"+ " +*/ resource.ResourceName + " x " + resource.CurrentValue.ToString());

                switch (resource.ResourceType)
                {
                    case ResourceType.Log:
                        break;
                    case ResourceType.Stone:
                        break;
                    case ResourceType.Food:
                        break;
                    case ResourceType.Minerals:
                        break;
                }

                //iconImage.sprite = resource.ResourceSpriteIcon;

                Debug.Log("Its a Resource");
            }

            if (gear)
            {
                earnText.SetText(/*"+ " +*/ gear.GearName);
                iconImage.sprite = gear.GearSpriteIcon;

                Debug.Log("Its a Gear");
            }
        }
    }
}