using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class CombatManager : MonoBehaviour
{
    public static CombatManager manager;
    public static Random random = new();
    
    [SerializeField] private Character player; //TODO: load in player from overworld data
    [SerializeField] private Character enemy; //TODO: load in enemy from overworld data
    [SerializeField] private EnemyBehaviour.BehaviourType behaviourType;
    private EnemyBehaviour behaviour;

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
    }

    private void Start()
    {
        if (ui is null) ui = FindAnyObjectByType<CombatUI>();
        behaviour = new EnemyBehaviour(behaviourType, enemy);
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

    public IEnumerator StartEnemyTurn()
    {
        ui.gameObject.SetActive(false);
        yield return new WaitForSeconds(.4f);
        behaviour.GetEnemyAction().UseAction(enemy);
        yield return new WaitForSeconds(.4f);
        EndAction();
    }

    public void EndAction()
    {
        //Count actions
        actions--;
        if (actions <= 0)
        {
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
            StartCoroutine(StartEnemyTurn());
        }
    }

    public class EnemyBehaviour
    {
        public enum BehaviourType
        {
            Random,
            Controllable, //TODO: Create
            //TODO: Implement smart enemy behaviour
            Agressive, //TODO: create
            Passive //TODO: create
        }

        private BehaviourType type;
        private Character character;
        private CombatAction[] actions;

        public EnemyBehaviour(BehaviourType _type, Character _character)
        {
            type = _type;
            character = _character;
            actions = GetAllActions();
        }

        public CombatAction GetEnemyAction()
        {
            switch (type)
            {
                default:
                    return GetRandomAction();
            }
        }

        private CombatAction GetRandomAction()
        {
            int randomN = random.Next(actions.Length);
            return actions[randomN];
        }

        private CombatAction[] GetAllActions()
        {
            List<CombatAction> list = new();
            //Add movement
            // for (int i = target.MoveRange.x; i <= target.MoveRange.y; i++)
            // {
            //     if (i == 0) continue;
            //     CombatAction action = ScriptableObject.CreateInstance<CombatAction>();
            //     action.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Move, -i));
            //     list.Append(action);
            // }
            
            CombatAction move1 = ScriptableObject.CreateInstance<CombatAction>();
            move1.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Move, 1));
            list.Add(move1);
            CombatAction move2 = ScriptableObject.CreateInstance<CombatAction>();
            move2.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Move, -1));
            list.Add(move2);
            CombatAction swap = ScriptableObject.CreateInstance<CombatAction>();
            swap.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Switch, 0));
            list.Add(swap);

            list.AddRange(character.Actions);

            return list.ToArray();
        }
    }
}
