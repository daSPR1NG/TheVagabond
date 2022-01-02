using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public delegate void ArenaStatusHandler();
    public static event ArenaStatusHandler OnEnteringArena;

    public List<Arena> arenas;

    #region Singleton
    public static ArenaManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [Serializable]
    public class Arena
    {
        public string arenaName = "[TYPE HERE]";
        [SerializeField] private int arenaID = 0;
        [SerializeField] private GameObject container;
        public List<Transform> playerUnitsPos;
        public List<Transform> enemiesUnitsPos;

        public String GetArenaName()
        {
            return arenaName;
        }

        public int GetArenaID()
        {
            return arenaID;
        }

        public GameObject GetArenaContainer()
        {
            return container;
        }

        public bool ArenaIsActive()
        {
            if (GetArenaContainer().activeInHierarchy)
            {
                return true;
            }
            return false;
        }
    }

    private void OnEnable()
    {
        ActivateArenaByID(0);
    }

    //DEBUG
    public void Update()
    {
        if (UtilityClass.IsKeyPressed(KeyCode.A))
        {
            ActivateArenaByID(0);
        }
    }

    public Arena GetActiveArena()
    {
        for (int i = 0; i < arenas.Count; i++)
        {
            if (arenas[ i ].ArenaIsActive())
            {
                return arenas [ i ];
            }
        }

        return null;
    }

    #region Display | Hide Arena
    public void ActivateArenaByID(int arenaID)
    {
        for (int i = 0; i < arenas.Count; i++)
        {
            if (arenas [ i ].GetArenaID() == arenaID)
            {
                arenas [ i ].GetArenaContainer().SetActive(true);
                OnEnteringArena?.Invoke();
                Debug.Log("Entering : " + arenas [ i ].arenaName);
            }
        }
    }

    public void HideActiveArena()
    {
        for (int i = 0; i < arenas.Count; i++)
        {
            if (arenas [ i ].ArenaIsActive())
            {
                arenas [ i ].GetArenaContainer().SetActive(false);
                Debug.Log("Active arena was : " + arenas[ i ].GetArenaName() + " .");
            }
        }
    }
    #endregion
}