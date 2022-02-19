using System.Collections;
using UnityEngine;

namespace Khynan_Coding
{
    public class LifeTime : MonoBehaviour
    {
        [Tooltip("Delay after which the vfx is hidden or destroyed.")]
        [SerializeField] private float lifetime = 0.2f;

        [Tooltip("Determines wether if you want to destroy or hide the vfx that has been created.")]
        [SerializeField] private bool needsToBeDestroyed = false;

        private void OnEnable()
        {
            StartCoroutine(DestroyOrHide(lifetime, needsToBeDestroyed));
        }

        private IEnumerator DestroyOrHide(float delay, bool doDestroy)
        {
            yield return new WaitForSeconds(delay);

            if (doDestroy)
            {
                Destroy(gameObject);
            }

            gameObject.SetActive(false);
        }
    }
}