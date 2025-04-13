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
    [SerializeField] private CombatAction.Effect actionEffect;
    

    [SerializeField] private UIData uiData;
    private Sequence sequence;
    private Image spriteRenderer;
    private TMP_Text label;

    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
        sequence = DOTween.Sequence();
        label = GetComponentInChildren<TMP_Text>();
        HandleActionSetUp();
        
        OnPointerExit(null);
    }

    public void HandleActionSetUp(CombatAction _action = null)
    {
        if (_action is not null)
        {
            action = _action;
        }

        if (builtIn)
        {
            action = ScriptableObject.CreateInstance<CombatAction>();
            action.SimpleInit(actionName, actionDesc, actionEffect);
        }
        if (action is null) return;
        
        label.text = action.title;
    }

    public void OnPointerDown(PointerEventData data)
    {
        action.UseAction(TileManager.manager.PlayerCharacter);
        spriteRenderer.color = uiData.selectColor;
        TweenColor(uiData.hoverColor, uiData.selectLerpSpeed);
        CombatManager.manager.EndAction();
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
        sequence.Append(spriteRenderer.DOColor(endValue, lerpSpeed));
    }
}
