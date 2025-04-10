using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatTile : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private float colorLerpSpeed = .7f;

    private SpriteRenderer renderer;

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
}
