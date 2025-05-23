using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatTile : MonoBehaviour
{
    [SerializeField] private UIData uiData;
    [SerializeField] private ObjectData objData;
    private Sequence sequence;
    private SpriteRenderer spriteRenderer;

    [NonSerialized] public Character occupant = null;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = uiData.defaultColor;
        sequence = DOTween.Sequence();
    }

    private void OnMouseEnter()
    {
        TweenColor(uiData.hoverColor, uiData.hoverLerpSpeed);
    }

    private void OnMouseExit()
    {
        TweenColor(uiData.defaultColor, uiData.hoverLerpSpeed);
    }
    
    public Vector2 GetCombatTilePos()
    {
        return ((RectTransform)transform).position;
    }

    public void AttackTile(int damage)
    {
        spriteRenderer.color = objData.hurtColor;
        TweenColor(uiData.defaultColor, objData.outOfHurtLerpSpeed);
        if (occupant is not null)
            occupant.TakeDamage(damage);
    }

    public bool Occupied()
    {
        return occupant is not null;
    }

    private void TweenColor(Color endValue, float lerpSpeed)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(spriteRenderer.DOColor(endValue, lerpSpeed));
    }
}
