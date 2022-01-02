using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatHUDManager : MonoBehaviour
{
    public TextMeshProUGUI tourNumberText;

    #region Singleton
    public static CombatHUDManager Instance;

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

    void Start() => SetTurnNumberText();

    private void OnEnable()
    {
        TurnOrderManager.OnTurnEnded += SetTurnNumberText;
    }

    private void OnDisable()
    {
        TurnOrderManager.OnTurnEnded -= SetTurnNumberText;
    }

    public void SetTurnNumberText()
    {
        tourNumberText.text = TurnOrderManager.Instance.GetCurrentTurnValue().ToString();
    }
}