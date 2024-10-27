using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyControl : MonoBehaviour
{
    public Image happySlider;

    public int happyValue = 25;
    public int happyMaxVal = 100;

    public int happyChange = 0;

    void Update()
    {
        //SliderCtrl();
    }

    public void SliderCtrl()
    {
        happyValue = Mathf.Min(happyMaxVal, happyValue);
        happySlider.fillAmount = (float) happyValue / (float) happyMaxVal;
    }
}
