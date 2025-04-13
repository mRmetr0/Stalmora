using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Action", menuName = "Scriptable Objects/Action")]
public class CombatAction : ScriptableObject
{
    public string title;
    [TextArea] 
    public string description;
    public bool isPlus = false;
    public Effect[] effects;
    public Effect[] effectsPlus;

    public void SimpleInit(string _name, string _description, Effect _effect)
    {
        title = _name;
        description = _description;
        effects = new [] { _effect };
    }

    public void UseAction(Character self)
    {
        Effect[] currentEffects = isPlus ? effectsPlus : effects;
        foreach (Effect effect in currentEffects)
        {
            if (!effect.ActivateEffect(self)) return; //If action cannot be done fully, end action
        }
    }

    [Serializable]
    public class Effect
    {
        public enum Type
        {
            Damage,
            Heal,
            Block,
            Move,
            Switch,
            Push, 
            Pull
        }

        public Type type;
        public bool uninterruptable;
        [Tooltip("The amount that this type will do")] [AllowNesting] [HideIf("type", Type.Switch)]
        public int value;
        [ShowIf("type", Type.Damage)] 
        public int[] attackRange;

        public Effect(Type _type, int _value, int[] _attackRange = null)
        {
            type = _type;
            value = _value;
            if (_attackRange != null)
                attackRange = _attackRange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self">Skill user</param>
        /// <param name="target">Skill effector, usually for damage</param>
        /// <returns>If the skill was succesful. Might stop the entire action if unable to complete fully</returns>
        public bool ActivateEffect(Character self)
        {
            bool isExecuted = true;
            switch (type)
            {
                case(Type.Damage):
                    HandleAttack(self);
                    break;
                case(Type.Heal):
                    self.HealHealth(value);
                    break;
                case(Type.Block):
                    self.GetBlock(value);
                    break;
                case(Type.Move):
                    isExecuted = TileManager.manager.MoveCharacter(self, value, !uninterruptable);
                    break;
                case(Type.Switch):
                    self.SwitchDirection();
                    break;
                case(Type.Push):
                    HandleMoveTarget(self, value);
                    break;
                case(Type.Pull):    
                    HandleMoveTarget(self, -value);
                    break;
            }
            return isExecuted || uninterruptable || CombatManager.manager.CombatOver;
        }

        private void HandleAttack(Character attacker)
        {
            int startPos = attacker.tilePos;
            for (int i = 0; i < attackRange.Length; i++)
            {
                int pos = startPos + (attackRange[i] * (attacker.facingRight ? 1 : -1));
                CombatTile toAttack = TileManager.manager.GetTile(pos);
                if (toAttack is null) continue;
                toAttack.AttackTile(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="moveValue">Amount the target should move, positive is push, negative is pull</param>
        private void HandleMoveTarget(Character attacker, int moveValue)
        {
            int startPos = attacker.tilePos;
            moveValue *= (attacker.facingRight ? 1 : -1);
            for (int i = 0; i < attackRange.Length; i++)
            {
                int pos = startPos + (attackRange[i] * (attacker.facingRight ? 1 : -1));
                Character toAttack = TileManager.manager.GetTile(pos).occupant;
                if (toAttack is null) continue;
                TileManager.manager.MoveCharacter(toAttack, moveValue, !uninterruptable);
            }
        }
    }
}
