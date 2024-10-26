using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubLevelControl : MonoBehaviour
{
    // �洢�ٶ�
    public int sheep2hubSpeed = 3;
    public int hubSpeedLevel = 0;

    // ��������
    public int fixSunCost;

    // ������
    public int delSunCost;

    public int[] upLevelCost = new int[3] { 0, 1000, 2000 };

    public bool UpMoveSpeed(SunControl sunCtrl)
    {
        if (sunCtrl != null)
        {
            if (sunCtrl.totalSun >= upLevelCost[hubSpeedLevel + 1])
            {
                if (sunCtrl.CostSun(upLevelCost[hubSpeedLevel + 1]))
                {
                    hubSpeedLevel++;
                    sheep2hubSpeed = (int)(sheep2hubSpeed * 1.2 + 0.5);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }
}
