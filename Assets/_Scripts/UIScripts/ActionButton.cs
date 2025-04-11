using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CombatAction action;

    [Header("Color Data")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color downColor;
    [SerializeField] private float colorLerpSpeed = .7f;

    private Image renderer;

    private void Awake()
    {
        renderer = GetComponent<Image>();
        
        TMP_Text label = GetComponentInChildren<TMP_Text>();
        label.text = action.name;
        
        OnPointerExit(null);
    }

    public void OnPointerDown(PointerEventData data)
    {
        action.UseAction(CombatManager.manager.PlayerCharacter);
        renderer.DOColor(downColor, colorLerpSpeed);
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        renderer.DOColor(hoverColor, colorLerpSpeed);
    }
    
    public void OnPointerExit(PointerEventData data)
    {
        renderer.DOColor(defaultColor, colorLerpSpeed);
    }
}
