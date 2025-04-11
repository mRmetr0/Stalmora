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
            Move
        }

        public Type type;
        [Tooltip("The amount that this type will do")]
        public int value;
        [ShowIf("type", Type.Damage)] [AllowNesting] [Tooltip("Distance of tiles, relative to the caster")]
        public int[] range;

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
                    break;
                case(Type.Heal):
                    self.HealHealth(value);
                    break;
                case(Type.Block):
                    break;
                case(Type.Move):
                    CombatManager.manager.MoveCharacter(self, value);
                    break;
            }

            return true;
        }
    }
}
