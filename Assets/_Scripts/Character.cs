using System;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [SerializeField] private ObjectData objData;
    private Sequence sequence;
    [Space(15)]
    
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private int maxBlock;
    private int blockHealth = 0;
    
    [Space(15)]
    [MinMaxSlider(-3, 3)]
    [SerializeField] private Vector2Int moveRange;
    public Vector2Int MoveRange => moveRange;
    
    [Space(5)]
    [SerializeField] private CombatAction action1;
    [SerializeField] private CombatAction action2;
    [SerializeField] private CombatAction action3;
    [SerializeField] private CombatAction action4;
    [SerializeField] private CombatAction action5;
    private CombatAction[] actions;
    public CombatAction[] Actions => actions;
    
    [Header("Misc")]
    [ReadOnly] public int tilePos;
    [ReadOnly] public bool facingRight = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sequence = DOTween.Sequence();
        
        //TODO: Have health remain between games
        health = maxHealth;
        //TODO: have actions be selectable from a list
        actions = new [] {action1,  action2, action3, action4, action5};
        actions = actions.Where(x => x != null).ToArray();
    }

    public void HealHealth(int value)
    {
        health = Mathf.Min(health + value, maxHealth);
    }

    public void GetBlock(int amount)
    {
        blockHealth = Mathf.Min(blockHealth + amount, maxBlock);
    }

    public void SwitchDirection()
    {
        facingRight = !facingRight;
        int xMod = facingRight ? 1 : -1;
        Vector3 ogScale = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(ogScale.x) * xMod, ogScale.y, ogScale.z); 
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
        spriteRenderer.color = objData.hurtColor;
        TweenColor(Color.white, objData.outOfHurtLerpSpeed);
    }

    private void Die()
    {
        sequence.Kill();
        CombatManager.manager.RemoveCharacter(this);
    }

    public bool IsFacing(Character target)
    {
        return (facingRight && tilePos < target.tilePos) || (!facingRight && tilePos > target.tilePos);
    }

    /// <param name="target">Other character to measure the difference between</param>
    /// <returns>Amount of tiles between target and this character</returns>
    public int GetDistance(Character target)
    {
        return Mathf.Abs(tilePos - target.tilePos) - 1;
    }
    
    private void TweenColor(Color endValue, float lerpSpeed)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(spriteRenderer.DOColor(endValue, lerpSpeed));
    }
}
