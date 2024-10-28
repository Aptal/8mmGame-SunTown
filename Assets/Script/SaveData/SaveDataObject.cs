using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataObject : MonoBehaviour
{
}

// ʱ��ϵͳ
public class MainTimeData
{
    public int dayCnt;
    public bool hasMiniGame;
    public int eventOutcome;
    public int researchOutcome;
    public int eventIndex;
    public bool hasShadowSheep = false;
    public bool hasRunSheep = false;
    public bool[] hasBuilding = new bool[3];
    public int[] hasEvent = new int[42];

    public MainTimeData()
    {
        dayCnt = 1;
        hasMiniGame = true;
        eventOutcome = 0;
        researchOutcome = 0;
        eventIndex = 0;
        hasShadowSheep = false;
        hasRunSheep = false;
        hasBuilding[0] = false;
        hasBuilding[1] = false;
        hasBuilding[2] = false;
        hasEvent = new int[42] { 1, 1, 1, 1, 1, 1, 0,
                                1, 0, 1, 1, 1, 2, 1,
                                1, 0, 0, 1, 1, 1, 0,
                                1, 1, 1, 0, 0, 0, 1,
                                1, 1, 1, 1, 0, 1, 1,
                                0, 0, 0, 1, 0, 0, 1};
    }

    public MainTimeData(int initialDayCnt, bool hasMiniGame, int eventOutcome, int researchOutcome, int eventIndex, bool hasShadowSheep, bool hasRunSheep, bool[] hasBuilding, int[] hasEvent)
    {
        this.dayCnt = initialDayCnt;
        this.hasMiniGame = hasMiniGame;
        this.eventOutcome = eventOutcome;
        this.researchOutcome = researchOutcome;
        this.eventIndex = eventIndex;
        this.hasShadowSheep = hasShadowSheep;
        this.hasRunSheep = hasRunSheep;
        this.hasBuilding = hasBuilding;
        this.hasEvent = hasEvent;
    }
}

// ����
public class MainSunData
{
    public int totalSun;

    public MainSunData(int initialTotalSun = 0)
    {
        totalSun = initialTotalSun;
    }
}

// ��Ⱥ
public class MainSheepData
{
    // ��Ⱥ�ٶ�
    public float moveSpeed;
    public int moveSpeedLevel;

    // �洢����
    public int sunLimit;
    public int sunLimitLevel;

    // �����ٶ�
    public int productSpeed;
    public int productSpeedLevel;

    public MainSheepData(float initialMoveSpeed = 0.5f, int initialMoveSpeedLevel = 0,
                         int initialSunLimit = 30, int initialSunLimitLevel = 0,
                         int initialProductSpeed = 2, int initialProductSpeedLevel = 0)
    {
        moveSpeed = initialMoveSpeed;
        moveSpeedLevel = initialMoveSpeedLevel;
        sunLimit = initialSunLimit;
        sunLimitLevel = initialSunLimitLevel;
        productSpeed = initialProductSpeed;
        productSpeedLevel = initialProductSpeedLevel;
    }
}

public class MainStoreData
{
    // �洢����
    public int storeLimit;
    public int storeLimitLevel;

    // �����ٶ�
    public int pushSpeed;
    public int pushSpeedLevel;

    // ȡ���ٶ�
    public int popSpeed;
    public int popSpeedLevel;

    public MainStoreData(int initialStoreLimit = 60, int initialStoreLimitLevel = 0,
                         int initialPushSpeed = 5, int initialPushSpeedLevel = 0,
                         int initialPopSpeed = 5, int initialPopSpeedLevel = 0)
    {
        storeLimit = initialStoreLimit;
        storeLimitLevel = initialStoreLimitLevel;
        pushSpeed = initialPushSpeed;
        pushSpeedLevel = initialPushSpeedLevel;
        popSpeed = initialPopSpeed;
        popSpeedLevel = initialPopSpeedLevel;
    }
}

public class MainHubData
{
    // �洢�ٶ�
    public int sheep2hubSpeed;
    public int hubSpeedLevel;

    public MainHubData(int initialSheep2hubSpeed = 50, int initialHubSpeedLevel = 0)
    {
        sheep2hubSpeed = initialSheep2hubSpeed;
        hubSpeedLevel = initialHubSpeedLevel;
    }
}

public class MainHappyData
{
    public int happyValue;

    public MainHappyData(int initialHappyValue = 0)
    {
        happyValue = initialHappyValue;
    }
}

public class MainFaithData
{
    public int faithValue;

    public MainFaithData(int initialFaithValue = 0)
    {
        faithValue = initialFaithValue;
    }
}