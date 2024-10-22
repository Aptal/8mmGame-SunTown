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
    public RunSheepCtrl[] runSheep;
    public GameObject[] destoryRoad; 

    public HubTile hubTile;
    public TextMeshProUGUI showCnt;
    public Unit selectedUnit;
    public Unit[] showUnits;

    [SerializeField] public Timer timer1;
    [SerializeField] public ShadowControl shadow;

    //public bool[,] linkGrass = new bool[8, 8];
    //public bool[,] linkStore = new bool[4, 4];
    //public bool[,] linkGandS = new bool[4, 8];
    public int destoryRoadpos1 = -1;
    public int destoryRoadpos2 = -1;



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
        InitTileId();
        InitArrivePos();
        // 毁坏一条路
        DestoryRoad();

        timer1.SetDuration(180).Begin();
        //linkGrass = new bool[8][];


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
        selectedSheep.Add(runSheep[Random.Range(0, runSheep.Length)]);


        // 剩余的5个随机挑选羊
        List<Unit> allSheep = new List<Unit>();
        allSheep.AddRange(sunSheep);
        allSheep.AddRange(shadowSheep);
        allSheep.AddRange(runSheep);


        // 移除已选择的羊
        allSheep.Remove(selectedSheep[0]); // 移除已经选中的阳光羊
        allSheep.Remove(selectedSheep[1]); // 移除已经选中的阴影羊
        allSheep.Remove(selectedSheep[2]); // 移除已经选中的跑跑羊


        // 从剩余的羊中挑选5个
        for (int i = 0; i < 5; i++)
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

    private void InitTileId()
    {
        for(int i = 0; i < grassTiles.Length; i++)
        {
            grassTiles[i].id = i;
        }
        for(int i = 0; i < storeTiles.Length; i++)
        {
            storeTiles[i].id = i + grassTiles.Length;
        }
        hubTile.id = grassTiles.Length + storeTiles.Length;
    }

    private void InitArrivePos()
    {
        grassTiles[0].arrivePos.Add(1);
        grassTiles[0].arrivePos.Add(3);
        grassTiles[0].arrivePos.Add(storeTiles[0].id);

        grassTiles[1].arrivePos.Add(0);
        grassTiles[1].arrivePos.Add(2);
        grassTiles[1].arrivePos.Add(storeTiles[0].id);
        grassTiles[1].arrivePos.Add(storeTiles[1].id);

        grassTiles[2].arrivePos.Add(1);
        grassTiles[2].arrivePos.Add(4);
        grassTiles[2].arrivePos.Add(storeTiles[1].id);

        grassTiles[3].arrivePos.Add(0);
        grassTiles[3].arrivePos.Add(5);
        grassTiles[3].arrivePos.Add(storeTiles[0].id);
        grassTiles[3].arrivePos.Add(storeTiles[2].id);

        grassTiles[4].arrivePos.Add(2);
        grassTiles[4].arrivePos.Add(7);
        grassTiles[4].arrivePos.Add(storeTiles[1].id);
        grassTiles[4].arrivePos.Add(storeTiles[3].id);

        grassTiles[5].arrivePos.Add(3);
        grassTiles[5].arrivePos.Add(6);
        grassTiles[5].arrivePos.Add(storeTiles[2].id);

        grassTiles[6].arrivePos.Add(5);
        grassTiles[6].arrivePos.Add(7);
        grassTiles[6].arrivePos.Add(storeTiles[2].id);
        grassTiles[6].arrivePos.Add(storeTiles[3].id);

        grassTiles[7].arrivePos.Add(4);
        grassTiles[7].arrivePos.Add(6);
        grassTiles[7].arrivePos.Add(storeTiles[3].id);

        //store
        storeTiles[storeTiles[0].id - grassTiles.Length].arrivePos.Add(grassTiles[0].id);
        storeTiles[storeTiles[0].id - grassTiles.Length].arrivePos.Add(grassTiles[1].id);
        storeTiles[storeTiles[0].id - grassTiles.Length].arrivePos.Add(grassTiles[3].id);
        storeTiles[storeTiles[0].id - grassTiles.Length].arrivePos.Add(storeTiles[1].id);
        storeTiles[storeTiles[0].id - grassTiles.Length].arrivePos.Add(storeTiles[2].id);
        storeTiles[storeTiles[0].id - grassTiles.Length].arrivePos.Add(hubTile.id);

        storeTiles[storeTiles[1].id - grassTiles.Length].arrivePos.Add(grassTiles[1].id);
        storeTiles[storeTiles[1].id - grassTiles.Length].arrivePos.Add(grassTiles[2].id);
        storeTiles[storeTiles[1].id - grassTiles.Length].arrivePos.Add(grassTiles[4].id);
        storeTiles[storeTiles[1].id - grassTiles.Length].arrivePos.Add(storeTiles[0].id);
        storeTiles[storeTiles[1].id - grassTiles.Length].arrivePos.Add(storeTiles[3].id);
        storeTiles[storeTiles[1].id - grassTiles.Length].arrivePos.Add(hubTile.id);

        storeTiles[storeTiles[2].id - grassTiles.Length].arrivePos.Add(grassTiles[3].id);
        storeTiles[storeTiles[2].id - grassTiles.Length].arrivePos.Add(grassTiles[5].id);
        storeTiles[storeTiles[2].id - grassTiles.Length].arrivePos.Add(grassTiles[6].id);
        storeTiles[storeTiles[2].id - grassTiles.Length].arrivePos.Add(storeTiles[0].id);
        storeTiles[storeTiles[2].id - grassTiles.Length].arrivePos.Add(storeTiles[3].id);
        storeTiles[storeTiles[2].id - grassTiles.Length].arrivePos.Add(hubTile.id);

        storeTiles[storeTiles[3].id - grassTiles.Length].arrivePos.Add(grassTiles[4].id);
        storeTiles[storeTiles[3].id - grassTiles.Length].arrivePos.Add(grassTiles[6].id);
        storeTiles[storeTiles[3].id - grassTiles.Length].arrivePos.Add(grassTiles[7].id);
        storeTiles[storeTiles[3].id - grassTiles.Length].arrivePos.Add(storeTiles[1].id);
        storeTiles[storeTiles[3].id - grassTiles.Length].arrivePos.Add(storeTiles[2].id);
        storeTiles[storeTiles[3].id - grassTiles.Length].arrivePos.Add(hubTile.id);

        hubTile.arrivePos.Add(storeTiles[0].id);
        hubTile.arrivePos.Add(storeTiles[1].id);
        hubTile.arrivePos.Add(storeTiles[2].id);
        hubTile.arrivePos.Add(storeTiles[3].id);

    }

    private void RemoveArrivePos(int sourcePos, int targetPos)
    {
        if(sourcePos < grassTiles.Length)
        {
            if(targetPos < grassTiles.Length)
            {
                grassTiles[sourcePos].arrivePos.Remove(targetPos);
                grassTiles[targetPos].arrivePos.Remove(sourcePos);
            }
            else
            {
                grassTiles[sourcePos].arrivePos.Remove(targetPos);
                storeTiles[targetPos - grassTiles.Length].arrivePos.Remove(sourcePos);
            }
        }
        else
        {
            if (targetPos < grassTiles.Length)
            {
                storeTiles[sourcePos - grassTiles.Length].arrivePos.Remove(targetPos);
                grassTiles[targetPos].arrivePos.Remove(sourcePos);
            }
            else
            {
                storeTiles[sourcePos - grassTiles.Length].arrivePos.Remove(targetPos);
                storeTiles[targetPos - grassTiles.Length].arrivePos.Remove(sourcePos);
            }
        }
    }

    private void DestoryRoad()
    {
        int desRoadIndex = Random.Range(0, 24);
        switch (desRoadIndex)
        {
            case 0:
                destoryRoadpos1 = 0; destoryRoadpos2 = 1; break;
            case 1:
                destoryRoadpos1 = 1; destoryRoadpos2 = 2; break;
            case 2:
                destoryRoadpos1 = 0; destoryRoadpos2 = 3; break;
            case 3:
                destoryRoadpos1 = 0; destoryRoadpos2 = 8; break;
            case 4:
                destoryRoadpos1 = 1; destoryRoadpos2 = 8; break;
            case 5:
                destoryRoadpos1 = 1; destoryRoadpos2 = 9; break;
            case 6:
                destoryRoadpos1 = 2; destoryRoadpos2 = 9; break;
            case 7:
                destoryRoadpos1 = 2; destoryRoadpos2 = 4; break;
            case 8:
                destoryRoadpos1 = 3; destoryRoadpos2 = 8; break;
            case 9:
                destoryRoadpos1 = 8; destoryRoadpos2 = 9; break;
            case 10:
                destoryRoadpos1 = 4; destoryRoadpos2 = 9; break;
            case 11:
                destoryRoadpos1 = 3; destoryRoadpos2 = 5; break;
            case 12:
                destoryRoadpos1 = 3; destoryRoadpos2 = 10; break;
            case 13:
                destoryRoadpos1 = 8; destoryRoadpos2 = 10; break;
            case 14:
                destoryRoadpos1 = 9; destoryRoadpos2 = 11; break;
            case 15:
                destoryRoadpos1 = 4; destoryRoadpos2 = 11; break;
            case 16:
                destoryRoadpos1 = 4; destoryRoadpos2 = 7; break;
            case 17:
                destoryRoadpos1 = 5; destoryRoadpos2 = 10; break;
            case 18:
                destoryRoadpos1 = 11; destoryRoadpos2 = 10; break;
            case 19:
                destoryRoadpos1 = 11; destoryRoadpos2 = 7; break;
            case 20:
                destoryRoadpos1 = 10; destoryRoadpos2 = 6; break;
            case 21:
                destoryRoadpos1 = 11; destoryRoadpos2 = 6; break;
            case 22:
                destoryRoadpos1 = 5; destoryRoadpos2 = 6; break;
            case 23:
                destoryRoadpos1 = 6; destoryRoadpos2 = 7; break;
            default:
                break;
        }
        if(destoryRoadpos1 > destoryRoadpos2)
        {
            int temp = destoryRoadpos1;
            destoryRoadpos1 = destoryRoadpos2;
            destoryRoadpos2 = temp;
        }
        RemoveArrivePos(destoryRoadpos1, destoryRoadpos2);

        destoryRoad[desRoadIndex].SetActive(true);
    }

    private void RepairRoad()
    {
        destoryRoadpos1 = -1;
        destoryRoadpos2 = -1;
    }

    private bool IsDestoryRoad(Vector3 targetPos)
    {
        Debug.Log(destoryRoadpos1 + " " + destoryRoadpos2);
        if(destoryRoadpos1 < 0 || destoryRoadpos2 < 0)  return false;
        int cnt = 0;
        if (destoryRoadpos1 > 7)
        {
            destoryRoadpos1 -= 8;
            if(storeTiles[destoryRoadpos1].transform.position == targetPos ||
               storeTiles[destoryRoadpos1].transform.position == selectedUnit.transform.position)
                cnt++;
            destoryRoadpos1 += 8;
        }   
        else
        {
            if(grassTiles[destoryRoadpos1].transform.position == targetPos ||
               grassTiles[destoryRoadpos1].transform.position == selectedUnit.transform.position)
                cnt++;
        }
        if (destoryRoadpos2 > 7)
        {
            destoryRoadpos2 -= 8;
            if (storeTiles[destoryRoadpos2].transform.position == targetPos ||
               storeTiles[destoryRoadpos2].transform.position == selectedUnit.transform.position)
                cnt++;
            destoryRoadpos2 += 8;
        }
        else
        {
            if (grassTiles[destoryRoadpos2].transform.position == targetPos ||
               grassTiles[destoryRoadpos2].transform.position == selectedUnit.transform.position)
                cnt++;
        }
        Debug.Log("cnt : "+cnt);
        return cnt == 2;
    }

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
                if(sheep != null && !sheep.isSelected)
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
                        //selectedUnit.isSelected = false;
                        selectedUnit.ResetTiles();
                        selectedUnit = null;
                    }
                }
                return;
            }

            //取消选中所有元素
            if(selectedUnit != null) 
            {
                selectedUnit.isSelected = false;
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
