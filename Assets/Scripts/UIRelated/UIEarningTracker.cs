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
            UIManager.Instance.PlayerInventory.OnAddingGear += SendGearEarningFeedback;
        }

        private void OnDisable()
        {
            UIManager.Instance.ResourcesManager.OnEarningResources -= SendResourceEarningFeedback;
            UIManager.Instance.PlayerInventory.OnAddingGear -= SendGearEarningFeedback;
        }

        private void SendResourceEarningFeedback(Resource resource)
        {
            Debug.Log("Resource Earn feedback");

            CreateEarningCompartment(resource, null);
        }
        
        private void SendGearEarningFeedback(Gear gear)
        {
            Debug.Log("Gear Earn feedback");

            CreateEarningCompartment(null, gear);
        }

        private void CreateEarningCompartment(Resource resource, Gear gear)
        {
            if (!earningCompartmentPrefab) 
            { 
                Debug.LogError("Compartment is not set", transform);
                return;
            }

            GameObject instance = Instantiate(earningCompartmentPrefab, earningCompartmentParent);

            UIEarningCompartment UIEarningCompartment = instance.GetComponent<UIEarningCompartment>();

            UIEarningCompartment.SetCompartment(resource, gear);
        }
    }
}