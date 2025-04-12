using System;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Scriptable Objects/Action")]
public class CombatAction : ScriptableObject
{
    public string name;
    [TextArea] 
    public string description;
    public bool isPlus = false;
    public Effect[] effects;
    public Effect[] effectsPlus;

    public void Init(string _name, string _description, bool _isPlus, Effect[] _effects, Effect[] _effectsPlus)
    {
        name = _name;
        description = _description;
        isPlus = _isPlus;
        effects = _effects;
        effectsPlus = _effectsPlus;
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
        }

        public Type type;
        [Tooltip("The amount that this type will do")] [AllowNesting] [HideIf("type", Type.Switch)]
        public int value;
        [ShowIf("type", Type.Damage)] 
        public int[] attackRange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self">Skill user</param>
        /// <param name="target">Skill effector, usually for damage</param>
        /// <returns>If the skill was succesful. Might stop the entire action if unable to complete fully</returns>
        public bool ActivateEffect(Character self)
        {
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
                    CombatManager.manager.MoveCharacter(self, value);
                    break;
                case(Type.Switch):
                    self.SwitchDirection();
                    break;
            }
            return true;
        }

        private void HandleAttack(Character attacker)
        {
            int startPos = attacker.tilePos;
            for (int i = 0; i < attackRange.Length; i++)
            {
                int pos = startPos + (attackRange[i] * (attacker.facingRight ? 1 : -1));
                CombatTile toAttack = CombatManager.manager.GetTile(pos);
                if (toAttack is null) continue;
                toAttack.AttackTile(value);
            }
        }
    }
}
