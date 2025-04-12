using System;
using System.Runtime.InteropServices;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private bool builtIn = false;
    
    [HideIf("builtIn")]
    [SerializeField] private CombatAction action;
    
    //Variables for built in actions
    [ShowIf("builtIn")] 
    [SerializeField] private string actionName;
    [ShowIf("builtIn")]
    [SerializeField] private string actionDesc;
    [ShowIf("builtIn")] 
    [SerializeField] private CombatAction.Effect[] actionEffects;
    

    [SerializeField] private UIData uiData;
    private Sequence sequence;
    private Image renderer;

    private void Awake()
    {
        renderer = GetComponent<Image>();
        sequence = DOTween.Sequence();
        HandleActionSetUp();
        
        OnPointerExit(null);
    }

    private void HandleActionSetUp()
    {
        if (builtIn)
        {
            action = ScriptableObject.CreateInstance<CombatAction>();
            action.Init(actionName, actionDesc, false, actionEffects, actionEffects);
        }
        TMP_Text label = GetComponentInChildren<TMP_Text>();
        label.text = action.name;
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
