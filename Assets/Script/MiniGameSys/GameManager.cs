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

        // ʣ���6�������ѡ��
        List<Unit> allSheep = new List<Unit>();
        allSheep.AddRange(sunSheep);
        allSheep.AddRange(shadowSheep);

        // �Ƴ���ѡ�����
        allSheep.Remove(selectedSheep[0]); // �Ƴ��Ѿ�ѡ�е�������
        allSheep.Remove(selectedSheep[1]); // �Ƴ��Ѿ�ѡ�е���Ӱ��

        // ��ʣ���������ѡ6��
        for (int i = 0; i < 6; i++)
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
                if(sheep != null)
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

                        selectedUnit.ResetTiles();
                        selectedUnit = null;
                    }
                }
                return;
            }

            //ȡ��ѡ������Ԫ��
            if(selectedUnit != null) 
            {
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
