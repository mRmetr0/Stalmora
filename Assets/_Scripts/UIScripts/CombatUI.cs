using Unity.Collections;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject[] moveLeftButtons;
    [SerializeField] private GameObject[] moveRightButtons;
    [SerializeField] private ActionButton[] actionButtons;
    public void SetUI(Character UITarget)
    {
        //Set movement buttons:
        for (int i = 0; i < moveRightButtons.Length; i++)
        {
            bool allowed = i + 1 <= UITarget.MoveRange.y;
            Debug.Log($"{i+1} <= {UITarget.MoveRange.y} = {allowed}");
            moveRightButtons[i].SetActive(allowed);
        }

        Debug.Log("--------------");
        for (int i = 0; i < moveLeftButtons.Length; i++)
        {
            bool allowed = i + 1 <= -UITarget.MoveRange.x;
            Debug.Log($"{i + 1} < {-UITarget.MoveRange.x} = {allowed}");
            moveLeftButtons[i].SetActive(allowed);
            Debug.Log($"{i} active? {allowed}");
        }
        
        //Set action buttons:
        for (int i = 0; i < actionButtons.Length; i++)
        {
            bool allowed = i < UITarget.Actions.Length;
            actionButtons[i].gameObject.SetActive(allowed);
            if (allowed)
            {
                actionButtons[i].HandleActionSetUp(UITarget.Actions[i]);
            }

        }
    }
}
