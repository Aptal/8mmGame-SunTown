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
                eventTitle.text = "Զ������";
                break;
            case 2:
                eventTitle.text = "��ʸ֮��";
                break;
            case 3:
                eventTitle.text = "�������";
                break;
            case 4:
                eventTitle.text = "�þ�����";
                break;
            case 5:
                eventTitle.text = "��ų�����";
                break;
            case 6:
                eventTitle.text = "��ȥ���������";
                break;
            case 7:
                eventTitle.text = "���ǵ�Ӣ��";
                break;
            case 8:
                eventTitle.text = "����ĲƲ�";
                break;
            case 9:
                eventTitle.text = "����";
                break;
            case 10:
                eventTitle.text = "����Ŀ��";
                break;
            case 11:
                eventTitle.text = "�������ĵط�";
                break;
            case 12:
                eventTitle.text = "�������Ҹ�";
                break;
            case 13:
                eventTitle.text = "����Ȯ";
                break;
            case 14:
                eventTitle.text = "�������";
                break;
            case 15:
                eventTitle.text = "һ��ֵ�õĽ���";
                break;
            case 16:
                eventTitle.text = "���ǲ�����";
                break;
            case 17:
                eventTitle.text = "�µ�����";
                break;
            case 18:
                eventTitle.text = "�ƽ�����";
                break;
            case 19:
                eventTitle.text = "ָ����Ĺ�";
                break;
            case 20:
                eventTitle.text = "������";
                break;
            case 21:
                eventTitle.text = "������";
                break;
            case 22:
                eventTitle.text = "���Ƶ�����";
                break;
            case 23:
                eventTitle.text = "ңԶ��Զ��";
                break;
            case 24:
                eventTitle.text = "���һ��";
                break;
            case 25:
                eventTitle.text = "һ�ʸ�ֵ�õĽ���";
                break;
            case 26:
                eventTitle.text = "���ħ��";
                break;
            case 27:
                eventTitle.text = "����";
                break;
            case 28:
                eventTitle.text = "�¼�������1";
                break;
            case 29:
                eventTitle.text = "�¼�������2";
                break;
            default:
                eventTitle.text = "����֮��";
                break;
        }
        Debug.Log("index " + plotIndex + "   text " + eventTitle.text);

    }

    public void PlayPlot()
    {
        if(plotCanvas == null)
        {
            /*Debug.Log("init plot plane");
            if (plotIndex == 10 || plotIndex == 15 || plotIndex == 25 || plotIndex == 19 ) // 21 3-1
            {
                plotCanvas = plot3Canvas;
            }
            else
            {
                plotCanvas = plot2Canvas;
            }*/
            plotCanvas = plot2Canvas;
            plotCanvas.SetActive(true);
            backButton.interactable = true;
            InitTitle();
        }
        //Debug.Log("plotIndex:  " + plotIndex);
        PlotNode plotnode = plot[plotIndex].plots[Mathf.Clamp(index, 0, plot[plotIndex].plots.Length - 1)];

        eventImg.sprite = plotnode.picture;

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

        if(plotnode.buttonAtext != "�˳�")
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

        if (plotnode.buttonBtext != "�˳�")
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
            
        if(choiceButton.Length == 3 && plotnode.choiceCtext != "�˳�")
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


        // �˳�, �¼�����
        if ( (plotnode.buttonAtext == "�˳�" && clickedButton <= 0 )  || (plotnode.buttonBtext == "�˳�" && clickedButton == 1) || (plotnode.buttonCtext == "�˳�" && clickedButton == 2))
        {
            TimeControl.Instance.hasEvent[TimeControl.Instance.eventIndex]--;

            // �����¼�Ч��
            if(plotIndex == 3)
            {
                // �����滮��
                TimeControl.Instance.hasBuilding[1] = true;
                TimeControl.Instance.happyCtrl.happyValue = 10;
            }
            else if (plotIndex == 4)
            {
                // ѡ��A
                if(clickedButton == 0)
                {
                    TimeControl.Instance.sunCtrl.CostSun(50);
                    TimeControl.Instance.eventOutcome += 50;
                }
                // ѡ��B 
                else if(clickedButton == 1)
                {
                    TimeControl.Instance.happyCtrl.happyValue += 10;
                }
            }
            else if(plotIndex == 5)
            {
                // �����о���
                TimeControl.Instance.hasBuilding[0] = true;
                // ȥ�������
            }
            else if(plotIndex == 7)
            {
                if(clickedButton == 0)
                {
                    TimeControl.Instance.happyCtrl.happyValue -= 10;
                }
                else if(clickedButton == 1)
                {
                    // ѡ��B���·���
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
                // ѡ�� B���200���
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
                // ��������
                TimeControl.Instance.hasBuilding[2] = true;
                TimeControl.Instance.faithCtrl.faithValue = 10;

                //close 11
                plotCanvas.gameObject.SetActive(false);
                TimeControl.Instance.UpdateUI();

                //���ſ����¼�12
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
                // ����������
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
                    // ��֣�������
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
                    // �ж�����70
                }
                else if (clickedButton == 1)
                {
                    
                }
            }

            // �¼����ر�
            plotCanvas.gameObject.SetActive(false);

            TimeControl.Instance.UpdateUI();
            // ҹ��������
            if(TimeControl.Instance.eventIndex % 2 != 0)
            {
                TimeControl.Instance.checkControl.ShowDailyCheck();
            }
            else
            {
                //TimeControl.Instance.eventIndex++;
            }

        }
    }

    public void ClickA()
    {
        if(plotIndex == 26)
        {
            if(TimeControl.Instance.faithCtrl.faithValue >= 70)
            {
                clickedButton = 0;
                backButton.interactable = false;
                NextText();
            }
        }
        else if(plotIndex == 4)
        {
            if (TimeControl.Instance.sunCtrl.totalSun >= 50)
            {
                clickedButton = 0;
                backButton.interactable = false;
                NextText();
            }
        }
        else if(plotIndex == 8)
        {
            if (TimeControl.Instance.sunCtrl.totalSun >= 100)
            {
                clickedButton = 0;
                backButton.interactable = false;
                NextText();
            }
        }
        else
        {
            clickedButton = 0;
            backButton.interactable = false;
            NextText();
        }
    }

    public void ClickB()
    {
        if (plotIndex == 13)
        {
            if (TimeControl.Instance.sunCtrl.totalSun >= 200)
            {
                clickedButton = 1;
                backButton.interactable = false;
                NextText();
            }
        }
        else
        {
            clickedButton = 1;
            backButton.interactable = false;
            NextText();
        }
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
        index = Mathf.Clamp(index, 0, plot[plotIndex].plots.Length - 1);
        PlayPlot();
    }

    public void BackText()
    {
        index--;
        index = Mathf.Clamp(index, 0, plot[plotIndex].plots.Length - 1);
        PlayPlot();
    }
}
