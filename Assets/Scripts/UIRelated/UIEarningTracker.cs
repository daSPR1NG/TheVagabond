using UnityEngine;

namespace Khynan_Coding
{
    public class UIEarningTracker : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private Transform earningCompartmentParent;
        [SerializeField] private GameObject earningCompartmentPrefab;

        private void OnEnable()
        {
            UIManager.Instance.ResourcesManager.OnEarningResources += SendResourceEarningFeedback;
        }

        private void OnDisable()
        {
            UIManager.Instance.ResourcesManager.OnEarningResources -= SendResourceEarningFeedback;
        }

        private void SendResourceEarningFeedback(EarnData earnData)
        {
            Debug.Log("Resource Earn feedback");

            CreateEarningCompartment(earnData);
        }

        private void CreateEarningCompartment(EarnData earnData)
        {
            if (!earningCompartmentPrefab) 
            { 
                Debug.LogError("Compartment is not set", transform);
                return;
            }

            GameObject instance = Instantiate(earningCompartmentPrefab, earningCompartmentParent);

            UIEarningCompartment UIEarningCompartment = instance.GetComponent<UIEarningCompartment>();

            UIEarningCompartment.SetCompartment(earnData);
        }
    }
}