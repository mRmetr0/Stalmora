using System;
using System.Collections.Generic;
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
        Debug.Log(tiles.Length);
        
        foreach (CombatTile tile in tiles)
        {
            Debug.Log(tile.GetCombatTilePos());
        }

        character.transform.position = tiles[0].GetCombatTilePos();
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
        int nextPos = character.tilePos + direction;
        if (nextPos < 0 || nextPos >= tiles.Length) return;
        character.tilePos = nextPos;
        character.transform.DOMove(tiles[nextPos].GetCombatTilePos(), moveLerpSpeed);
    }
}
