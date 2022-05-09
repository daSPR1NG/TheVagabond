using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Khynan_Coding
{
    public static class Helper
    {
        #region Debug
        public static void DebugMessage(string messageContent, Transform source = null)
        {
            Debug.Log(messageContent, source);
        }

        public static void ThrowErrorMessage(string messageContent, Transform source = null)
        {
            Debug.Log(messageContent, source);
        }
        #endregion

        #region Main Camera
        public static Camera GetMainCamera()
        {
            return Camera.main;
        }

        public static CinemachineBrain GetCinemachineBrain()
        {
            return GetMainCamera().GetComponent<CinemachineBrain>();
        }

        public static ICinemachineCamera GetActiveVirtualCamera()
        {
            return GetCinemachineBrain().ActiveVirtualCamera;
        }

        public static Vector3 GetMainCameraForwardDirection(float yValue)
        {
            Vector3 cameraForwardDirection = GetMainCamera().transform.forward;
            cameraForwardDirection.y = yValue;

            return cameraForwardDirection;
        }

        public static Vector3 GetMainCameraRightDirection(float yValue)
        {
            Vector3 cameraRightDirection = GetMainCamera().transform.right;
            cameraRightDirection.y = yValue;

            return cameraRightDirection;
        }
        #endregion

        #region Left and right click pressure check
        public static bool IsLeftClickPressed()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            
            return false;
        }

        public static bool IsRightClickPressed()
        {
            if (Input.GetMouseButtonDown(1))
            {
                return true;
            }
            
            return false;
        }
        #endregion

        #region Left and right key hold check
        public static bool IsLeftClickHeld()
        {
            if (Input.GetMouseButton(0))
            {
                return true;
            }
            
            return false;
        }

        public static bool IsRightClickHeld()
        {
            if (Input.GetMouseButton(1))
            {
                return true;
            }
            
            return false;
        }
        #endregion

        #region Left and right click on UI elements pressure check
        public static bool IsLeftClickPressedOnUIElement(PointerEventData requiredEventData)
        {
            if (requiredEventData.button == PointerEventData.InputButton.Left)
            {
                return true;
            }
            
            return false;
        }

        public static bool IsRightClickPressedOnUIElement(PointerEventData requiredEventData)
        {
            if (requiredEventData.button == PointerEventData.InputButton.Right)
            {
                return true;
            }
            
            return false;
        }
        #endregion

        #region Inputs pressure and hold check
        public static bool IsKeyPressed(KeyCode key)
        {
            if (Input.GetKeyDown(key)) return true;
            
            return false;
        }

        public static bool IsKeyUnpressed(KeyCode key)
        {
            if (Input.GetKeyUp(key)) return true;
            
            return false;
        }

        public static bool IsKeyMaintained(KeyCode key)
        {
            if (Input.GetKey(key)) return true;
            
            return false;
        }

        public static bool IsKeyPressedOrMaintained(KeyCode key)
        {
            if (Input.GetKeyDown(key) || Input.GetKey(key)) return true;

            return false;
        }
        #endregion

        #region Cursor
        public static Vector3 GetCursorClickPosition(LayerMask layerMask)
        {
            Ray rayFromMainCameraToCursorPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 hitPointPos = Vector3.zero;

            if (Physics.Raycast(rayFromMainCameraToCursorPosition, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                hitPointPos = hit.point;
            }

            Helper.DebugMessage("Cursor clicked position : " + hitPointPos);

            return hitPointPos;
        }

        public static void SetCursorLockMode(CursorLockMode lockMode)
        {
            Cursor.lockState = lockMode;
        }
        #endregion

        #region NavMeshAgent
        public static void SetAgentDestination(NavMeshAgent navMeshAgent, Vector3 target)
        {
            navMeshAgent.SetDestination(target);
        }

        public static void SetAgentStoppingDistance(NavMeshAgent navMeshAgent, float newStoppingDistanceValue)
        {
            navMeshAgent.stoppingDistance = newStoppingDistanceValue;
        }

        public static void ResetAgentDestination(NavMeshAgent navMeshAgent)
        {
            if (navMeshAgent.hasPath)
            {
                navMeshAgent.isStopped = true;

                navMeshAgent.path.ClearCorners();
                navMeshAgent.ResetPath();
            }

            navMeshAgent.isStopped = false;
        }
        #endregion

        #region String - float value rounded to seconds or minutes
        public static string GetStringOfValueRoundedToMinutes(float value)
        {
            string minutes = Mathf.Floor(value / 60).ToString("0.00");

            return minutes;
        }

        public static string GetStringOfValueRoundedToSeconds(float value)
        {
            string seconds = Mathf.Floor(value % 60).ToString("0.00");

            return seconds;
        }

        public static string GetStringOfValueRoundedToMinutesAndSeconds(float value)
        {
            string minutes = Mathf.Floor(value / 60).ToString("00");
            string seconds = Mathf.Floor(value % 60).ToString("00");

            return (minutes + " : " + seconds);
        }
        #endregion

        #region UI
        public static void SetImageSprite(Image image, Sprite sprite)
        {
            if (image.sprite == sprite) { return; }

            image.sprite = sprite;
        }

        public static void ActivateUIWindow(GameObject window)
        {
            if (window.activeInHierarchy) { return; }

            window.SetActive(true);
        }

        public static void DeactivateUIWindow(GameObject window)
        {
            if (!window.activeInHierarchy) { return; }

            window.SetActive(false);
        }
        #endregion

        public static T GetComponentInChildren<T>(Transform parent) where T : Component
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).GetComponent<T>() == null) { continue; }

                return parent.GetChild(i).GetComponent<T>();
            }

            return default;
        }
    }
}