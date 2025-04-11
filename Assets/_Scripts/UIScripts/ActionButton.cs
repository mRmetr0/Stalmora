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
        renderer.DOColor(uiData.selectColor, uiData.selectLerpSpeed);
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        renderer.DOColor(uiData.hoverColor, uiData.hoverLerpSpeed);
    }
    
    public void OnPointerExit(PointerEventData data)
    {
        renderer.DOColor(uiData.defaultColor, uiData.hoverLerpSpeed);
    }
}
