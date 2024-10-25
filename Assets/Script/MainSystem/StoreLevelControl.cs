using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreLevelControl : MonoBehaviour
{
    // �洢����
    public int storeLimit = 10;
    public int storeLimitLevel = 1;

    // �����ٶ�
    public int pushSpeed = 4;
    public int pushSpeedLevel = 1;

    // ȡ���ٶ�
    public int popSpeed = 4;
    public int popSpeedLevel = 1;

    //�������⻨��
    public int[] upLevelCost = new int[4] { 0, 200, 400, 800 };

    public bool upStoreLimitLevel(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[storeLimitLevel])
            {
                if (sunCtrl.CostSun(upLevelCost[storeLimitLevel]))
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
            if (sunCtrl.totalSun >= upLevelCost[pushSpeedLevel])
            {
                if (sunCtrl.CostSun(upLevelCost[pushSpeedLevel]))
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
            if (sunCtrl.totalSun >= upLevelCost[popSpeedLevel])
            {
                if (sunCtrl.CostSun(upLevelCost[popSpeedLevel]))
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
