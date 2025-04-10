using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;
    
    [ReadOnly] public int tilePos;

    private CombatManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<CombatManager>();
    }

    private void Update()
    {
        int move = (Input.GetKey(KeyCode.LeftShift)) ? 2 : 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            manager.MoveCharacter(this, -move);   
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            manager.MoveCharacter(this, move);   
        }
    }
}
