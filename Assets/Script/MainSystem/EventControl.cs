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

    [SerializeField] protected GameObject plotCanvas;

    public Image eventImg;
    public Button backButton;
    public Button nextButton;
    public Button[] choiceButton;
    public TextMeshProUGUI[] choiceText;

    protected int index;
    public int plotIndex;

    public void PlayPlot()
    {
        plotCanvas.SetActive(true);

        PlotNode plotnode = plot[plotIndex].plots[Mathf.Clamp(index, 0, plot[plotIndex].plots.Length - 1)];

        plotText.text = plotnode.content;

        if (index == plot[plotIndex].plots.Length-1)
        {
            for(int i = 0; i < choiceButton.Length; i++)
            {
                if (choiceButton[i] != null)
                {
                    choiceButton[i].interactable = true;
                    choiceText[i].gameObject.SetActive(true);
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
                    choiceText[i].gameObject.SetActive(false);
                }
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
