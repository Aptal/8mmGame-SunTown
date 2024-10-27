using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreLevelControl : MonoBehaviour
{
    // �洢����
    public int storeLimit = 30;
    public int storeLimitLevel = 0;
    public int[] stlimlevel = new int[6] { 30, 36, 43, 52, 62, 75 };

    // �����ٶ�
    public int pushSpeed = 4;
    public int pushSpeedLevel = 0;
    public int[] pushlevel = new int[6] { 4, 5, 6, 7, 8, 9 };

    // ȡ���ٶ�
    public int popSpeed = 4;
    public int popSpeedLevel = 0;
    public int[] poplevel = new int[6] { 4, 5, 6, 7, 8, 9 };

    //�������⻨��
    public int[] upLevelCost = new int[4] { 0, 200, 400, 800 };


    public void UpdateInfo()
    {
        storeLimit = stlimlevel[storeLimitLevel];
        pushSpeed = pushlevel[pushSpeedLevel];
        popSpeed = poplevel[popSpeedLevel];
    }

    public bool upStoreLimitLevel(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[storeLimitLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[storeLimitLevel + 1]))
                {
                    storeLimitLevel++;
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    public bool UpPushSpeed(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[pushSpeedLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[pushSpeedLevel + 1]))
                {
                    pushSpeedLevel++;
                    pushSpeed = (int)(pushSpeed * 1.2 + 0.5);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    public bool UpPopSpeedLevel(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[popSpeedLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[popSpeedLevel + 1]))
                {
                    popSpeedLevel++;
                    popSpeed = (int)(popSpeed * 1.2 + 0.5);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

}
