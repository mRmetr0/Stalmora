using System;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Scriptable Objects/Action")]
public class Action : ScriptableObject
{
    public string name;
    [TextArea] 
    public string description;
    public Effect[] effects;
    public Effect[] effectsPlus;

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

        public void ActivateEffect(Character self, Character target)
        {
            switch (type)
            {
                case(Type.Damage):
                    break;
            }
        }
    }
}
