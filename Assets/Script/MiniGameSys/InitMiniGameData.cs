using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        UnityEditor.AssetDatabase.Refresh();
        loadData = Resources.Load<TextAsset>("UpdateData/MoveSence/MoveMiniGame");
        if (loadData != null)
        {
            string[] dataStringList = loadData.text.Split(new string[] {"\n\n"}, System.StringSplitOptions.None);
            unitData = JsonUtility.FromJson<UnitData>(dataStringList[0]);
            storeData = JsonUtility.FromJson<StoreData>(dataStringList[1]);
            hubData = JsonUtility.FromJson<HubData>(dataStringList[2]);
            roadData = JsonUtility.FromJson<RoadData>(dataStringList[3]);
        }
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
            GameManager.Instance.badRoadList = roadData.RoadList;
        }
        else
        {
            Debug.LogError("minigame init failed : can't find GameManager Instance");
        }
    }
}
