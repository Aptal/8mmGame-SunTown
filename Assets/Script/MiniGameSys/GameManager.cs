using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public Tile[] tiles;
    public StoreTile[] storeTiles;
    public GrassTile[] grassTiles;
    public SunSheepCtrl[] sunSheep;
    public ShadowSheepCtrl[] shadowSheep;

    public HubTile hubTile;
    public TextMeshProUGUI showCnt;
    public Unit selectedUnit;
    public Unit[] showUnits;

    [SerializeField] public Timer timer1;
    [SerializeField] public ShadowControl shadow;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timer1.SetDuration(180).Begin();

        // 初始化羊的位置
        PlaceSheepOnGrass();
    }

    // 随机将羊分配到草地上
    private void PlaceSheepOnGrass()
    {
        // 创建草地的索引列表，草地总数是8
        List<int> grassIndices = new List<int>();
        for (int i = 0; i < grassTiles.Length; i++)
        {
            grassIndices.Add(i);
        }

        // 使用Fisher-Yates算法打乱草地索引列表
        for (int i = grassIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = grassIndices[i];
            grassIndices[i] = grassIndices[randomIndex];
            grassIndices[randomIndex] = temp;
        }

        // 至少选一只阳光羊和一只阴影羊
        List<Unit> selectedSheep = new List<Unit>();
        selectedSheep.Add(sunSheep[Random.Range(0, sunSheep.Length)]);
        selectedSheep.Add(shadowSheep[Random.Range(0, shadowSheep.Length)]);

        // 剩余的6个随机挑选羊
        List<Unit> allSheep = new List<Unit>();
        allSheep.AddRange(sunSheep);
        allSheep.AddRange(shadowSheep);

        // 移除已选择的羊
        allSheep.Remove(selectedSheep[0]); // 移除已经选中的阳光羊
        allSheep.Remove(selectedSheep[1]); // 移除已经选中的阴影羊

        // 从剩余的羊中挑选6个
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, allSheep.Count);
            selectedSheep.Add(allSheep[randomIndex]);
            allSheep.RemoveAt(randomIndex); // 移除已选择的羊
        }

        // 将羊分配到打乱顺序的草地上
        for (int i = 0; i < selectedSheep.Count; i++)
        {
            Unit sheep = selectedSheep[i];
            int grassIndex = grassIndices[i];
            // 将羊的位置设置为相应的草地位置
            sheep.gameObject.SetActive(true);
            sheep.transform.position = grassTiles[grassIndex].transform.position;
            // 取消选择状态
            //sheep.ResetTiles();
        }
    }
    /*private void PlaceSheepOnGrass()
    {
        // 创建草地的索引列表，草地总数是8
        List<int> grassIndices = new List<int>();
        for (int i = 0; i < grassTiles.Length; i++)
        {
            grassIndices.Add(i);
        }

        // 使用Fisher-Yates算法打乱草地索引列表
        for (int i = grassIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = grassIndices[i];
            grassIndices[i] = grassIndices[randomIndex];
            grassIndices[randomIndex] = temp;
        }
        for(int i = 0; i < grassTiles.Length; i++)
        {
            Debug.Log(grassIndices[i]);
        }

        int sunSheepCnt = Random.Range(1, sunSheep.Length);
        int shadowSheepCnt = grassTiles.Length - sunSheepCnt;

        List<Unit> allSunSheep = new List<Unit>();
        List<Unit> allShadowSheep = new List<Unit>();
        allSunSheep.AddRange(sunSheep);
        allShadowSheep.AddRange(shadowSheep);

        int grassIndex = 0;
        for (int i = 0; i < sunSheepCnt; i++)
        {
            int randomIndex = Random.Range(0, allSunSheep.Count);
            allSunSheep[randomIndex].gameObject.SetActive(true);
            allSunSheep[randomIndex].transform.position = grassTiles[grassIndices[grassIndex++]].transform.position;
            allSunSheep.RemoveAt(randomIndex); // 移除已选择的羊
        }

        for (int i = 0; i < shadowSheepCnt; i++)
        {
            int randomIndex = Random.Range(0, allShadowSheep.Count);
            allShadowSheep[randomIndex].gameObject.SetActive(true);
            allShadowSheep[randomIndex].transform.position = grassTiles[grassIndices[grassIndex++]].transform.position;
            allShadowSheep.RemoveAt(randomIndex); // 移除已选择的羊
        }

*//*        // 将羊分配到打乱顺序的草地上
        for (int i = 0; i < selectedSheep.Count; i++)
        {
            Unit sheep = selectedSheep[i];
            //int grassIndex = grassIndices[i];
            // 将羊的位置设置为相应的草地位置
            sheep.gameObject.SetActive(true);
            sheep.transform.position = grassTiles[i].transform.position;
            // 取消选择状态
            //sheep.ResetTiles();
        }*//*
    }*/


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 先检查羊群所在的层
            RaycastHit2D hitSheep = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("player"));
            if (hitSheep.collider != null)
            {
                Unit sheep = hitSheep.collider.GetComponent<Unit>();
                if(sheep.isFlag)
                {
                    sheep.isBeingDragged = true;
                    selectedUnit = sheep;
                    Debug.Log("flag");
                    return;
                }
                if(sheep != null)
                {
                    sheep.SelectSheep();
                    Debug.Log("sheep");

                }
                return; // 成功选中羊群后，不继续检测
            }

            // 检查Tile层（草地/仓库等）
            RaycastHit2D hitTile = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));
            if (hitTile.collider != null)
            {
                Tile tile = hitTile.collider.GetComponent<Tile>();
                if(tile != null)
                {
                    if(selectedUnit != null && selectedUnit.isFlag)
                    {
                        selectedUnit.PlaceOnGrassTile(hitTile.transform.position);
                    }
                    else if (tile.canGo && selectedUnit != null && selectedUnit.canCtrl)
                    {
                        // 如果Tile可走，且有选中的羊群，执行移动
                        selectedUnit.Move(hitTile.transform);

                        selectedUnit.ResetTiles();
                        selectedUnit = null;
                    }
                }
                return;
            }

            //取消选中所有元素
            if(selectedUnit != null) 
            {
                selectedUnit.ResetTiles();
                selectedUnit = null;
            }
        }
    }

/*    public void HandleCollision(List<Unit> collidingUnits)
    {
        // 随机选择一个羊群继续前进
        int randomIndex = Random.Range(0, collidingUnits.Count);
        Unit chosenUnit = collidingUnits[randomIndex];

        for (int i = 0; i < collidingUnits.Count; i++)
        {
            if (i == randomIndex)
            {
                // 随机选择的羊继续前进，不做处理
                continue;
            }
            else
            {
                // 其他的羊变成旗帜
                collidingUnits[i].hasSun = 0;
                collidingUnits[i].BecomeFlag();
            }
        }
    }

    // 示例碰撞检测，调用 HandleCollision
    void OnTriggerEnter2D(Collider2D collision)
    {
        List<Unit> collidingUnits = new List<Unit>();

        foreach (var contact in collision.contacts)
        {
            Unit unit = contact.collider.GetComponent<Unit>();
            if (unit != null)
            {
                collidingUnits.Add(unit);
            }
        }

        if (collidingUnits.Count > 1)
        {
            HandleCollision(collidingUnits);
        }
    }*/
}
