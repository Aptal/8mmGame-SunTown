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

    public TextMeshProUGUI eventTitle;
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
            //backButton.gameObject.SetActive(false);
            backButton.interactable = false;
        }
    }

    void InitTitle()
    {
        switch (plotIndex)
        {
            case 1:
                eventTitle.text = "远道而来";
                break;
            case 2:
                eventTitle.text = "众矢之的";
                break;
            case 3:
                eventTitle.text = "光的子民";
                break;
            case 4:
                eventTitle.text = "拿酒来！";
                break;
            case 5:
                eventTitle.text = "天才出少年";
                break;
            case 6:
                eventTitle.text = "“去，捡回来”";
                break;
            case 7:
                eventTitle.text = "落魄的英雄";
                break;
            case 8:
                eventTitle.text = "遗留的财产";
                break;
            case 9:
                eventTitle.text = "哀悼";
                break;
            case 10:
                eventTitle.text = "紧急的快递";
                break;
            case 11:
                eventTitle.text = "离光最近的地方";
                break;
            case 12:
                eventTitle.text = "曾经的幸福";
                break;
            case 13:
                eventTitle.text = "羊与犬";
                break;
            case 14:
                eventTitle.text = "康复如初";
                break;
            case 15:
                eventTitle.text = "一笔值得的交易";
                break;
            case 16:
                eventTitle.text = "并非不可能";
                break;
            case 17:
                eventTitle.text = "新的线索";
                break;
            case 18:
                eventTitle.text = "逼近真相";
                break;
            case 19:
                eventTitle.text = "指引你的光";
                break;
            case 20:
                eventTitle.text = "你的身边";
                break;
            case 21:
                eventTitle.text = "日落金黄";
                break;
            case 22:
                eventTitle.text = "萦绕的心绪";
                break;
            case 23:
                eventTitle.text = "遥远的远方";
                break;
            case 24:
                eventTitle.text = "最后一面";
                break;
            case 25:
                eventTitle.text = "一笔更值得的交易";
                break;
            case 26:
                eventTitle.text = "光的魔法";
                break;
            case 27:
                eventTitle.text = "审判";
                break;
            case 28:
                eventTitle.text = "事件：终曲1";
                break;
            case 29:
                eventTitle.text = "事件：终曲2";
                break;
            default:
                eventTitle.text = "牧光之地";
                break;
        }
        Debug.Log("index " + plotIndex + "   text " + eventTitle.text);

    }

    public void PlayPlot()
    {
        if(plotCanvas == null)
        {
            Debug.Log("init plot plane");
            if (plotIndex == 10 || plotIndex == 15 || plotIndex == 25 || plotIndex == 19 ) // 21 3-1
            {
                plotCanvas = plot3Canvas;
            }
            else
            {
                plotCanvas = plot2Canvas;
            }
            
            plotCanvas.SetActive(true);
            backButton.interactable = true;
            InitTitle();
        }
        Debug.Log("123:  "+plotIndex);
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

        if(plotnode.buttonAtext != "退出")
        {
            buttonText[0].text = plotnode.buttonAtext;
            if (plotnode.buttonAtext != "")
            {
                choiceButton[0].interactable = true;
                nextButton.interactable = false;
            }
            else
            {
                choiceButton[0].interactable = false;
                nextButton.interactable = true;
            }
        }

        if (plotnode.buttonBtext != "退出")
        {
            buttonText[1].text = plotnode.buttonBtext;
            if (plotnode.buttonBtext != "")
            {
                choiceButton[1].interactable = true;
                nextButton.interactable = false;
            }
            else
            {
                choiceButton[1].interactable = false;
                nextButton.interactable = true;
            }
        }
            
        if(choiceButton.Length == 3 && plotnode.choiceCtext != "退出")
        {
            buttonText[2].text = plotnode.buttonCtext;
            if (plotnode.buttonCtext != "")
            {
                choiceButton[2].interactable = true;
                nextButton.interactable = false;
            }
            else
            {
                choiceButton[2].interactable = false;
                nextButton.interactable = true;
            }
        }


        // 退出, 事件结束
        if ( (plotnode.buttonAtext == "退出" && clickedButton <= 0 )  || (plotnode.buttonBtext == "退出" && clickedButton == 1) || (plotnode.buttonCtext == "退出" && clickedButton == 2))
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

                //close 11
                plotCanvas.gameObject.SetActive(false);
                TimeControl.Instance.UpdateUI();

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
            else
            {
                
            }

            TimeControl.Instance.eventIndex++;
        }
    }

    public void ClickA()
    {
        clickedButton = 0;
        backButton.interactable = false;
        NextText();
    }

    public void ClickB()
    {
        clickedButton = 1;
        backButton.interactable = false;
        NextText();
    }
    public void ClickC()
    {
        clickedButton = 2;
        backButton.interactable = false;
        NextText();
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
