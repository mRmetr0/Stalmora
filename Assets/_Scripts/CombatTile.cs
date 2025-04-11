using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatTile : MonoBehaviour
{
    [SerializeField] private UIUnivarsalData uiData;
    private SpriteRenderer renderer;

    [NonSerialized] public Character occupant = null;

    private void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.color = uiData.defaultColor;
    }

    private void OnMouseEnter()
    {
        renderer.DOColor(uiData.hoverColor, uiData.hoverLerpSpeed);
    }

    private void OnMouseExit()
    {
        renderer.DOColor(uiData.defaultColor, uiData.hoverLerpSpeed);
    }
    
    public Vector2 GetCombatTilePos()
    {
        return ((RectTransform)transform).position;
    }

    public void AttackTile(int damage)
    {
        renderer.color = uiData.hurtColor;
        renderer.DOColor(uiData.defaultColor, uiData.outOfHurtLerpSpeed);
        if (occupant is not null)
            occupant.TakeDamage(damage);
    }

    public bool Occupied()
    {
        return occupant is not null;
    }
}
