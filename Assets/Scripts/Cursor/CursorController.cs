using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum CursorType
    {
        Default, Enemy, Ressource, Building,
    }

    [DisallowMultipleComponent]
    public class CursorController : MonoBehaviour
    {
        [Header("APPEARANCE SETTINGS")]
        [SerializeField] private List<CursorAppearance> cursorAppearances = new();

        [SerializeField] private float updateRate = 0.1f;
        private int currentFrame = 0;
        private float frameTimer = 0f;
        [SerializeField] private bool cursorIsConfined = false;
        [SerializeField] private bool cursorIsLocked = false;

        #region Singleton
        public static CursorController Instance;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        #endregion

        [System.Serializable]
        public class CursorAppearance
        {
            [Header("SETUP")]
            [SerializeField] private string appearanceName;
            [SerializeField] private CursorType cursorType = CursorType.Default;
            [SerializeField] private bool isSelected = false;
            [SerializeField] private Vector2 offsetPosition;
            [SerializeField] private float updateDelayValue = 0.15f;

            [Space]
            [Header("VISUAL SETTINGS")]
            [SerializeField] private List<Texture2D> cursorTextures = new();

            #region Public references
            public string AppearanceName { get => appearanceName; set => appearanceName = value; }
            public CursorType CursorType { get => cursorType; }
            public bool IsSelected { get => isSelected; set => isSelected = value; }
            public List<Texture2D> CursorTextures { get => cursorTextures; }
            public Vector2 OffsetPosition { get => offsetPosition; }
            public float UpdateDelayValue { get => updateDelayValue; }
            public int frameCount => CursorTextures.Count;
            #endregion
        }


        private void Start()
        {
            SetCursorAppearance(CursorType.Default);
        }

        private void LateUpdate()
        {
            UpdateCursorAppearance();
        }

        public void SetCursorAppearance(CursorType cursorType)
        {
            CursorAppearance currentCursorAppearance = null;

            for (int i = 0; i < cursorAppearances.Count; i++)
            {
                cursorAppearances[i].IsSelected = false;

                if (cursorAppearances[i].CursorType == cursorType)
                {
                    currentCursorAppearance = cursorAppearances[i];
                }
            }

            currentCursorAppearance.IsSelected = true;

            Cursor.SetCursor(currentCursorAppearance.CursorTextures[0], currentCursorAppearance.OffsetPosition, CursorMode.Auto);
            currentFrame = 0;
        }

        private void UpdateCursorAppearance()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                SetCursorAppearance(CursorType.Default);
                return;
            }

            if (GetCurrentCursorAppearance().CursorTextures.Count <= 1) { return; }

            frameTimer -= Time.deltaTime;

            if (frameTimer <= 0f)
            {
                frameTimer += updateRate;
                currentFrame = (currentFrame + 1) % GetCurrentCursorAppearance().frameCount;
                Cursor.SetCursor(GetCurrentCursorAppearance().CursorTextures[currentFrame], GetCurrentCursorAppearance().OffsetPosition, CursorMode.Auto);
            }
        }

        private CursorAppearance GetCurrentCursorAppearance()
        {
            if (cursorAppearances.Count == 0) 
            {
                Debug.LogError("No cursor appearances have been set, list count is zero.");
                return null; 
            }

            for (int i = 0; i < cursorAppearances.Count; i++)
            {
                if (cursorAppearances[i].IsSelected)
                {
                    return cursorAppearances[i];
                }
            }

            return null;
        }

        #region Editor
        private void OnValidate()
        {
            if (cursorAppearances.Count == 0) { return; }

            for (int i = 0; i < cursorAppearances.Count; i++)
            {
                cursorAppearances[i].AppearanceName = cursorAppearances[i].CursorType.ToString();
            }
        }
        #endregion
    }
}