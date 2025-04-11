using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;
    
    [ReadOnly] public int tilePos;
}
