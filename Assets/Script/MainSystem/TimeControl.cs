using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeControl : MonoBehaviour
{
    private static TimeControl _instance;
    public static TimeControl Instance
    {
        get
        {
            if (_instance == null)
            {
                // �ڳ����в���GameDataManager��������û���򴴽�һ���µ�
                _instance = FindObjectOfType<TimeControl>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameDataManager");
                    _instance = obj.AddComponent<TimeControl>();
                }
            }
            return _instance;
        }
    }

    public FaithControl faithCtrl;
    public HappyControl happyCtrl;
    public HubLevelControl hubLevelCtrl;
    public StoreLevelControl storeLevelCtrl;
    public SheepLevelControl sheepLevelCtrl;
    public SunControl sunCtrl;
    public RoadControl roadCtrl;

    public LoadMainGameData initMainData;
    public SaveData saveMainData;

    public TextMeshProUGUI dayText;
    public int dayCnt = -1;

    private bool hasShadowSheep = false;
    private bool hasRunSheep = false;
    public int[] sheepCnt = new int[3];

    // weather
    public int weatherType = 0;
    public float weatherK = 1.0f;
    public Image weatherImg;
    public Sprite[] weatherSprites;

    public Button[] buildingButton;
    private bool[] hasBuilding = new bool[3];

    public bool canFix = false;
    public bool hasMiniGame = true;

    public int eventOutcome;
    public int researchOutcome;


    private void Awake()
    {
        Debug.Log("awake main");
        if(Instance != null)
        {
            initMainData = gameObject.GetComponent<LoadMainGameData>();
            if (initMainData != null)
            {
                initMainData.LoadData();
            }
            else
            {
                Debug.LogError("main game load error");
            }
        }
    }

    private void Start()
    {
        Debug.Log("start main");
        hasBuilding[0] = true;
        hasBuilding[1] = true;
        hasBuilding[2] = true;
        
        int minigameSun = PlayerPrefs.GetInt("MiniGameGotSun");
        if (minigameSun >= 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        sunCtrl.sunIncome += minigameSun;
        sunCtrl.sunOutcome = minigameSun / 10;

        UpdateUI();
    }

    public void NewDay()
    {
        if (dayCnt == -1) dayCnt = 1;
        else dayCnt++;
        hasMiniGame = true;


        // �¼��ж�

        UpdateUI();

    }

    public void SaveMainData()
    {
        saveMainData.SaveData1();
    }

    public void UpdateUI()
    {
        UpdateDay();
        UpdateBuilding();
        faithCtrl.SliderCtrl();
        happyCtrl.SliderCtrl();

        UpdateSheepCnt();
        UpdateWeather();
    }

    public void UpdateDay()
    {
        dayText.text = dayCnt.ToString();
    }

    private void UpdateBuilding()
    {
        for (int i = 0; i < 3; ++i)
        {
            buildingButton[i].interactable = hasBuilding[i];
        }
    }

    public void UpdateWeather()
    {
        if (weatherType == 0)
        {
            weatherK = 1.0f;
        }
        else if(weatherType == 1)
        {
            weatherK = 0.75f;
        }
        else
        {
            weatherK = 0.5f;
        }
        weatherImg.sprite = weatherSprites[weatherType];
    }

    public void UpdateSheepCnt()
    {
        if (!hasRunSheep)
        {
            sheepCnt[2] = 0;
            if (!hasShadowSheep)
            {
                sheepCnt[1] = 0;
                sheepCnt[0] = 8;
            }
            else
            {
                sheepCnt[0] = sheepCnt[1] = 1;
                sheepCnt[0] += Random.Range(0, 7);
                sheepCnt[1] += Random.Range(0, 7 - sheepCnt[0]);
            }
        }
        else
        {
            sheepCnt[0] = sheepCnt[1] = 1;
            sheepCnt[2] = Random.Range(1, 3);
            sheepCnt[0] += Random.Range(0, 7 - sheepCnt[2]);
            sheepCnt[1] += Random.Range(0, 7 - sheepCnt[2] - sheepCnt[0]);
        }
    }
}
