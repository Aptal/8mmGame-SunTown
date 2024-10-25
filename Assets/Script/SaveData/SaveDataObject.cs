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

    public MainTimeData(int initialDayCnt)
    {
        dayCnt = initialDayCnt;
    }
}

// ����
public class MainSunData
{
    public int totalSun;

    public MainSunData(int initialTotalSun)
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
    // �洢����
    public int storeLimit;
    public int storeLimitLevel = 1;

    // �����ٶ�
    public int pushSpeed;
    public int pushSpeedLevel;

    // ȡ���ٶ�
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
    // �洢�ٶ�
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
    public float happyValue;

    public MainHappyData(float initialHappyValue)
    {
        happyValue = initialHappyValue;
    }
}

public class MainFaithData
{
    public float faithValue;

    public MainFaithData(float initialFaithValue)
    {
        faithValue = initialFaithValue;
    }
}