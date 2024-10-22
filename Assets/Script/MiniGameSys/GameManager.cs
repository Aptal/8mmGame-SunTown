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
        // �ٻ�һ��·
        DestoryRoad();

        timer1.SetDuration(180).Begin();
        //linkGrass = new bool[8][];


        // ��ʼ�����λ��
        PlaceSheepOnGrass();
    }

    // ���������䵽�ݵ���
    private void PlaceSheepOnGrass()
    {
        // �����ݵص������б��ݵ�������8
        List<int> grassIndices = new List<int>();
        for (int i = 0; i < grassTiles.Length; i++)
        {
            grassIndices.Add(i);
        }

        // ʹ��Fisher-Yates�㷨���Ҳݵ������б�
        for (int i = grassIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = grassIndices[i];
            grassIndices[i] = grassIndices[randomIndex];
            grassIndices[randomIndex] = temp;
        }

        // ����ѡһֻ�������һֻ��Ӱ��
        List<Unit> selectedSheep = new List<Unit>();
        selectedSheep.Add(sunSheep[Random.Range(0, sunSheep.Length)]);
        selectedSheep.Add(shadowSheep[Random.Range(0, shadowSheep.Length)]);
        selectedSheep.Add(runSheep[Random.Range(0, runSheep.Length)]);


        // ʣ���5�������ѡ��
        List<Unit> allSheep = new List<Unit>();
        allSheep.AddRange(sunSheep);
        allSheep.AddRange(shadowSheep);
        allSheep.AddRange(runSheep);


        // �Ƴ���ѡ�����
        allSheep.Remove(selectedSheep[0]); // �Ƴ��Ѿ�ѡ�е�������
        allSheep.Remove(selectedSheep[1]); // �Ƴ��Ѿ�ѡ�е���Ӱ��
        allSheep.Remove(selectedSheep[2]); // �Ƴ��Ѿ�ѡ�е�������


        // ��ʣ���������ѡ5��
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, allSheep.Count);
            selectedSheep.Add(allSheep[randomIndex]);
            allSheep.RemoveAt(randomIndex); // �Ƴ���ѡ�����
        }

        // ������䵽����˳��Ĳݵ���
        for (int i = 0; i < selectedSheep.Count; i++)
        {
            Unit sheep = selectedSheep[i];
            int grassIndex = grassIndices[i];
            // �����λ������Ϊ��Ӧ�Ĳݵ�λ��
            sheep.gameObject.SetActive(true);
            sheep.transform.position = grassTiles[grassIndex].transform.position;
            // ȡ��ѡ��״̬
            //sheep.ResetTiles();
        }
    }
    /*private void PlaceSheepOnGrass()
    {
        // �����ݵص������б��ݵ�������8
        List<int> grassIndices = new List<int>();
        for (int i = 0; i < grassTiles.Length; i++)
        {
            grassIndices.Add(i);
        }

        // ʹ��Fisher-Yates�㷨���Ҳݵ������б�
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
            allSunSheep.RemoveAt(randomIndex); // �Ƴ���ѡ�����
        }

        for (int i = 0; i < shadowSheepCnt; i++)
        {
            int randomIndex = Random.Range(0, allShadowSheep.Count);
            allShadowSheep[randomIndex].gameObject.SetActive(true);
            allShadowSheep[randomIndex].transform.position = grassTiles[grassIndices[grassIndex++]].transform.position;
            allShadowSheep.RemoveAt(randomIndex); // �Ƴ���ѡ�����
        }

*//*        // ������䵽����˳��Ĳݵ���
        for (int i = 0; i < selectedSheep.Count; i++)
        {
            Unit sheep = selectedSheep[i];
            //int grassIndex = grassIndices[i];
            // �����λ������Ϊ��Ӧ�Ĳݵ�λ��
            sheep.gameObject.SetActive(true);
            sheep.transform.position = grassTiles[i].transform.position;
            // ȡ��ѡ��״̬
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

            // �ȼ����Ⱥ���ڵĲ�
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
                return; // �ɹ�ѡ����Ⱥ�󣬲��������
            }

            // ���Tile�㣨�ݵ�/�ֿ�ȣ�
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
                        // ���Tile���ߣ�����ѡ�е���Ⱥ��ִ���ƶ�
                        selectedUnit.Move(hitTile.transform);
                        //selectedUnit.isSelected = false;
                        selectedUnit.ResetTiles();
                        selectedUnit = null;
                    }
                }
                return;
            }

            //ȡ��ѡ������Ԫ��
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
        // ���ѡ��һ����Ⱥ����ǰ��
        int randomIndex = Random.Range(0, collidingUnits.Count);
        Unit chosenUnit = collidingUnits[randomIndex];

        for (int i = 0; i < collidingUnits.Count; i++)
        {
            if (i == randomIndex)
            {
                // ���ѡ��������ǰ������������
                continue;
            }
            else
            {
                // ��������������
                collidingUnits[i].hasSun = 0;
                collidingUnits[i].BecomeFlag();
            }
        }
    }

    // ʾ����ײ��⣬���� HandleCollision
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
