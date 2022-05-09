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

        public void SetCompartment(EarnData earnData)
        {
            Debug.Log("Set Compartment");

            earnText.SetText(/*"+ " +*/ earnData.Name + " x " + earnData.Value.ToString());
        }
    }
}