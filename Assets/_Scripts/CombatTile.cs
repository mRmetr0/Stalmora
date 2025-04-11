using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatTile : MonoBehaviour
{
    [Header("Color Data")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private float colorLerpSpeed = .7f;

    private SpriteRenderer renderer;

    [NonSerialized] public bool occupied;

    private void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.color = defaultColor;
    }

    private void OnMouseEnter()
    {
        renderer.DOColor(hoverColor, colorLerpSpeed);
    }

    private void OnMouseExit()
    {
        renderer.DOColor(defaultColor, colorLerpSpeed);
    }
    
    public Vector2 GetCombatTilePos()
    {
        return ((RectTransform)transform).position;
    }
}
