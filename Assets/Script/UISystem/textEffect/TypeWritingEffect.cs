using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;

public class DOTweenTMPTypewriter : MonoBehaviour
{
    public TMP_Text textComponent1;
    public TMP_Text textComponent2;
    
    string fullText1 = "辛苦啦！\n今日光能收入为：";
    

    void Start()
    {

        // 获取NumberProviderScript组件的引用
        int SunCnt = GameManager.Instance.hubTile.totalSun;
        StartCoroutine(DisplayTextSequentially());
        
        textComponent1.text = "";
        textComponent2.text = "";
        IEnumerator DisplayTextSequentially()
        {
            // 显示第一段TMP文字
            for (int i = 0; i < fullText1.Length; i++)
            {
                textComponent1.text += fullText1[i];
                yield return new WaitForSeconds(0.05f);
            }
            // 显示第二段TMP文字（使用数字变量）
                string numberText = GameManager.Instance.hubTile.totalSun.ToString();
                for (int j = 0; j < numberText.Length; j++)
                {
                    textComponent2.text += numberText[j];
                    yield return new WaitForSeconds(0.05f);
                }
            
        } 
    }
       
        
    }
    
    

