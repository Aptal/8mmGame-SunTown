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
    protected float moveSpeed = 0.2f;
    [SerializeField]
    public int productV = 1;

    [SerializeField]
    public int sunLimit = 10;

    [SerializeField]
    public int hasSun = 0;
    public TextMeshProUGUI sheepSunCnt;//��ʾ��ǰ��ȺЯ����������

    public int sheepCnt = 6;

    public int posIndex = -2;
    public PosType preType = PosType.errorType;
    public PosType curType = PosType.errorType;
    protected float totalTime = 0;

    protected SpriteRenderer spriteRenderer;

    [SerializeField] protected Sprite sheepSprite;  // ԭ���ͼƬ
    [SerializeField] protected Sprite flagSprite;   // ���ĵ�ͼƬ
    public bool isFlag = false;  // �Ƿ�������״̬
    public bool isBeingDragged = false; // �Ƿ����ڱ��϶�
    protected Vector3 stopPosition; // ��¼��ײʱ��λ��
    protected Collider2D sheepCollider;

    public bool canCtrl = true;
    public Color selectedColor;

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

        // ȷ��������ײ��������Ⱥ���ҵ�ǰ��Ⱥû�б������
        if (otherSheep != null && !isFlag && !otherSheep.isFlag)
        {
            HandleCollisionWithOtherSheep(otherSheep);
        }
    }

    // ������Ⱥ֮�����ײ�߼�
    protected void HandleCollisionWithOtherSheep(Unit otherSheep)
    {
        // ����ǰ��Ⱥ��������Ⱥ����������
        this.hasSun = 0;
        otherSheep.hasSun = 0;

        // ���ѡ��һ�������ǰ��
        Unit selectedSheep = Random.value > 0.5f ? this : otherSheep;

        // ��δ��ѡ�����������
        Unit flagSheep = (selectedSheep == this) ? otherSheep : this;
        flagSheep.TurnIntoFlag();
    }

    // ����Ⱥ�������
    protected void TurnIntoFlag()
    {
        isFlag = true;
        //canCtrl = false; // �������������
        //sheepCollider.enabled = false; // ��ʱ������ײ
        GetComponent<SpriteRenderer>().sprite = flagSprite; // �ı�ͼƬΪ����
        
        stopPosition = transform.position; 
        // ֹͣ�ƶ�����λ�ù̶�
        StopAllCoroutines();
        transform.position = stopPosition;
    }

    // ����ҵ�����Ĳ���������ڲݵ�ʱ���ָ�Ϊ���״̬
    public void PlaceOnGrassTile(Vector3 grassTilePosition)
    {
        // �ָ����״̬
        isFlag = false;
        canCtrl = true;
        transform.position = grassTilePosition; // �������ƶ���ָ���ݵ�
        GetComponent<SpriteRenderer>().sprite = sheepSprite; // �ı�ͼƬΪ��
        sheepCollider.enabled = true; // ����������ײ
    }

    //protected void OnMouseDown()
    //{
    //    if (isFlag)
    //    {
    //        isBeingDragged = true; // ��ʼ�϶�
    //    }
    //}

    //protected void OnMouseUp()
    //{
    //    if (isFlag)
    //    {
    //        isBeingDragged = false; // ֹͣ�϶�
    //        TryPlaceFlag(); // ���Է�������
    //    }
    //}

    // ���Խ����ķ��õ��ݵ���
    //protected void TryPlaceFlag()
    //{
    //    // ��ȡ��굱ǰλ��
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    // ���������Ƿ��вݵ� Tile
    //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));
    //    if (hit.collider != null)
    //    {
    //        GrassTile grassTile = hit.collider.GetComponent<GrassTile>();
    //        if (grassTile != null && grassTile.canGo) // ����ǿ��ߵĲݵ�
    //        {
    //            PlaceOnGrassTile(grassTile.transform.position); // �����ķ��õ��ݵ���
    //        }
    //    }
    //}

    // �����ĸ�������ƶ�
/*    private void OnMouseDrag()
    {
        if (isFlag)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // ȷ��ֻ��X��Y���ƶ�
            transform.position = mousePosition;
        }
    }*/


    /*public void BecomeFlag()
    {
        spriteRenderer.sprite = flagSprite;
        isFlag = true;
        canCtrl = false; // ��ֹ�ƶ�
    }

    public void BecomeSheep()
    {
        spriteRenderer.sprite = sheepSprite;
        isFlag = false;
        canCtrl = true; // �ָ��ƶ�����
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

            // ��������������ֹͣ��ק������Ƿ��ڲݵ���
            if (Input.GetMouseButtonDown(0))
            {
                // ����Ƿ��ڲݵ��Ϸ���
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grass"));
                if (hit.collider != null)
                {
                    BecomeSheep(); // �ָ���״̬
                    break;
                }
            }
            yield return null;
        }
    }*/

    public void SelectSheep()
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
        //while (Vector2.Distance(transform.position, _trans.position) > 0.0001f)
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, _trans.position, moveSpeed * Time.deltaTime);
        //    yield return new WaitForSeconds(0);
        //}
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = _trans.position;
        float elapsedTime = 0f;
        float moveDuration = 1f / moveSpeed;  // ����moveSpeed�����ƶ�ʱ��

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // ��֤����λ�þ�׼
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
