using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaithControl : MonoBehaviour
{
    public Image faithSlider;

    public int faithValue = 25;
    public int faithMaxVal = 100;

    public int faithChange = 0;

    void Start()
    {
        
    }

    void Update()
    {
        //SliderCtrl();
    }

    public void SliderCtrl()
    {
        faithValue = Mathf.Min(faithMaxVal, faithValue);
        faithSlider.fillAmount = (float) (faithValue) / (float) (faithMaxVal);
    }
}
