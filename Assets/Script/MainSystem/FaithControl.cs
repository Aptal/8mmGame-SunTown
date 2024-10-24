using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaithControl : MonoBehaviour
{
    public Image faithSlider;

    public float faithValue = 25;
    public float faithMaxVal = 100;

    private void Awake()
    {
        //faithSlider = transform.GetChild(3).GetComponent<Image>();
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
        faithValue = Mathf.Min(faithMaxVal, faithValue);
        faithSlider.fillAmount = faithValue / faithMaxVal;
    }
}
