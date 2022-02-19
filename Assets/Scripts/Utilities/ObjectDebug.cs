using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    public class ObjectDebug : MonoBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField] private bool isDebuggingAPosition = false;
        [SerializeField] private float debugRadiusValue = 1.0f;
        [SerializeField] private Color debugDrawColor = Color.red;
        [SerializeField] private Vector3 positionOffset = Vector3.zero;

        [Header("COLLIDER SETTINGS")]
        [SerializeField] private Collider colliderToDebug;
        [SerializeField] private bool isDebuggingAColliderRadius = false;
        [SerializeField] private bool isASphereCollider = false;

        private void OnDrawGizmos()
        {
            Gizmos.color = debugDrawColor;

            DebugPosition();
            DebugColliderRadius();
        }

        private void DebugPosition()
        {
            if (!isDebuggingAPosition) { return; }

            Gizmos.DrawWireCube(
                    new Vector3(transform.position.x + positionOffset.x, transform.position.y + positionOffset.y, transform.position.z + positionOffset.z),
                    new Vector3(debugRadiusValue, debugRadiusValue, debugRadiusValue));
        }

        private void DebugColliderRadius()
        {
            if (!isDebuggingAColliderRadius || !colliderToDebug) { return; }

            switch (isASphereCollider)
            {
                case true:
                    SphereCollider sphereCollider = (SphereCollider)colliderToDebug;

                    Gizmos.DrawWireSphere(new Vector3(
                            transform.position.x + positionOffset.x,
                            transform.position.y + positionOffset.y,
                            transform.position.z + positionOffset.z),
                        sphereCollider.radius);
                    break; 
                case false:
                    Gizmos.DrawWireCube(new Vector3(
                        transform.position.x + positionOffset.x,
                        transform.position.y + positionOffset.y,
                        transform.position.z + positionOffset.z),
                    new Vector3(colliderToDebug.bounds.size.x, colliderToDebug.bounds.size.y, colliderToDebug.bounds.size.z));
                    break;
            }
        }
    }
}