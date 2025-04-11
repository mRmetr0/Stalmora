using UnityEngine;

[CreateAssetMenu(fileName = "UIUnivarsalData", menuName = "Scriptable Objects/UIUnivarsalData")]
public class UIUnivarsalData : ScriptableObject
{
    public Color hurtColor;
    public float outOfHurtLerpSpeed;
    
    [Header("Interactable coloring")]
    //Colors and lerp speed for ui buttons
    public Color defaultColor;
    
    public Color hoverColor;
    public float hoverLerpSpeed;
    
    public Color selectColor;
    public float selectLerpSpeed;
}
