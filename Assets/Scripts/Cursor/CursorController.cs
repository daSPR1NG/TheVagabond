using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum CursorType
    {
        Default,
        Enemy,
        Ressource,
        Building,
    }

    [System.Serializable]
    public class CursorAppearance
    {
        [Header("GENERAL SETTINGS")]
        public string appearanceName;
        public CursorType cursorType = CursorType.Default;
        public bool isSelected = false;

        [Space]
        [Header("VISUAL SETTINGS")]
        public List<Texture2D> cursorTextures;

        [Space]
        [Header("POSITION SETTINGS")]
        public Vector2 offsetPosition;
        public float updateDelayValue = 0.15f;

        public int frameCount => cursorTextures.Count;
    }

    public class CursorController : MonoBehaviour
    {
        [Header("APPEARANCE SETTINGS")]
        public List<CursorAppearance> cursorAppearances;

        [SerializeField] private float frameRate = 0.1f;
        private int currentFrame = 0;
        private float frameTimer = 0f;
        public bool cursorIsConfined = false;
        public bool cursorIsLocked = false;

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
                cursorAppearances[i].isSelected = false;

                if (cursorAppearances[i].cursorType == cursorType)
                {
                    currentCursorAppearance = cursorAppearances[i];
                }
            }

            currentCursorAppearance.isSelected = true;

            Cursor.SetCursor(currentCursorAppearance.cursorTextures[0], currentCursorAppearance.offsetPosition, CursorMode.Auto);
            currentFrame = 0;
        }

        private void UpdateCursorAppearance()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                SetCursorAppearance(CursorType.Default);
                return;
            }

            if (GetCurrentCursorAppearance().cursorTextures.Count <= 1) { return; }

            frameTimer -= Time.deltaTime;

            if (frameTimer <= 0f)
            {
                frameTimer += frameRate;
                currentFrame = (currentFrame + 1) % GetCurrentCursorAppearance().frameCount;
                Cursor.SetCursor(GetCurrentCursorAppearance().cursorTextures[currentFrame], GetCurrentCursorAppearance().offsetPosition, CursorMode.Auto);
            }
        }

        private CursorAppearance GetCurrentCursorAppearance()
        {
            for (int i = 0; i < cursorAppearances.Count; i++)
            {
                if (cursorAppearances[i].isSelected)
                {
                    return cursorAppearances[i];
                }
            }

            return null;
        }

        #region Editor
        private void OnValidate()
        {
            for (int i = 0; i < cursorAppearances.Count; i++)
            {
                cursorAppearances[i].appearanceName = cursorAppearances[i].cursorType.ToString();
            }
        }
        #endregion
    }
}