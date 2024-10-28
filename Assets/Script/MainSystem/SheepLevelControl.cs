using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepLevelControl : MonoBehaviour
{
    // 羊群速度
    public float moveSpeed = 0.5f;
    public int moveSpeedLevel = 0;
    public float[] speedlevel = new float[6] {0.6f, 0.72f, 0.86f, 1.0f, 1.2f, 1.5f};

    // 存储上限
    public int sunLimit = 30;
    public int[] limlevel = new int[6] { 30, 36, 43, 52, 62, 75 };
    public int sunLimitLevel = 0;

    // 产出速度
    public int productSpeed = 2;
    public int[] prolevel = new int[6] { 3, 4, 5, 6, 7, 8 };
    public int productSpeedLevel = 0;

    //升级阳光花费
    public int[] upLevelCost = new int[6] {0, 500, 1000, 1500, 2000, 2500 };

    public void UpdateInfo()
    {
        moveSpeed = speedlevel[moveSpeedLevel];
        sunLimit = limlevel[sunLimitLevel];
        productSpeed = prolevel[productSpeedLevel];
    }

    public bool UpMoveSpeed(SunControl sunCtrl)
    {
        if(sunCtrl != null)
        {
            if(sunCtrl.totalSun >= upLevelCost[moveSpeedLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[moveSpeedLevel + 1]))
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
            if (sunCtrl.totalSun >= upLevelCost[sunLimitLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[sunLimitLevel + 1]))
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
            if (sunCtrl.totalSun >= upLevelCost[productSpeedLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[productSpeedLevel + 1]))
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
