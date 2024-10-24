using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyControl : MonoBehaviour
{
    public Image happySlider;

    public float happyValue = 25;
    public float happyMaxVal = 100;

    private void Awake()
    {
    }

    void Start()
    {

    }

    void Update()
    {
        SliderCtrl();
    }

    void SliderCtrl()
    {
        happyValue = Mathf.Min(happyMaxVal, happyValue);
        happySlider.fillAmount = happyValue / happyMaxVal;
    }
}
