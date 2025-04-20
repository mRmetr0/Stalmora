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
    private bool combatOver = false;

    public bool CombatOver => combatOver;
    
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
        behaviour = new EnemyBehaviour(enemy, player, behaviourType);
        StartPlayerTurn();
    }

    private void OnDestroy()
    {
        if (manager == this) manager = null;
    }

    private void StartPlayerTurn()
    {
        ui.SetUI(player);
    }

    public IEnumerator StartEnemyTurn()
    {
        yield return HandleAction(enemy, behaviour.GetEnemyAction());
    }

    public IEnumerator HandleAction(Character actor, CombatAction action)
    {
        ui.SetUI(null);
        yield return StartCoroutine(action.UseAction(actor));
        yield return new WaitForSeconds(0.2f);
        EndAction();
    }

    public void EndAction()
    {
        if (combatOver) return;
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

    public void RemoveCharacter(Character toRemove)
    {
        ui.SetUI(null);
        ui.SetCombatEndUI(toRemove == enemy);

        TileManager.manager.GetTile(enemy.tilePos).occupant = null;
        Destroy(toRemove.gameObject);
        combatOver = true;
    }

    public class EnemyBehaviour
    {
        public enum BehaviourType
        {
            Random,
            Controllable, //TODO: Create
            //TODO: Implement smart enemy behaviour
            Agressive, 
            Passive //TODO: create
        }

        private BehaviourType type;
        private Character enemy;
        private Character target;
        private CombatAction[] actions;
        
        //General movement
        private CombatAction swap;
        private CombatAction moveR;
        private CombatAction moveL;


        public EnemyBehaviour(Character _enemy, Character _target, BehaviourType _type)
        {
            type = _type;
            enemy = _enemy;
            target = _target;
            SetUpGeneralActions();
            actions = GetAllActions();
        }

        public CombatAction GetEnemyAction()
        {
            switch (type)
            {
                case(BehaviourType.Agressive):
                    Debug.Log("AGRESSIVE");
                    return GetAgressiveAction();
                case(BehaviourType.Random):
                default:
                    Debug.Log("RANDOM");
                    return GetRandomAction();
            }
        }

        private CombatAction GetAgressiveAction() //TODO: make aggression based off of best move
        {
            //Turn to target if facing away
            if (!enemy.IsFacing(target)) return swap;
            
            int dist = enemy.GetDistance(target);
            //Attack if close enough
            if (dist == 0) return enemy.Actions[0];
            //Move closer if not
            return enemy.facingRight ? moveR : moveL;
        }

        private CombatAction GetRandomAction()
        {
            int randomN = random.Next(actions.Length);
            return actions[randomN];
        }

        private void SetUpGeneralActions()
        {
            swap = ScriptableObject.CreateInstance<CombatAction>();
            swap.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Switch, 0));
            moveR = ScriptableObject.CreateInstance<CombatAction>();
            moveR.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Move, 1));
            moveL = ScriptableObject.CreateInstance<CombatAction>();
            moveL.SimpleInit("", "", new CombatAction.Effect(CombatAction.Effect.Type.Move, -1));
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
            
            list.Add(swap);
            list.Add(moveR);
            list.Add(moveL);

            list.AddRange(enemy.Actions);

            return list.ToArray();
        }
    }
}
