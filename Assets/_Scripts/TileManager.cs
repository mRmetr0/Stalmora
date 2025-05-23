using System.Collections;
using DG.Tweening;
using UnityEngine;
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

    public CombatTile GetTile(int posIndex)
    {
        if (posIndex < 0 || posIndex >= tiles.Length) return null;
        return tiles[posIndex];
    }

    /// <summary>
    /// Move character from one tile to another
    /// </summary>
    /// <param name="character">Character to move</param>
    /// <param name="direction">Direction to move</param>
    /// <param name="onlyIfExecutable">If cant fully move, dont move character at all</param>
    /// <returns>If movement was able to fully execute</returns>
    public IEnumerator MoveCharacter(Character character, int direction, bool onlyIfExecutable = true)
    {
        int oldPos = character.tilePos;
        int nextPos = oldPos;
        for (int i = 1; i <= Mathf.Abs(direction); i++)
        {
            int nextPosCalc = character.tilePos + direction / Mathf.Abs(direction) * i;
            //Check if can move
            if (nextPosCalc < 0 || nextPosCalc >= tiles.Length || tiles[nextPosCalc].Occupied()) //CANNOT MOVE FURTHER
            {
                if (onlyIfExecutable)
                {
                    yield return false;
                    yield break;
                }
            }
            nextPos = nextPosCalc;
        }

        //Move and update data
        tiles[oldPos].occupant = null;
        character.tilePos = nextPos;
        Tween moveTween = character.transform.DOMove(tiles[nextPos].GetCombatTilePos(), moveLerpSpeed);
        yield return moveTween.WaitForCompletion();
        tiles[nextPos].occupant = character;
        yield return true;
        
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
