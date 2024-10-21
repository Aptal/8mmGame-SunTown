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
    protected float moveRange = 6f;
    [SerializeField]
    protected float moveSpeed = 0.2f;
    [Header("kStore(羊在草地和仓库间的系数，speed*k)")]
    [SerializeField]
    protected float kStore = 0.5f;
    [Header("kHub(羊在中枢和仓库间的系数，speed*k)")]
    [SerializeField]
    protected float kHub = 0.25f;

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

    [SerializeField] protected Sprite sheepSprite;  // 原羊的图片
    [SerializeField] protected Sprite flagSprite;   // 旗帜的图片
    public bool isFlag = false;  // 是否处于旗帜状态
    public bool isBeingDragged = false; // 是否正在被拖动
    protected Vector3 stopPosition; // 记录碰撞时的位置
    protected Collider2D sheepCollider;

    public bool canCtrl = true;
    public bool isSelected = false;
    public Color selectedColor = Color.gray;

    void Start()
    {
        canCtrl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sheepCollider = GetComponent<Collider2D>();
        //transform.position = GameManager.Instance.grassTiles[ Random.Range(0, 8) ].transform.position;
    }


    protected void OnMouseEnter()
    {
        if (!canCtrl) return;
        if(spriteRenderer.sortingOrder > 10)
            transform.localScale += Vector3.one * 0.1f;
    }

    protected void OnMouseExit()
    {
        if (!canCtrl) return;
        if(spriteRenderer.sortingOrder > 10)
            transform.localScale -= Vector3.one * 0.1f;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        Unit otherSheep = other.GetComponent<Unit>();

        // 确保两个碰撞对象都是羊群，且当前羊群没有变成旗帜
        if (otherSheep != null && !isFlag && !otherSheep.isFlag)
        {
            HandleCollisionWithOtherSheep(otherSheep);
        }
    }

    // 处理羊群之间的碰撞逻辑
    protected void HandleCollisionWithOtherSheep(Unit otherSheep)
    {
        // 将当前羊群和其他羊群的阳光清零
        this.hasSun = 0;
        otherSheep.hasSun = 0;

        // 随机选择一个羊继续前进
        Unit selectedSheep = Random.value > 0.5f ? this : otherSheep;

        // 让未被选择的羊变成旗帜
        Unit flagSheep = (selectedSheep == this) ? otherSheep : this;
        flagSheep.TurnIntoFlag();
    }

    // 将羊群变成旗帜
    protected void TurnIntoFlag()
    {
        isFlag = true;
        //canCtrl = false; // 不允许继续控制
        //sheepCollider.enabled = false; // 暂时禁用碰撞
        GetComponent<SpriteRenderer>().sprite = flagSprite; // 改变图片为旗帜
        
        stopPosition = transform.position; 
        // 停止移动，将位置固定
        StopAllCoroutines();
        transform.position = stopPosition;
    }

    // 当玩家点击旗帜并将其放置在草地时，恢复为羊的状态
    public void PlaceOnGrassTile(Vector3 grassTilePosition)
    {
        // 恢复羊的状态
        isFlag = false;
        canCtrl = true;
        transform.position = grassTilePosition; // 将旗帜移动到指定草地
        GetComponent<SpriteRenderer>().sprite = sheepSprite; // 改变图片为羊
        sheepCollider.enabled = true; // 重新启用碰撞
    }

    //protected void OnMouseDown()
    //{
    //    if (isFlag)
    //    {
    //        isBeingDragged = true; // 开始拖动
    //    }
    //}

    //protected void OnMouseUp()
    //{
    //    if (isFlag)
    //    {
    //        isBeingDragged = false; // 停止拖动
    //        TryPlaceFlag(); // 尝试放置旗帜
    //    }
    //}

    // 尝试将旗帜放置到草地上
    //protected void TryPlaceFlag()
    //{
    //    // 获取鼠标当前位置
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    // 检查鼠标下是否有草地 Tile
    //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));
    //    if (hit.collider != null)
    //    {
    //        GrassTile grassTile = hit.collider.GetComponent<GrassTile>();
    //        if (grassTile != null && grassTile.canGo) // 如果是可走的草地
    //        {
    //            PlaceOnGrassTile(grassTile.transform.position); // 将旗帜放置到草地上
    //        }
    //    }
    //}

    // 让旗帜跟随鼠标移动
/*    private void OnMouseDrag()
    {
        if (isFlag)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // 确保只在X和Y轴移动
            transform.position = mousePosition;
        }
    }*/


    /*public void BecomeFlag()
    {
        spriteRenderer.sprite = flagSprite;
        isFlag = true;
        canCtrl = false; // 禁止移动
    }

    public void BecomeSheep()
    {
        spriteRenderer.sprite = sheepSprite;
        isFlag = false;
        canCtrl = true; // 恢复移动控制
    }

    void OnMouseDown()
    {
        if (isFlag)
        {
            StartCoroutine(DragFlag());
        }
    }

    private IEnumerator DragFlag()
    {
        while (isFlag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x, mousePosition.y);

            // 如果鼠标左键点击，停止拖拽并检查是否在草地上
            if (Input.GetMouseButtonDown(0))
            {
                // 检查是否在草地上放置
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grass"));
                if (hit.collider != null)
                {
                    BecomeSheep(); // 恢复羊状态
                    break;
                }
            }
            yield return null;
        }
    }*/

    public void SelectSheep()
    {
        if (isSelected) return;
        isSelected = true;
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

    bool ComparePos(Vector2 pos, Vector2 pos2)
    {
        return pos == pos2;
    }

    IEnumerator MoveCo(Transform _trans)
    {
        //while (Vector2.Distance(transform.position, _trans.position) > 0.0001f)
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, _trans.position, moveSpeed * Time.deltaTime);
        //    yield return new WaitForSeconds(0);
        //}
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = _trans.position;

        int type1 = -1, type2 = -1;
        for(int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            if(type1 == -1 && ComparePos(startPosition, GameManager.Instance.grassTiles[i].transform.position))
            {
                type1 = 1;
            }
            if (type2 == -1 && ComparePos(targetPosition, GameManager.Instance.grassTiles[i].transform.position))
            {
                type2 = 1;
            }
        }
        for (int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            if (type1 == -1 && ComparePos(startPosition, GameManager.Instance.storeTiles[i].transform.position))
            {
                type1 = 2;
            }
            if (type2 == -1 && ComparePos(targetPosition, GameManager.Instance.storeTiles[i].transform.position))
            {
                type2 = 2;
            }
        }
        if (type1 == -1 && ComparePos(startPosition, GameManager.Instance.hubTile.transform.position))
        {
            type1 = 3;
        }
        if (type2 == -1 && ComparePos(targetPosition, GameManager.Instance.hubTile.transform.position))
        {
            type2 = 3;
        }
        float k = 1;
        if(type1 == 3 ||  type2 == 3)
        {
            k = kHub;
        }   
        else if(type1 == 2 || type2 == 2)
        {
            k = kStore;
        }

        //moveSpeed *= k;
        float v = moveSpeed * k;
        float elapsedTime = 0f;
        float moveDuration = 1f / v;  // 根据moveSpeed (v)调整移动时间

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // 保证最终位置精准
        if (transform.position == _trans.position)
        {
            isSelected = false;
        }
        //moveSpeed /= k; //复原
    }

    public  void ResetTiles()
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
