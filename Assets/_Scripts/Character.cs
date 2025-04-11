using System;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    [SerializeField] private UIUnivarsalData uiData;
    [Space(15)]
    
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private int maxBlock;
    private int blockHealth = 0;
    
    [ReadOnly] public int tilePos;
    [ReadOnly] public bool facingRight = true;

    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        
        //TODO: Have health remain between games
        health = maxHealth;
    }

    public void HealHealth(int value)
    {
        health = Mathf.Min(health + value, maxHealth);
    }

    public void GetBlock(int amount)
    {
        blockHealth = Mathf.Min(blockHealth + amount, maxBlock);
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
        if (health <= 0)
        {
            Die();
            return;
        }
        renderer.color = uiData.hurtColor;
        renderer.DOColor(Color.white, uiData.outOfHurtLerpSpeed);
    }

    private void Die()
    {
        //TODO: remove character from turn order.
        Debug.Log($"{name} HAS DIED");
    }
}
