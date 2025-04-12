using UnityEngine;

[CreateAssetMenu(fileName = "UIData", menuName = "Data/UIData")]
public class UIData : ScriptableObject
{
    [Header("Interactable coloring")]
    //Colors and lerp speed for ui buttons
    public Color defaultColor;
    
    public Color hoverColor;
    public float hoverLerpSpeed;
    
    public Color selectColor;
    public float selectLerpSpeed;
}