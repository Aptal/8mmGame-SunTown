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
    //public Unit[] showUnits;

    public Timer timer1;
    public ShadowControl shadow;
    public InitMiniGameData initData;

    private AudioClip gameSound;
    private AudioClip envSound;
    [Range(0.0f, 1.0f)] // 在编辑器中添加滑块来调整音量
    private float envSoundVolume = 1f; // 移动声音的默认音量

    [SerializeField] private AudioClip clickNothingSound;
    [SerializeField] private AudioClip sunTotalSound;

    private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceGame;

    public List<(int ID, int U, int V)> badRoadList;
    //public int destoryRoadpos1 = -1;
    //public int destoryRoadpos2 = -1;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            initData = gameObject.GetComponent<InitMiniGameData>();
            if (initData != null)
            {
                initData.LoadData();
                initData.setValue();
            }   
            else
            {
                Debug.LogError("data pass failed");
            }
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
        audioSource = GetComponent<AudioSource>();
        // 播放移动音频并循环
        audioSource.clip = envSound;
        audioSource.loop = true; // 设置为循环播放
        audioSource.volume = envSoundVolume;
        audioSource.Play(); // 开始播放音频

        InitTileId();
        InitArrivePos();


        // 毁坏一条路
        DestoryRoad();

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

        List<Unit> selectedSheep = new List<Unit>();

        List<Unit> allSheep = new List<Unit>();
        allSheep.AddRange(sunSheep);
        for(int i = 0; i < initData.sheepCnt[0]; ++i)
        {
            int randomIndex = Random.Range(0, allSheep.Count);
            selectedSheep.Add(allSheep[randomIndex]);
            allSheep.RemoveAt(randomIndex);
        }
        allSheep.Clear();

        allSheep.AddRange(shadowSheep);
        for (int i = 0; i < initData.sheepCnt[1]; ++i)
        {
            int randomIndex = Random.Range(0, allSheep.Count);
            selectedSheep.Add(allSheep[randomIndex]);
            allSheep.RemoveAt(randomIndex);
        }
        allSheep.Clear();

        allSheep.AddRange(runSheep);
        for (int i = 0; i < initData.sheepCnt[2]; ++i)
        {
            int randomIndex = Random.Range(0, allSheep.Count);
            selectedSheep.Add(allSheep[randomIndex]);
            allSheep.RemoveAt(randomIndex);
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
        if (badRoadList == null) return;
        foreach(var badRoad in badRoadList)
        {
            int desRoadIndex = badRoad.ID;
            int destoryRoadpos1 = badRoad.U;
            int destoryRoadpos2 = badRoad.V;
            RemoveArrivePos(destoryRoadpos1, destoryRoadpos2);
            destoryRoad[desRoadIndex].SetActive(true);
        }
    }

    /*private bool IsDestoryRoad(Vector3 targetPos)
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
                    if(selectedUnit != null)
                    {
                        resetSelectUnit();
                    }
                    selectedUnit = sheep;
                    selectedUnit.spriteRenderer.color = selectedUnit.selectedColor;
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
                    //点击仓库声音
                    StoreTile stTile = hitTile.collider.GetComponent<StoreTile>();
                    if (stTile != null && stTile.opt == StoreOpt.disable)
                    {
                        audioSource.PlayOneShot(stTile.noWorkSound);
                    }

                    if(selectedUnit != null && selectedUnit.isFlag)
                    {
                        if(hitTile.collider.GetComponent<GrassTile>())
                            selectedUnit.PlaceOnGrassTile(hitTile.transform.position);
                    }
                    else if (tile.canGo && selectedUnit != null && selectedUnit.canCtrl)
                    {
                        // 如果Tile可走，且有选中的羊群，执行移动
                        // 
                        if(selectedUnit.curType == PosType.store)
                        {
                            storeTiles[selectedUnit.posIndex].hasSheep = false;
                        }
                        selectedUnit.Move(hitTile.transform);
                        //selectedUnit.isSelected = false;
                        selectedUnit.ResetTiles();
                        selectedUnit = null;
                    }
                }
                return;
            }

            audioSource.PlayOneShot(clickNothingSound);
            //取消选中所有元素
            if(selectedUnit != null) 
            {
                resetSelectUnit();
            }
        }
    }

    void resetSelectUnit()
    {
        selectedUnit.isSelected = false;
        selectedUnit.ResetTiles();
        selectedUnit = null;
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
