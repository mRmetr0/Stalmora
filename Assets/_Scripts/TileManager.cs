using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager manager;
    public Character PlayerCharacter;
    public Character EnemyCharacter;

    [SerializeField] private GameObject tileContainer;
    private CombatTile[] tiles;

    [Header("Lerp Data")] 
    [SerializeField] private float moveLerpSpeed;

    private void Awake()
    {
        if (manager != null)
        {
            Debug.LogError("TOO MANY MANAGERS!!");
            Destroy(gameObject);
            return;
        }

        manager = this;

        // SetUpTileLayout();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tileContainer.transform);
        tiles = tileContainer.GetComponentsInChildren<CombatTile>();

        // StartCoroutine(LateAwake());
        PlaceCharacter(PlayerCharacter, 1);
        PlaceCharacter(EnemyCharacter, 3);
    }

    private void OnDestroy()
    {
        if (manager == this) manager = null;
    }

    // private void Update()
    // {
    //     //DEBUG TOOLS
    //     int move = (Input.GetKey(KeyCode.LeftShift)) ? 2 : 1;
    //     if (Input.GetKeyDown(KeyCode.LeftArrow))
    //     {
    //         MoveCharacter(PlayerCharacter, -move);   
    //     }
    //     
    //     if (Input.GetKeyDown(KeyCode.RightArrow))
    //     {
    //         MoveCharacter(PlayerCharacter, move);   
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         PlayerCharacter.TakeDamage(1);
    //     }
    // }
    
    private void SetUpTileLayout()
    {
        HorizontalLayoutGroup group = tileContainer.GetComponent<HorizontalLayoutGroup>();
        group.CalculateLayoutInputHorizontal();
        group.SetLayoutHorizontal();
        group.CalculateLayoutInputVertical();
        group.SetLayoutVertical();
    }

    public CombatTile GetTile(int posIndex)
    {
        if (posIndex < 0 || posIndex >= tiles.Length) return null;
        return tiles[posIndex];
    }

    public void MoveCharacter(Character character, int direction)
    {
        int oldPos = character.tilePos;
        int nextPos = character.tilePos;
        for (int i = 1; i <= Mathf.Abs(direction); i++)
        {
            nextPos = character.tilePos + direction / Mathf.Abs(direction) * i;
            //Check if can move
            if (nextPos < 0 || nextPos >= tiles.Length || tiles[nextPos].Occupied()) return; //CANNOT MOVE
        }
        //Move and update data
        tiles[oldPos].occupant = null;
        character.tilePos = nextPos;
        character.transform.DOMove(tiles[nextPos].GetCombatTilePos(), moveLerpSpeed);
        tiles[nextPos].occupant = character;
    }

    private void PlaceCharacter(Character character, int newPos)
    {
        if (newPos < 0 || newPos >= tiles.Length) return;
        tiles[character.tilePos].occupant = null;
        character.tilePos = newPos;
        character.transform.position = tiles[newPos].GetCombatTilePos();
        tiles[newPos].occupant = character;
    }
}
