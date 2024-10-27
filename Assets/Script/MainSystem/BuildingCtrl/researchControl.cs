using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class researchControl : MonoBehaviour
{
    public Image[] researchImg;
    public TextMeshProUGUI[] researchCost;
    public Button[] upButton;
    
    private int[] researchLevel = new int [9];
    private int upCost = 0;

    public Sprite[] fiveLevelSprite;

    public Sprite[] threeLevelSprite;

    public Sprite[] hubSpeedSprite;

    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (TimeControl.Instance.sunCtrl.QueryUp(TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[i] + 1]))
            {
                upButton[i].interactable = true;
            }
            else
            {
                upButton[i].interactable = false;
            }
        }

        for (int i = 3; i < 6; i++)
        {
            if (TimeControl.Instance.sunCtrl.QueryUp(TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[i] + 1]))
            {
                upButton[i].interactable = true;
            }
            else
            {
                upButton[i].interactable = false;
            }
        }

        for (int i = 6; i < 7; i++)
        {
            if (TimeControl.Instance.sunCtrl.QueryUp(TimeControl.Instance.hubLevelCtrl.upLevelCost[researchLevel[i] + 1]))
            {
                upButton[i].interactable = true;
            }
            else
            {
                upButton[i].interactable = false;
            }
        }

    }

    private void UpdateImg(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }
    private void UpdateText(TextMeshProUGUI text, string cost)
    {
        text.text = cost;
        Debug.Log(cost);
    }

    public void ResearchButton()
    {
        researchLevel[0] = TimeControl.Instance.sheepLevelCtrl.moveSpeedLevel;
        researchLevel[1] = TimeControl.Instance.sheepLevelCtrl.sunLimitLevel;
        researchLevel[2] = TimeControl.Instance.sheepLevelCtrl.productSpeedLevel;
        for(int i = 0; i < 3; ++i)
        {
            UpdateImg(researchImg[i], fiveLevelSprite[researchLevel[i]]);
            UpdateText(researchCost[i], TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[i] + 1].ToString());
        }

        researchLevel[3] = TimeControl.Instance.storeLevelCtrl.storeLimitLevel;
        researchLevel[4] = TimeControl.Instance.storeLevelCtrl.pushSpeedLevel;
        researchLevel[5] = TimeControl.Instance.storeLevelCtrl.popSpeedLevel;
        for (int i = 3; i < 6; ++i)
        {
            UpdateImg(researchImg[i], threeLevelSprite[researchLevel[i]]);
            UpdateText(researchCost[i], TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[i] + 1].ToString());
        }

        researchLevel[6] = TimeControl.Instance.hubLevelCtrl.hubSpeedLevel;
        for (int i = 6; i < 7; ++i)
        {
            UpdateImg(researchImg[i], hubSpeedSprite[researchLevel[i]]);
            UpdateText(researchCost[i], TimeControl.Instance.hubLevelCtrl.upLevelCost[researchLevel[i] + 1].ToString());
        }

    }

    public void SheepSpeedButton()
    {
        if(TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[0] + 1]))
        {
            upCost += TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[0] + 1];
            researchLevel[0]++;
            UpdateImg(researchImg[0], fiveLevelSprite[researchLevel[0]]);
            UpdateText(researchCost[0], TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[0] + 1].ToString());
        }
    }

    public void SheepLimitButton()
    {
        if (TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[1] + 1]))
        {
            upCost += TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[1] + 1];
            researchLevel[1]++;
            UpdateImg(researchImg[1], fiveLevelSprite[researchLevel[1]]);
            UpdateText(researchCost[1], TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[1] + 1].ToString());

        }
    }

    public void SheepProdVButton()
    {
        if (TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[2] + 1]))
        {
            upCost += TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[2] + 1];
            researchLevel[2]++;
            UpdateImg(researchImg[2], fiveLevelSprite[researchLevel[2]]);
            UpdateText(researchCost[2], TimeControl.Instance.sheepLevelCtrl.upLevelCost[researchLevel[2] + 1].ToString());
        }
    }

    public void StoreLimitButton()
    {
        if (TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[3] + 1]))
        {
            upCost += TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[3] + 1];
            researchLevel[3]++;
            UpdateImg(researchImg[3], threeLevelSprite[researchLevel[3]]);
            UpdateText(researchCost[3], TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[3] + 1].ToString());
        }
    }

    public void StorePushVButton()
    {
        if (TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[4] + 1]))
        {
            upCost += TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[4] + 1];
            researchLevel[4]++;
            UpdateImg(researchImg[4], threeLevelSprite[researchLevel[4]]);
            UpdateText(researchCost[4], TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[4] + 1].ToString());
        }
    }

    public void StorePopVButton()
    {
        if (TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[5] + 1]))
        {
            upCost += TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[5] + 1];
            researchLevel[5]++;
            UpdateImg(researchImg[5], threeLevelSprite[researchLevel[5]]);
            UpdateText(researchCost[5], TimeControl.Instance.storeLevelCtrl.upLevelCost[researchLevel[5] + 1].ToString());
        }
    }

    public void hubSpeedButton()
    {
        if (TimeControl.Instance.sunCtrl.CostSun(TimeControl.Instance.hubLevelCtrl.upLevelCost[researchLevel[6] + 1]))
        {
            upCost += TimeControl.Instance.hubLevelCtrl.upLevelCost[researchLevel[6] + 1];
            researchLevel[6]++;
            UpdateImg(researchImg[6], hubSpeedSprite[researchLevel[6]]);
            UpdateText(researchCost[6], TimeControl.Instance.hubLevelCtrl.upLevelCost[researchLevel[6] + 1].ToString());
        }
    }

    public void ApplyButton()
    {
        TimeControl.Instance.sheepLevelCtrl.moveSpeedLevel = researchLevel[0];
        TimeControl.Instance.sheepLevelCtrl.sunLimitLevel = researchLevel[1];
        TimeControl.Instance.sheepLevelCtrl.productSpeedLevel = researchLevel[2];

        TimeControl.Instance.storeLevelCtrl.storeLimitLevel = researchLevel[3];
        TimeControl.Instance.storeLevelCtrl.pushSpeedLevel = researchLevel[4];
        TimeControl.Instance.storeLevelCtrl.popSpeedLevel = researchLevel[5];

        TimeControl.Instance.hubLevelCtrl.hubSpeedLevel = researchLevel[6];

        TimeControl.Instance.researchOutcome += upCost;
        upCost = 0;
    }

    public void CancelButton() 
    {
        TimeControl.Instance.sunCtrl.totalSun += upCost;
        upCost = 0;
        ResearchButton();
    }
}
