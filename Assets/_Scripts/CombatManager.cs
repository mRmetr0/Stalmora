using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public enum EnemyBehaviour
    {
        Random,
        Controllable,
        Agressive, //TODO: create
        Passive //TODO: create
    }

    public static CombatManager manager;
    
    [SerializeField] private Character player; //TODO: load in player from overworld data
    [SerializeField] private Character enemy; //TODO: load in enemy from overworld data
    [SerializeField] private EnemyBehaviour behaviour;

    [SerializeField] private CombatUI ui;
    private bool isPlayerTurn = true;
    private int actions = 2;

    private void Awake()
    {
        //Handle singleton
        if (manager != null)
        {
            Debug.LogError("MORE THEN ONE COMBAT MANAGER!!!");
            Destroy(gameObject);
            return;
        }
        manager = this;

        if (ui is null) ui = FindAnyObjectByType<CombatUI>();
    }

    private void Start()
    {
        StartPlayerTurn();
    }

    private void OnDestroy()
    {
        if (manager == this) manager = null;
    }

    private void StartPlayerTurn()
    {
        ui.gameObject.SetActive(true);
        ui.SetUI(player);
    }

    public void StartEnemyTurn()
    {
        if (behaviour == EnemyBehaviour.Controllable)
        {
            ui.gameObject.SetActive(true);
            ui.SetUI(enemy);
        }
    }

    public void EndAction()
    {
        //Count actions
        actions--;
        if (actions <= 0)
        {
            Debug.Log("END OF TURN");
            isPlayerTurn = !isPlayerTurn;
            actions = 2;
        }
        //Set turn
        if (isPlayerTurn)
        {
            StartPlayerTurn();
        }
        else
        {
            StartEnemyTurn();
        }
    }
}
