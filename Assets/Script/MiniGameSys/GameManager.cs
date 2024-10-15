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
    public HubTile hubTile;
    public TextMeshProUGUI showCnt;
    public Unit selectedUnit;

    [SerializeField] Timer timer1;
    [SerializeField] ShadowControl shadow;

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
                if(sheep != null)
                {
                    sheep.SelectSheep();
                    Debug.Log("sheep");

                }
                return; // �ɹ�ѡ����Ⱥ�󣬲��������
            }

            // ���Tile�㣨�ݵصȣ�
            RaycastHit2D hitTile = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));
            if (hitTile.collider != null)
            {
                Tile tile = hitTile.collider.GetComponent<Tile>();
                if(tile != null)
                {
                    if (tile.canGo && selectedUnit != null && selectedUnit.canCtrl)
                    {
                        // ���Tile���ߣ�����ѡ�е���Ⱥ��ִ���ƶ�
                        selectedUnit.Move(hitTile.transform);
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
}
