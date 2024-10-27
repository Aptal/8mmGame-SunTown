using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyControl : MonoBehaviour
{
    public TextMeshProUGUI[] sheepCnt;
    public Image[] sheepImg;
    public Button[] badRoadButtons;
    public Button roadFixButton;

    public Image weatherImg;
    public Sprite[] weatherSprite;

    public bool isFixing = false;

    private void Start()
    {
    }

    void InitButton()
    {
        // 获取所有特定名称开头的按钮并存储到数组中
        GameObject mapObject = GameObject.Find("image_minigameMap");
        if (mapObject != null)
        {
            badRoadButtons = new Button[24];
            for (int i = 0; i < 24; i++)
            {
                string buttonName = $"button_BadRoad_{i}";
                GameObject buttonObject = mapObject.transform.Find(buttonName)?.gameObject;
                if (buttonObject != null)
                {
                    badRoadButtons[i] = buttonObject.GetComponent<Button>();
                    badRoadButtons[i].gameObject.SetActive(false);
                }
            }
        }
        roadFixButton = GameObject.Find("button_roadFix")?.gameObject.GetComponent<Button>();
        roadFixButton.interactable = TimeControl.Instance.canFix;
        isFixing = false;
    }

    void BadRoadButtonClicked(int buttonIndex)
    {
        if (isFixing && !TimeControl.Instance.roadCtrl.hasFix)
        {
            TimeControl.Instance.roadCtrl.FixRoad(buttonIndex);
            badRoadButtons[buttonIndex].gameObject.SetActive(false);
        }
    }

    public void FixButton()
    {
        if (!isFixing)
        {
            isFixing = true;
            roadFixButton.interactable = false;
        }
        else
        {
            roadFixButton.interactable = false;
        }
    }

    public void GotoMiniGame()
    {
        //InitMiniGameData();
    }

    public void ReadyCanvasButton()
    {
        InitButton();
        // show sheep
        for (int i = 0; i < 3; ++i)
        {
            if(TimeControl.Instance.sheepCnt[i] > 0)
            {
                sheepCnt[i].text = TimeControl.Instance.sheepCnt[i].ToString();
            }
            else
            {
                sheepImg[i].gameObject.SetActive(false);
            }
        }

        // show road
        TimeControl.Instance.roadCtrl.badRoadCnt = 2;
        TimeControl.Instance.roadCtrl.AddRoad();
        foreach (var road in TimeControl.Instance.roadCtrl.RoadList)
        {
            Debug.Log(road.ID);
            badRoadButtons[road.ID].gameObject.SetActive(true);
            badRoadButtons[road.ID].onClick.AddListener(() => BadRoadButtonClicked(road.ID));
        }

        weatherImg.sprite = weatherSprite[TimeControl.Instance.weatherType];


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
        SheepLevelControl sheepInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<SheepLevelControl>();
        StoreLevelControl storeInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<StoreLevelControl>();
        HubLevelControl hubInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<HubLevelControl>();
        RoadControl roadInfo = GameObject.FindGameObjectWithTag("dataInfo").GetComponent<RoadControl>();

        UnitData unitData = new UnitData(sheepInfo.moveSpeed, (int)(sheepInfo.productSpeed * TimeControl.Instance.weatherK + 0.5f), sheepInfo.sunLimit, TimeControl.Instance.sheepCnt);
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

    public void InitMiniGameData()
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
}
