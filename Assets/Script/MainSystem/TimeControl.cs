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
                // ï¿½Ú³ï¿½ï¿½ï¿½ï¿½Ð²ï¿½ï¿½ï¿½GameDataManagerï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ã»ï¿½ï¿½ï¿½ò´´½ï¿½Ò»ï¿½ï¿½ï¿½Âµï¿½
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

    public Canvas beginCanvas;
    public Canvas endCanvas;

    public CheckControl checkControl;
    public EventControl event2Control;


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
    public bool hasRunSheep = false;
    public int[] sheepCnt = new int[3];

    // weather
    public int weatherType = 0;
    public float weatherK = 1.0f;
    public Image weatherImg;
    public Sprite[] weatherSprites;

    public Button[] buildingButton;
    public bool[] hasBuilding = new bool[3];

    public bool canFix = false;
    public bool hasMiniGame = true;

    public int eventOutcome;
    public int researchOutcome;
    public int[] hasEvent = new int[42] { 1, 1, 1, 1, 1, 1, 0, 
                                          1, 0, 1, 1, 1, 2, 1,
                                          1, 0, 0, 1, 1, 1, 0,
                                          1, 1, 1, 0, 0, 0, 1,
                                          1, 1, 1, 1, 0, 1, 1,
                                          0, 0, 0, 1, 0, 0, 1};
    public int eventIndex = 0;

    private void Awake()
    {
        Debug.Log("awake main");
        if (Instance != null)
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

        /*        if (Instance == null)
                {
                    Instance = this;
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
                else
                {
                    if (Instance != this)
                    {
                        Destroy(gameObject);
                    }
                }
                DontDestroyOnLoad(gameObject);*/

    }

    private void Start()
    {
        Debug.Log("start main");

        if(PlayerPrefs.GetString("SceneInfo") == "FinishMiniGame")
        {
            hasMiniGame = false;
            PlayerPrefs.DeleteKey("SceneInfo");
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("totalsun + " + sunCtrl.totalSun);
            checkControl.beginSunText.text = sunCtrl.totalSun.ToString();
        }

        if(hasMiniGame)
        {
            beginCanvas.gameObject.SetActive(true);
        }   
        else
        {
            endCanvas.gameObject.SetActive(true);
        }
        
        int minigameSun = PlayerPrefs.GetInt("MiniGameGotSun");
        if (minigameSun >= 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        sunCtrl.sunIncome += minigameSun;
        sunCtrl.sunOutcome = minigameSun / 10;
        sunCtrl.totalSun += minigameSun - sunCtrl.sunOutcome;

        UpdateUI();
        //UpdateEvent();
    }

    private void Update()
    {
        // ½øÈë°×Ìì, Ò»Ìì¿ªÊ¼
        if (PlayerPrefs.GetString("animatN2D") == "yes")
        {
            PlayerPrefs.DeleteKey("animatN2D");
            PlayerPrefs.Save();
            event2Control.PlayPlot();

            if (eventIndex == 0)
            {
                event2Control.plotIndex = 1;
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 2)
            {
                event2Control.plotIndex = 3;
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 4)
            {
                event2Control.plotIndex = 5;
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 6)
            {
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 8)
            {
                weatherType = 1;
                roadCtrl.badRoadCnt = 0;
                hasShadowSheep = true;
            }
            else if (eventIndex == 10)
            {
                event2Control.plotIndex = 9;
                weatherType = 2;
                roadCtrl.badRoadCnt = 3;
            }
            else if (eventIndex == 12)
            {
                event2Control.plotIndex = 11;
                weatherType = 1;
                roadCtrl.badRoadCnt = 2;
            }
            else if (eventIndex == 14)
            {
                event2Control.plotIndex = 14;
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 16)
            {
                weatherType = 0;
                roadCtrl.badRoadCnt = 2;
            }
            else if (eventIndex == 18)
            {
                event2Control.plotIndex = 16;
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 20)
            {
                weatherType = 1;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 22)
            {
                event2Control.plotIndex = 19;
                weatherType = 2;
                roadCtrl.badRoadCnt = 3;
            }
            else if (eventIndex == 24)
            {
                weatherType = 1;
                roadCtrl.badRoadCnt = 2;
            }
            else if (eventIndex == 26)
            {
                weatherType = 0;
                roadCtrl.badRoadCnt = 1;
            }
            else if (eventIndex == 28)
            {
                event2Control.plotIndex = 22;
                weatherType = 0;
                roadCtrl.badRoadCnt = 1;
            }
            else if (eventIndex == 30)
            {
                event2Control.plotIndex = 24;
                weatherType = 1;
                roadCtrl.badRoadCnt = 3;
            }
            else if (eventIndex == 32)
            {
                weatherType = 0;
                roadCtrl.badRoadCnt = 0;
            }
            else if (eventIndex == 34)
            {
                event2Control.plotIndex = 27;
                weatherType = 0;
                roadCtrl.badRoadCnt = 2;
            }
            else if (eventIndex == 36)
            {
                weatherType = 0;
                roadCtrl.badRoadCnt = 2;
            }
            else if (eventIndex == 38)
            {
                event2Control.plotIndex = 28;
                weatherType = 1;
                roadCtrl.badRoadCnt = 2;
            }
            else if (eventIndex == 40)
            {
                weatherType = 2;
                roadCtrl.badRoadCnt = 3;
            }

            if (hasEvent[eventIndex] > 0)
            {
                event2Control.PlayPlot();
            }

        }



        // minigame½áÊø, ÍíÉÏ
        if (PlayerPrefs.GetString("animatD2N") == "yes")
        {
            PlayerPrefs.DeleteKey("animatD2N");
            PlayerPrefs.Save();

            if (eventIndex == 1)
            {
                event2Control.plotIndex = 2;
            }
            else if (eventIndex == 3)
            {
                event2Control.plotIndex = 4;
            }
            else if (eventIndex == 5)
            {
                event2Control.plotIndex = 6;
            }
            else if (eventIndex == 7)
            {
                event2Control.plotIndex = 7;
            }
            else if (eventIndex == 9)
            {
                event2Control.plotIndex = 8;
            }
            else if (eventIndex == 11)
            {
                event2Control.plotIndex = 10;
            }
            else if (eventIndex == 13)
            {
                event2Control.plotIndex = 13;
            }
            else if (eventIndex == 15)
            {

            }
            else if (eventIndex == 17)
            {
                event2Control.plotIndex = 15;
            }
            else if (eventIndex == 19)
            {
                event2Control.plotIndex = 17;
            }
            else if (eventIndex == 21)
            {
                event2Control.plotIndex = 18;
            }
            else if (eventIndex == 23)
            {
                event2Control.plotIndex = 20;
            }
            else if (eventIndex == 25)
            {
                
            }
            else if (eventIndex == 27)
            {
                event2Control.plotIndex = 21;
            }
            else if (eventIndex == 29)
            {
                event2Control.plotIndex = 23;
            }
            else if (eventIndex == 31)
            {
                event2Control.plotIndex = 25;
            }
            else if (eventIndex == 33)
            {
                event2Control.plotIndex = 26;
            }
            else if (eventIndex == 35)
            {

            }
            else if (eventIndex == 37)
            {

            }
            else if (eventIndex == 39)
            {

            }
            else if (eventIndex == 41)
            {
                event2Control.plotIndex = 29;
            }

            if (hasEvent[eventIndex] > 0)
            {
                event2Control.PlayPlot();
            }
            else if (!hasMiniGame)
            {
                checkControl.ShowDailyCheck();
            }

        }
    }

    public void NewDay()
    {
        if (dayCnt == -1) dayCnt = 1;
        else dayCnt++;
        hasMiniGame = true;


        // ï¿½Â¼ï¿½ï¿½Ð¶ï¿½

        UpdateUI();


        saveMainData.SaveData1();
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
