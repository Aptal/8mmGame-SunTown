using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataObject : MonoBehaviour
{
}

// 时间系统
public class MainTimeData
{
    public int dayCnt;
    public bool hasMiniGame;

    public MainTimeData(int initialDayCnt, bool hasMiniGame)
    {
        dayCnt = initialDayCnt;
        this.hasMiniGame = hasMiniGame;
    }
}

// 阳光
public class MainSunData
{
    public int totalSun;

    public MainSunData(int initialTotalSun)
    {
        totalSun = initialTotalSun;
    }
}

// 羊群
public class MainSheepData
{
    // 羊群速度
    public float moveSpeed;
    public int moveSpeedLevel;

    // 存储上限
    public int sunLimit;
    public int sunLimitLevel;

    // 产出速度
    public int productSpeed;
    public int productSpeedLevel;

    public MainSheepData(float initialMoveSpeed, int initialMoveSpeedLevel,
                         int initialSunLimit, int initialSunLimitLevel,
                         int initialProductSpeed, int initialProductSpeedLevel)
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
    // 存储上限
    public int storeLimit;
    public int storeLimitLevel;

    // 储存速度
    public int pushSpeed;
    public int pushSpeedLevel;

    // 取出速度
    public int popSpeed;
    public int popSpeedLevel;

    public MainStoreData(int initialStoreLimit, int initialStoreLimitLevel,
                         int initialPushSpeed, int initialPushSpeedLevel,
                         int initialPopSpeed, int initialPopSpeedLevel)
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
    // 存储速度
    public int sheep2hubSpeed;
    public int hubSpeedLevel;

    public MainHubData(int initialSheep2hubSpeed, int initialHubSpeedLevel)
    {
        sheep2hubSpeed = initialSheep2hubSpeed;
        hubSpeedLevel = initialHubSpeedLevel;
    }
}

public class MainHappyData
{
    public int happyValue;

    public MainHappyData(int initialHappyValue)
    {
        happyValue = initialHappyValue;
    }
}

public class MainFaithData
{
    public int faithValue;

    public MainFaithData(int initialFaithValue)
    {
        faithValue = initialFaithValue;
    }
}