using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;


public enum PosType
{
    errorType,
    hub,
    store,
    grass
}

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected float moveRange = 4.5f;
    [SerializeField]
    protected float moveSpeed = 0.5f;
    [SerializeField]
    public int productV = 1;

    [SerializeField]
    public int sunLimit = 10;

    [SerializeField]
    public int hasSun = 0;
    public TextMeshProUGUI sheepSunCnt;//显示当前羊群携带阳光数量

    public int sheepCnt = 6;

    public int posIndex = -2;
    public PosType preType = PosType.errorType;
    public PosType curType = PosType.errorType;
    protected float totalTime = 0;

    protected SpriteRenderer spriteRenderer;
    public bool canCtrl = true;
    public Color selectedColor;

    void Start()
    {
        canCtrl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GameManager.Instance.grassTiles[ Random.Range(0, 8) ].transform.position;
    }

    protected void OnMouseEnter()
    {
        if (!canCtrl) return;
        transform.localScale += Vector3.one * 0.1f;
        //spriteRenderer.sortingOrder = 25;
    }

    protected void OnMouseExit()
    {
        if (!canCtrl) return;
        transform.localScale -= Vector3.one * 0.1f;
        //spriteRenderer.sortingOrder = 0;
    }
    protected void OnMouseDown()
    {
        if (!canCtrl) return;

        spriteRenderer.color = selectedColor; 
        if(GameManager.Instance.selectedUnit ==  null)
            GameManager.Instance.selectedUnit = this;
        if (GameManager.Instance.selectedUnit != this)
        {
            GameManager.Instance.selectedUnit.spriteRenderer.color = Color.white;
            GameManager.Instance.selectedUnit = this;
        }
        ShowWalkableRoad();
    }

    protected PosType GetCurInfo()
    {
        if (Vector2.Distance(transform.position, GameManager.Instance.hubTile.transform.position) < 0.5f)
        {
            posIndex = -1;
            return PosType.hub;
        }

        for (int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            if (Vector2.Distance(transform.position, GameManager.Instance.grassTiles[i].transform.position) < 0.5f)
            {
                posIndex = i;
                return PosType.grass;
            }
        }
        // temp store
        for (int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            if (Vector2.Distance(transform.position, GameManager.Instance.storeTiles[i].transform.position) < 0.5f)
            {
                posIndex = i;
                return PosType.store;
            }
        }
        return PosType.errorType;
    }

    protected void ShowWalkableRoad()
    {
        //Debug.Log(transform.position);

        curType = GetCurInfo();
        //Debug.Log(curType.ToString());
        //hub
        //Debug.Log("hub : " + GameManager.Instance.hubTile.transform.position);
        
        if (curType == PosType.store && Vector2.Distance(transform.position, GameManager.Instance.hubTile.transform.position) <= moveRange)
        {
            GameManager.Instance.hubTile.canGo = true;
            GameManager.Instance.hubTile.HighlightTile();
        }

        //grass
        for (int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            //Debug.Log("grass : " + GameManager.Instance.grassTiles[i].transform.position);
            if ((curType == PosType.grass || curType == PosType.store) && Vector2.Distance(transform.position, GameManager.Instance.grassTiles[i].transform.position) <= moveRange)
            {
                GameManager.Instance.grassTiles[i].canGo = true;
                GameManager.Instance.grassTiles[i].HighlightTile();
            }
        }
        // temp store
        for (int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            //Debug.Log("store : " + GameManager.Instance.storeTiles[i].transform.position);
            if (Vector2.Distance(transform.position, GameManager.Instance.storeTiles[i].transform.position) <= moveRange)
            {
                GameManager.Instance.storeTiles[i].canGo = true;
                GameManager.Instance.storeTiles[i].HighlightTile();
            }
        }

    }

    public void Move(Transform _trans)
    {
        ResetTiles();
        canCtrl = false;
        spriteRenderer.color = Color.white;
        StartCoroutine(MoveCo(_trans));
        canCtrl = true;
    }

    IEnumerator MoveCo(Transform _trans)
    {
        while (Vector2.Distance(transform.position, _trans.position) > 0.0001f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _trans.position, moveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0);
        }
    }

    protected void ResetTiles()
    {
        GameManager.Instance.hubTile.ResetTile();
        for(int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            GameManager.Instance.storeTiles[i].ResetTile();
        }
        for (int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            GameManager.Instance.grassTiles[i].ResetTile();
        }
    }
}
