using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreLevelControl : MonoBehaviour
{
    // 存储上限
    public int storeLimit = 60;
    public int storeLimitLevel = 0;
    public int[] stlimlevel = new int[6] { 60, 75, 93, 117, 146, 183 };

    // 储存速度
    public int pushSpeed = 5;
    public int pushSpeedLevel = 0;
    public int[] pushlevel = new int[6] { 5, 6, 8, 9, 10, 12 };

    // 取出速度
    public int popSpeed = 5;
    public int popSpeedLevel = 0;
    public int[] poplevel = new int[6] { 5, 6, 8, 9, 10, 12 };

    //升级阳光花费
    public int[] upLevelCost = new int[4] { 0, 1000, 2000, 3000 };


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
