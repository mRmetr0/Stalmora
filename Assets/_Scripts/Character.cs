using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;
    private int blockHealth = 0;
    
    [ReadOnly] public int tilePos;

    private void Awake()
    {
        //TODO: Have health remain between games
        health = maxHealth;
    }

    public void HealHealth(int value)
    {
        health = Mathf.Min(health + value, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        //First have damage go through block:
        if (blockHealth > 0)
        {
            int afterBlock = damage - blockHealth;
            blockHealth = Mathf.Max(blockHealth - damage, 0);
            if (afterBlock <= 0) return; //If block stops all damage, dont continue
            damage = afterBlock;
        }
        //Deal damage to health if it breaks through block
        health -= damage;
        if (health > 0) return;
        Die();
    }

    private void Die()
    {
        //TODO: remove character from turn order.
        Debug.Log($"{name} HAS DIED");
    }
}
