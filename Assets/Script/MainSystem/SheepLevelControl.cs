using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepLevelControl : MonoBehaviour
{
    // 羊群速度
    public float moveSpeed = 0.2f;
    public int moveSpeedLevel = 1;

    // 存储上限
    public int sunLimit = 10;
    public int sunLimitLevel = 1;

    // 产出速度
    public int productSpeed = 4;
    public int productSpeedLevel = 1;

    //升级阳光花费
    public int[] upLevelCost = new int[6] {0, 100, 200, 400, 800, 1500 };

    public bool UpMoveSpeed(SunControl sunCtrl)
    {
        if(sunCtrl != null)
        {
            if(sunCtrl.totalSun >= upLevelCost[moveSpeedLevel])
            {
                if (sunCtrl.CostSun(upLevelCost[moveSpeedLevel]))
                {
                    moveSpeedLevel++;
                    return true;
                }   
                else
                    return false;
            }
        }
        return false;
    }

    public bool UpSunLimit(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[sunLimitLevel])
            {
                if (sunCtrl.CostSun(upLevelCost[sunLimitLevel]))
                {
                    sunLimitLevel++;
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    public bool UpProductSpeed(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[productSpeedLevel])
            {
                if (sunCtrl.CostSun(upLevelCost[productSpeedLevel]))
                {
                    productSpeedLevel++;
                    productSpeed = (int)(productSpeed * 1.2 + 0.5);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }
}
