using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreLevelControl : MonoBehaviour
{
    // 存储上限
    public int storeLimit = 40;
    public int storeLimitLevel = 0;

    // 储存速度
    public int pushSpeed = 4;
    public int pushSpeedLevel = 0;

    // 取出速度
    public int popSpeed = 4;
    public int popSpeedLevel = 0;

    //升级阳光花费
    public int[] upLevelCost = new int[4] { 0, 200, 400, 800 };

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
