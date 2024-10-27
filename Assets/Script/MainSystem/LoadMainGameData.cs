using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            TimeControl.Instance.sunCtrl.totalSun = sunData.totalSun;

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
