using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Character character;

    [SerializeField] private GameObject tileContainer;
    private CombatTile[] tiles;

    [Header("Lerp Data")] 
    [SerializeField] private float moveLerpSpeed;

    private void Awake()
    {
        SetUpTileLayout();
        tiles = tileContainer.GetComponentsInChildren<CombatTile>();

        StartCoroutine(LateAwake());
    }

    private IEnumerator LateAwake() //TODO: Have layout group set up in code instead of waiting a frame
    {
        yield return new WaitForEndOfFrame();
        character.transform.position = tiles[0].GetCombatTilePos();
    }

    private void Update()
    {
        int move = (Input.GetKey(KeyCode.LeftShift)) ? 2 : 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCharacter(character, -move);   
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCharacter(character, move);   
        }
    }
    
    private void SetUpTileLayout()
    {
        HorizontalLayoutGroup group = tileContainer.GetComponent<HorizontalLayoutGroup>();
        group.CalculateLayoutInputHorizontal();
        group.SetLayoutHorizontal();
        group.CalculateLayoutInputVertical();
        group.SetLayoutVertical();
    }

    public void MoveCharacter(Character character, int direction)
    {
        int oldPos = character.tilePos;
        int nextPos = character.tilePos;
        for (int i = 1; i <= Mathf.Abs(direction); i++)
        {
            nextPos = character.tilePos + direction / Mathf.Abs(direction) * i;
            //Check if can move
            if (nextPos < 0 || nextPos >= tiles.Length) return; //CANNOT MOVE
            if (tiles[nextPos].occupied) return; //CANNOT MOVE
        }
        //Move and update data
        tiles[oldPos].occupied = false;
        character.tilePos = nextPos;
        character.transform.DOMove(tiles[nextPos].GetCombatTilePos(), moveLerpSpeed);
        tiles[nextPos].occupied = true;
    }
}
