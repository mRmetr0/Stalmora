using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Data/ObjectData")]
public class ObjectData : ScriptableObject
{
    public Color hurtColor;
    public float outOfHurtLerpSpeed;
}
