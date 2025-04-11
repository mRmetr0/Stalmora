using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CombatAction action;

    [SerializeField] private UIUnivarsalData uiData;
    private Sequence sequence;
    private Image renderer;

    private void Awake()
    {
        renderer = GetComponent<Image>();
        sequence = DOTween.Sequence();
        
        TMP_Text label = GetComponentInChildren<TMP_Text>();
        label.text = action.name;
        
        OnPointerExit(null);
    }

    public void OnPointerDown(PointerEventData data)
    {
        action.UseAction(CombatManager.manager.PlayerCharacter);
        renderer.color = uiData.selectColor;
        TweenColor(uiData.hoverColor, uiData.selectLerpSpeed);
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        TweenColor(uiData.hoverColor, uiData.hoverLerpSpeed);
    }
    
    public void OnPointerExit(PointerEventData data)
    {
        TweenColor(uiData.defaultColor, uiData.hoverLerpSpeed);
    }
    
    private void TweenColor(Color endValue, float lerpSpeed)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(renderer.DOColor(endValue, lerpSpeed));
    }
}
