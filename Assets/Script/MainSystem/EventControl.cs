using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Security.Cryptography;

public class EventControl : MonoBehaviour
{
    /*    public Sprite[] studyLineImg;
        public Sprite[] dogImg;
        public Sprite[] mayorImg;
        public Sprite[] LUODANImg;
        public Sprite[] bishopImg;
        public Sprite[] endingImg;*/

    [SerializeField] protected PlotArray[] plot;
    [SerializeField] protected TextMeshProUGUI plotText;

    protected GameObject plotCanvas;
    [SerializeField] protected GameObject plot2Canvas;
    [SerializeField] protected GameObject plot3Canvas;

    public Image eventImg;
    public Button backButton;
    public Button nextButton;
    public Button[] choiceButton;
    public TextMeshProUGUI[] buttonText;

    protected int index;
    public int plotIndex;
    public int clickedButton = -1;

    private void Update()
    {
        if(clickedButton != -1 && backButton.gameObject.activeSelf)
        {
            backButton.gameObject.SetActive(false);
        }
    }

    public void PlayPlot()
    {
        if(plotCanvas.activeSelf == null)
        {
            if (TimeControl.Instance.eventIndex == 1)
            {
                plotCanvas = plot2Canvas;
            }
            else
            {
                plotCanvas = plot3Canvas;
            }
            plotCanvas.SetActive(true);
        }

        PlotNode plotnode = plot[plotIndex].plots[Mathf.Clamp(index, 0, plot[plotIndex].plots.Length - 1)];

        if(clickedButton == -1)
        {
            plotText.text = plotnode.content;
        }
        else
        {
            if(clickedButton == 0)
            {
                plotText.text = plotnode.choiceAtext;
            }
            if(clickedButton == 1)
            {
                plotText.text = plotnode.choiceBtext;
            }
            if(clickedButton == 2)
            {
                plotText.text = plotnode.choiceCtext;
            }
        }

        for (int i = 0; i < choiceButton.Length; i++)
        {
            
            if(plotnode.buttonAtext != "")
            {

            }
            else
            {

            }
        }

        if (index == plot[plotIndex].plots.Length-1)
        {
            for(int i = 0; i < choiceButton.Length; i++)
            {
                if (choiceButton[i] != null)
                {
                    choiceButton[i].interactable = true;
                    //choiceText[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < choiceButton.Length; i++)
            {
                if (choiceButton[i] != null)
                {
                    choiceButton[i].interactable = false;
                    //choiceText[i].gameObject.SetActive(false);
                }
            }
        }

        // 退出, 事件结束
        if (plotnode.buttonAtext == "退出")
        {
            TimeControl.Instance.hasEvent[TimeControl.Instance.eventIndex]--;

            // 结算事件效果
            if(plotIndex == 3)
            {
                // 解锁规划局
                TimeControl.Instance.hasBuilding[1] = true;
                TimeControl.Instance.happyCtrl.happyValue = 10;
            }
            else if (plotIndex == 4)
            {
                // 选择A
                if(clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.CostSun(50);
                    TimeControl.Instance.eventOutcome += 50;
                }
                // 选择B 
                else if(clickedButton == 1)
                {
                    TimeControl.Instance.happyCtrl.happyValue += 10;
                }
            }
            else if(plotIndex == 5)
            {
                // 解锁研究所
                TimeControl.Instance.hasBuilding[0] = true;
                // 去升级面板
            }
            else if(plotIndex == 7)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.happyCtrl.happyValue -= 10;
                }
                else if(clickedButton == 1)
                {
                    // 选择B无事发生
                }
            }
            else if (plotIndex == 8)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.CostSun(100);
                    TimeControl.Instance.eventOutcome += 100;
                    TimeControl.Instance.happyCtrl.happyValue += 15;
                }
                // 选择 B获得200阳光
                else if(clickedButton == 1)
                {
                    TimeControl.Instance.sunCtrl.CostSun(-200);
                    TimeControl.Instance.eventOutcome -= 200;
                }
            }
            else if(plotIndex == 11)
            {
/*                if (clickedButton == 0)
                {
                    TimeControl.Instance.faithCtrl.faithValue = 10;
                }
                else if(clickedButton == 1)
                {
                    TimeControl.Instance.faithCtrl.autoAdd = 5;
                }*/
                // 解锁神庙
                TimeControl.Instance.hasBuilding[2] = true;
                TimeControl.Instance.faithCtrl.faithValue = 10;

                //接着开启事件12
                TimeControl.Instance.event2Control.plotIndex = 12;
                TimeControl.Instance.event2Control.PlayPlot();
            }
            else if(plotIndex == 12)
            {
                TimeControl.Instance.sunCtrl.CostSun(-500);
                TimeControl.Instance.eventOutcome -= 500;
            }
            else if(plotIndex == 13)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.happyCtrl.happyValue += 10;
                }
                else if(clickedButton==1)
                {
                    TimeControl.Instance.sunCtrl.CostSun(200);
                    TimeControl.Instance.eventOutcome += 200;
                }
            }
            else if(plotIndex == 14)
            {
                // 解锁跑洋洋
                TimeControl.Instance.hasRunSheep = true;
            }
            else if(plotIndex==15)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.CostSun(1000);
                    TimeControl.Instance.eventOutcome += 1000;
                    TimeControl.Instance.happyCtrl.happyValue += 15;
                }
                else if (clickedButton==1)
                {
                    TimeControl.Instance.sunCtrl.CostSun(1000);
                    TimeControl.Instance.eventOutcome += 1000;
                    TimeControl.Instance.faithCtrl.faithValue += 15;
                }
                else
                {
                    
                }
            }
            else if(plotIndex == 18)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.faithCtrl.faithValue += 10;
                }
                else if(clickedButton==1)
                {
                    TimeControl.Instance.faithCtrl.faithValue -= 10;
                }
            }
            else if(plotIndex == 19)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.happyCtrl.happyValue += 10;
                }
                else if(clickedButton == 1)
                {
                    TimeControl.Instance.faithCtrl.faithValue += 10;
                }
            }
            else if(plotIndex == 20)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.sunAuto = 500;

                }
            }
            else if(plotIndex == 21)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.faithCtrl.faithValue += 10;
                }
                else if(clickedButton == 1)
                {
                    TimeControl.Instance.faithCtrl.faithValue -= 10;
                }
            }
            else if( plotIndex == 22)
            {
                TimeControl.Instance.happyCtrl.happyValue += 10;
            }
            else if(plotIndex == 23)
            {
                if (clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.CostSun(-500);
                    TimeControl.Instance.eventOutcome -= 500;
                }
                else if (clickedButton == 1)
                {
                    TimeControl.Instance.happyCtrl.happyValue += 10;
                }
                else if(clickedButton == 2)
                {
                    // 结局？？？？
                }
            }
            else if (plotIndex == 25)
            {
                if (clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.CostSun(3000);
                    TimeControl.Instance.eventOutcome = 3000;
                    TimeControl.Instance.happyCtrl.happyValue += 30;
                }
                else if (clickedButton == 1)
                {
                    TimeControl.Instance.sunCtrl.CostSun(3000);
                    TimeControl.Instance.eventOutcome = 3000;
                    TimeControl.Instance.faithCtrl.faithValue += 30;
                }
                else if (clickedButton == 2)
                {
                    // nothing
                }
            }
            else if( plotIndex == 26)
            {
                if (clickedButton == 0)
                {
                    // 判断信仰70
                }
                else if (clickedButton == 1)
                {
                    
                }
            }

            // 事件面板关闭
            plotCanvas.gameObject.SetActive(false);

            TimeControl.Instance.UpdateUI();
            // 夜晚进入结算
            if(TimeControl.Instance.eventIndex % 2 != 0)
            {
                TimeControl.Instance.checkControl.ShowDailyCheck();
            }
        }
    }

    public void NextText()
    {
        index++;
        PlayPlot();
    }

    public void BackText()
    {
        index--;
        PlayPlot();
    }
}
