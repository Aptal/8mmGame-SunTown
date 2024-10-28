using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadMainGameData : MonoBehaviour
{
    private TextAsset loadData;

    private MainTimeData timeData;
    private MainSunData sunData;
    private MainSheepData sheepData;
    private MainStoreData storeData;
    private MainHubData hubData;
    private MainHappyData happyData;
    private MainFaithData faithData;

    public void LoadData()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        loadData = Resources.Load<TextAsset>("UpdateData/SaveData/Data1");
        if (loadData != null)
        {
            string[] dataStringList = loadData.text.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);
            timeData = JsonUtility.FromJson<MainTimeData>(dataStringList[0]);
            sunData = JsonUtility.FromJson<MainSunData>(dataStringList[1]);
            sheepData = JsonUtility.FromJson<MainSheepData>(dataStringList[2]);
            storeData = JsonUtility.FromJson<MainStoreData>(dataStringList[3]);
            hubData = JsonUtility.FromJson<MainHubData>(dataStringList[4]);
            happyData = JsonUtility.FromJson<MainHappyData>(dataStringList[5]);
            faithData = JsonUtility.FromJson<MainFaithData>(dataStringList[6]);

            setValue();
        }
        else
        {
            Debug.Log("no load data");
        }
#else
        string path = Path.Combine(Application.persistentDataPath, "Data1.txt");

        if (File.Exists(path))
        {
            try
            {
                // ��ȡ�ļ�����
                string loadData = File.ReadAllText(path);

                // �����ݷָ�Ϊ��� JSON ����
                string[] dataStringList = loadData.Split(new string[] { "\n\n" }, StringSplitOptions.None);

                // �����л�Ϊ��Ӧ�����ݽṹ
                timeData = JsonUtility.FromJson<MainTimeData>(dataStringList[0]);
                sunData = JsonUtility.FromJson<MainSunData>(dataStringList[1]);
                sheepData = JsonUtility.FromJson<MainSheepData>(dataStringList[2]);
                storeData = JsonUtility.FromJson<MainStoreData>(dataStringList[3]);
                hubData = JsonUtility.FromJson<MainHubData>(dataStringList[4]);
                happyData = JsonUtility.FromJson<MainHappyData>(dataStringList[5]);
                faithData = JsonUtility.FromJson<MainFaithData>(dataStringList[6]);

                // ���ö�ȡ��������
                setValue();
            }
            catch (Exception e)
            {
                Debug.LogError("��ȡ������ļ�ʧ��: " + e.Message);
            }
        }
        else
        {
            Debug.Log("δ�ҵ��浵�ļ���·��: " + path);
        }
#endif

    }

    public void setValue()
    {
        if (TimeControl.Instance != null)
        {
            if (timeData == null)
            {
                Debug.LogError("timeData");
                return;
            }
            TimeControl.Instance.dayCnt = timeData.dayCnt;
            TimeControl.Instance.hasMiniGame = timeData.hasMiniGame;
            TimeControl.Instance.eventOutcome = timeData.eventOutcome;
            TimeControl.Instance.researchOutcome = timeData.researchOutcome;
            TimeControl.Instance.eventIndex = timeData.eventIndex;
            Debug.Log("load data: " + timeData.eventIndex);
            TimeControl.Instance.hasShadowSheep = timeData.hasShadowSheep;
            TimeControl.Instance.hasRunSheep = timeData.hasRunSheep;
            TimeControl.Instance.hasBuilding = timeData.hasBuilding;
            TimeControl.Instance.hasEvent = timeData.hasEvent;


            TimeControl.Instance.sunCtrl.totalSun = sunData.totalSun;
            TimeControl.Instance.sunCtrl.preSun = sunData.preSun;


            TimeControl.Instance.sheepLevelCtrl.moveSpeed = sheepData.moveSpeed;
            TimeControl.Instance.sheepLevelCtrl.moveSpeedLevel = sheepData.moveSpeedLevel;
            TimeControl.Instance.sheepLevelCtrl.sunLimit = sheepData.sunLimit;
            TimeControl.Instance.sheepLevelCtrl.sunLimitLevel = sheepData.sunLimitLevel;
            TimeControl.Instance.sheepLevelCtrl.productSpeed = sheepData.productSpeed;
            TimeControl.Instance.sheepLevelCtrl.productSpeedLevel = sheepData.productSpeedLevel;

            TimeControl.Instance.storeLevelCtrl.storeLimit = storeData.storeLimit;
            TimeControl.Instance.storeLevelCtrl.storeLimitLevel = storeData.storeLimitLevel;
            TimeControl.Instance.storeLevelCtrl.pushSpeed = storeData.pushSpeed;
            TimeControl.Instance.storeLevelCtrl.pushSpeedLevel = storeData.pushSpeedLevel;
            TimeControl.Instance.storeLevelCtrl.popSpeed = storeData.popSpeed;
            TimeControl.Instance.storeLevelCtrl.popSpeedLevel = storeData.popSpeedLevel;

            TimeControl.Instance.hubLevelCtrl.sheep2hubSpeed = hubData.sheep2hubSpeed;
            TimeControl.Instance.hubLevelCtrl.hubSpeedLevel = hubData.hubSpeedLevel;

            TimeControl.Instance.happyCtrl.happyValue = happyData.happyValue;

            TimeControl.Instance.faithCtrl.faithValue = faithData.faithValue;

        }
        else
        {
            Debug.LogError("continue game init failed : can't find Instance");
        }
    }
}
