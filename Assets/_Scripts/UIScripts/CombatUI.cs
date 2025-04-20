using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header("Combat Buttons")]
    [SerializeField] private GameObject[] moveLeftButtons;
    [SerializeField] private GameObject[] moveRightButtons;
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private RectTransform layoutGroup;
    
    [Space(5)]
    [Header("Misc")] 
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private void Awake()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    public void SetUI(Character uiTarget)
    {
        if (uiTarget is null)
        {
            layoutGroup.gameObject.SetActive(false);
            return;
        }
        else
        {
            layoutGroup.gameObject.SetActive(true);
        }

        GameObject[] forwardButtons = uiTarget.facingRight ? moveRightButtons : moveLeftButtons;
        GameObject[] backButtons = uiTarget.facingRight ? moveLeftButtons : moveRightButtons;
        //Handle forward buttons:
        for (int i = 0; i < forwardButtons.Length; i++)
        {
            bool allowed = i + 1 <= uiTarget.MoveRange.y;
            forwardButtons[i].SetActive(allowed);
        }
        //Handle backwards buttons:
        for (int i = 0; i < backButtons.Length; i++)
        {
            bool allowed = i + 1 <= -uiTarget.MoveRange.x;
            backButtons[i].SetActive(allowed);
        }
        //Set action buttons:
        for (int i = 0; i < actionButtons.Length; i++)
        {
            bool allowed = i < uiTarget.Actions.Length;
            actionButtons[i].gameObject.SetActive(allowed);
            if (allowed)
            {
                actionButtons[i].HandleActionSetUp(uiTarget.Actions[i]);
            }
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }

    public void SetCombatEndUI(bool gameWon)
    {
        winScreen.SetActive(gameWon);
        loseScreen.SetActive(!gameWon);
    }

    public void ReturnToMapScreen()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
