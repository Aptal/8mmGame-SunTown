using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckControl : MonoBehaviour
{
    public Sprite[] trendSprite;
    public Image happyTrend;
    public Image faithTrend;

    public TextMeshProUGUI sunIncomeText;
    public TextMeshProUGUI sunOutcomeText;
    public TextMeshProUGUI eventOutcomeText;
    public TextMeshProUGUI researchOutcomeText;

    public TextMeshProUGUI beginSunText;
    public TextMeshProUGUI endSunText;

    public void ShowDailyCheck()
    {
        if(TimeControl.Instance.happyCtrl.happyChange > 0)
        {
            happyTrend.sprite = trendSprite[0];
        }   
        else if(TimeControl.Instance.happyCtrl.happyChange < 0)
        {
            happyTrend.sprite = trendSprite[1];
        }
        else
        {
            happyTrend.sprite = trendSprite[4];
        }

        if (TimeControl.Instance.faithCtrl.faithChange > 0)
        {
            faithTrend.sprite = trendSprite[2];
        }
        else if (TimeControl.Instance.faithCtrl.faithChange < 0)
        {
            faithTrend.sprite = trendSprite[3];
        }
        else
        {
            faithTrend.sprite = trendSprite[4];
        }

        sunIncomeText.text = TimeControl.Instance.sunCtrl.sunIncome.ToString();
        sunOutcomeText.text = TimeControl.Instance.sunCtrl.sunOutcome.ToString();
        eventOutcomeText.text = TimeControl.Instance.eventOutcome.ToString();
        researchOutcomeText.text = TimeControl.Instance.researchOutcome.ToString();

        endSunText.text = TimeControl.Instance.sunCtrl.totalSun.ToString();
        beginSunText.text = (TimeControl.Instance.sunCtrl.totalSun -
                            (TimeControl.Instance.sunCtrl.sunIncome - TimeControl.Instance.sunCtrl.sunOutcome)).ToString();
    }

    public void NextDay()
    {
        TimeControl.Instance.happyCtrl.happyChange = 0;
        TimeControl.Instance.faithCtrl.faithChange = 0;
        TimeControl.Instance.sunCtrl.sunIncome = 0;
        TimeControl.Instance.sunCtrl.sunOutcome = 0;
        TimeControl.Instance.eventOutcome = 0;
        TimeControl.Instance.researchOutcome = 0;

        TimeControl.Instance.NewDay();
    }
}
