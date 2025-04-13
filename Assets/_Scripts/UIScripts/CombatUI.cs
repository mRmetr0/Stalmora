using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject[] moveLeftButtons;
    [SerializeField] private GameObject[] moveRightButtons;
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private RectTransform[] layoutGroups;
    public void SetUI(Character UITarget)
    {
        GameObject[] forwardButtons = UITarget.facingRight ? moveRightButtons : moveLeftButtons;
        GameObject[] backButtons = UITarget.facingRight ? moveLeftButtons : moveRightButtons;
        //HandleForwardButtons:
        for (int i = 0; i < forwardButtons.Length; i++)
        {
            bool allowed = i + 1 <= UITarget.MoveRange.y;
            forwardButtons[i].SetActive(allowed);
        }
        //HandleBackwardsButtons:
        for (int i = 0; i < backButtons.Length; i++)
        {
            bool allowed = i + 1 <= -UITarget.MoveRange.x;
            backButtons[i].SetActive(allowed);
        }

        foreach (RectTransform group in layoutGroups)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(group);
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
