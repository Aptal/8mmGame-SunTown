using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{

    public bool DeleteFile(string path)
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            Debug.LogError("删除文件失败，没有足够权限访问文件路径：" + path);
            return false;
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("删除文件失败，文件不存在：" + path);
            return false;
        }
    }

    public bool CreateOrOpenFile(string path, string name, string info)
    {
        try
        {
            StreamWriter sw;
            FileInfo fi = new FileInfo(path + "//" + name);

            // rewrite info, add info can use fi.AppendText()
            sw = fi.CreateText();

            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            Debug.LogError("创建或打开文件失败，没有足够权限访问文件路径：" + path);
            return false;
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("创建或打开文件失败，文件不存在：" + path);
            return false;
        }
    }

    private string LogMainGameData()
    {
        string info = "";
        TimeControl timeInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<TimeControl>();
        SunControl sunInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<SunControl>();
        SheepLevelControl sheepInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<SheepLevelControl>();
        StoreLevelControl storeInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<StoreLevelControl>();
        HubLevelControl hubInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<HubLevelControl>();
        HappyControl happyInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<HappyControl>();
        FaithControl faithInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<FaithControl>();
        //TimeControl timeInfo = TimeControl.Instance;
        //SunControl sunInfo = TimeControl.Instance.sunCtrl;
        //SheepLevelControl sheepInfo = TimeControl.Instance.sheepLevelCtrl;
        //StoreLevelControl storeInfo = TimeControl.Instance.storeLevelCtrl;
        //HubLevelControl hubInfo = TimeControl.Instance.hubLevelCtrl;
        //HappyControl happyInfo = TimeControl.Instance.happyCtrl;
        //FaithControl faithInfo = TimeControl.Instance.faithCtrl;


        MainTimeData timeData = new MainTimeData(timeInfo.dayCnt, timeInfo.hasMiniGame, timeInfo.eventOutcome, timeInfo.researchOutcome, timeInfo.eventIndex, timeInfo.hasShadowSheep, timeInfo.hasRunSheep, timeInfo.hasBuilding, timeInfo.hasEvent);
        MainSunData sunData = new MainSunData(sunInfo.totalSun, sunInfo.preSun);
        Debug.Log("info level " + sheepInfo.sunLimitLevel);
        MainSheepData sheepData = new MainSheepData(sheepInfo.moveSpeed, sheepInfo.moveSpeedLevel, 
                                                    sheepInfo.sunLimit, sheepInfo.sunLimitLevel, 
                                                    sheepInfo.productSpeed, sheepInfo.productSpeedLevel);
        MainStoreData storeData = new MainStoreData(storeInfo.storeLimit, storeInfo.storeLimitLevel,
                                                    storeInfo.pushSpeed, storeInfo.pushSpeedLevel,
                                                    storeInfo.popSpeed, storeInfo.popSpeedLevel);
        MainHubData hubData = new MainHubData(hubInfo.sheep2hubSpeed, hubInfo.hubSpeedLevel);
        MainHappyData happyData = new MainHappyData(happyInfo.happyValue);
        MainFaithData faithData = new MainFaithData(faithInfo.faithValue);
        

        // convert
        info = JsonUtility.ToJson(timeData) + "\n\n";
        info += JsonUtility.ToJson(sunData) + "\n\n";
        info += JsonUtility.ToJson(sheepData) + "\n\n";
        info += JsonUtility.ToJson(storeData) + "\n\n";
        info += JsonUtility.ToJson(hubData) + "\n\n";
        info += JsonUtility.ToJson(happyData) + "\n\n";
        info += JsonUtility.ToJson(faithData);
        return info;
    }

    //save data once
    public void SaveData1()
    {
        // create file before continue Game
        string path = "Assets/Resources/UpdateData/SaveData";
        string name = "Data1.txt";
        string info = LogMainGameData();

        // delete file
        bool deleteSuccess = DeleteFile(path + name);

        // create file
        bool createSuccess = CreateOrOpenFile(path, name, info);

        if (deleteSuccess && createSuccess)
        {
            //SceneManager.LoadScene(2);
            Debug.Log("record success");
        }
        else
        {
            Debug.LogError("在继续游戏之前，文件操作出现问题，请检查！");
        }
    }
}
