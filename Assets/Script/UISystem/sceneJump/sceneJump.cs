using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;
using System;

public class sceneJump : MonoBehaviour
{
    //ͨ�������ťʵ�ֽ�����ת
    public void JumpToMainScene()
    {
        LoadInitData();
        SceneManager.LoadScene(1);
    }

    public void JumpToMenu()
    {
        SceneManager.LoadScene(0);
    }

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

    private string LogMiniGameData()
    {
        string info = "";
        SheepLevelControl sheepInfo = TimeControl.Instance.sheepLevelCtrl;
        StoreLevelControl storeInfo = TimeControl.Instance.storeLevelCtrl;
        HubLevelControl hubInfo = TimeControl.Instance.hubLevelCtrl;
        RoadControl roadInfo = TimeControl.Instance.roadCtrl;

        UnitData unitData = new UnitData(sheepInfo.moveSpeed, (int) (sheepInfo.productSpeed * TimeControl.Instance.weatherK + 0.5f), sheepInfo.sunLimit, TimeControl.Instance.sheepCnt);
        StoreData storeData = new StoreData(storeInfo.storeLimit, storeInfo.pushSpeed, storeInfo.popSpeed);
        HubData hubData = new HubData(hubInfo.sheep2hubSpeed);
        RoadData roadData = new RoadData(roadInfo.RoadList);

        // convert
        info = JsonUtility.ToJson(unitData) + "\n\n";
        info += JsonUtility.ToJson(storeData) + "\n\n";
        info += JsonUtility.ToJson(hubData) + "\n\n";
        info += JsonUtility.ToJson(roadData);
        return info;
    }

    private string InitMainGameData()
    {
        string info = "";
        MainTimeData timeData = new MainTimeData(1);
        MainSunData sunData = new MainSunData(200);
        MainSheepData sheepData = new MainSheepData(0.2f, 0,
                                                    10, 0,
                                                    4, 0);
        MainStoreData storeData = new MainStoreData(10, 0,
                                                    4, 0,
                                                    4, 0);
        MainHubData hubData = new MainHubData(12, 0);
        MainHappyData happyData = new MainHappyData(25);
        MainFaithData faithData = new MainFaithData(25);

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
    
    private void LoadInitData()
    {
        string path = "Assets/Resources/UpdateData/SaveData";
        string name = "Data1.txt";
        string info = InitMainGameData();

        // delete file
        bool deleteSuccess = DeleteFile(path + name);

        // create file
        bool createSuccess = CreateOrOpenFile(path, name, info);

        if (deleteSuccess && createSuccess)
        {
            //SceneManager.LoadScene(2);
        }
        else
        {
            Debug.LogError("init文件操作出现问题，请检查！");
        }
    }

    // button click
    public void JumpToMiniGame()
    {
        string path = "Assets/Resources/UpdateData/MoveSence";
        string name = "MoveMiniGame.txt";
        string info = LogMiniGameData();

        // delete file
        bool deleteSuccess = DeleteFile(path + name);

        // create file
        bool createSuccess = CreateOrOpenFile(path, name, info);
        
        if (deleteSuccess && createSuccess)
        {
            //SceneManager.LoadScene(2);
        }
        else
        {
            Debug.LogError("在跳转到迷你游戏场景之前，文件操作出现问题，请检查！");
        }
    }
    // 动画事件调用的函数
    public void JumpToMiniGameLater()
    {
        JumpToMiniGame();
        StartCoroutine(LoadSceneAfterDelay());
    }
    // 协程用于延迟场景加载
    private System.Collections.IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}
