using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class InitMiniGameData : MonoBehaviour
{
    private TextAsset loadData;

    private UnitData unitData;
    private StoreData storeData;
    private HubData hubData;
    private RoadData roadData;
    public int[] sheepCnt = { 8, 0, 0};

    private void Awake()
    {
        //LoadData();
    }

    public void LoadData()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        loadData = Resources.Load<TextAsset>("UpdateData/MoveSence/MoveMiniGame");
        if (loadData != null)
        {
            string[] dataStringList = loadData.text.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);
            unitData = JsonUtility.FromJson<UnitData>(dataStringList[0]);
            storeData = JsonUtility.FromJson<StoreData>(dataStringList[1]);
            hubData = JsonUtility.FromJson<HubData>(dataStringList[2]);
            roadData = JsonUtility.FromJson<RoadData>(dataStringList[3]);
        }
#else
        string path = Path.Combine(Application.persistentDataPath, "MoveMiniGame.txt");

        if (File.Exists(path))
        {
            try
            {
                string loadData = File.ReadAllText(path);
                string[] dataStringList = loadData.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

                // Deserialize each segment of JSON data
                unitData = JsonUtility.FromJson<UnitData>(dataStringList[0]);
                storeData = JsonUtility.FromJson<StoreData>(dataStringList[1]);
                hubData = JsonUtility.FromJson<HubData>(dataStringList[2]);
                roadData = JsonUtility.FromJson<RoadData>(dataStringList[3]);
            }
            catch (System.Exception e)
            {
                Debug.LogError("读取或解析文件失败: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("数据文件未找到，路径: " + path);
        }
#endif

    }

    public void setValue()
    {
        if(GameManager.Instance != null)
        {
            if (unitData == null)
            {
                Debug.LogError("unitdata");
                return;
            }
            sheepCnt = unitData.sheepCnt;

            foreach (var sheep in GameManager.Instance.sunSheep)
            {
                sheep.moveSpeed = unitData.moveSpeed;
                sheep.productV = unitData.productV;
                sheep.sunLimit = unitData.sunLimit;
            }

            foreach (var sheep in GameManager.Instance.shadowSheep)
            {
                sheep.moveSpeed = unitData.moveSpeed;
                sheep.productV = unitData.productV;
                sheep.sunLimit = unitData.sunLimit;
            }

            foreach (var sheep in GameManager.Instance.runSheep)
            {
                sheep.moveSpeed = unitData.moveSpeed;
                sheep.productV = unitData.productV;
                sheep.sunLimit = unitData.sunLimit;
            }

            if (storeData == null)
            {
                Debug.LogError("storeData");
                return;
            }
            foreach (var store in GameManager.Instance.storeTiles)
            {
                store.stSunLimit = storeData.storeLimit;
                store.sheep2storeV = storeData.pushSpeed;
                store.store2sheepV = storeData.popSpeed;
            }

            if (hubData == null) return;
            GameManager.Instance.hubTile.sheep2hubV = hubData.sheep2hubSpeed;
            if (roadData == null) return;
            GameManager.Instance.badRoadList = new List<(int ID, int U, int V)> ();
            for(int i = 0; i < roadData.cnt; ++i)
            {
                GameManager.Instance.badRoadList.Add((roadData.ID[i], roadData.U[i], roadData.V[i]));
            }
            //GameManager.Instance.badRoadList = roadData.RoadList;
        }
        else
        {
            Debug.LogError("minigame init failed : can't find GameManager Instance");
        }
    }
}
