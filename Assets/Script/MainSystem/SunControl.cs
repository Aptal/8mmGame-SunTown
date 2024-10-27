using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SunControl : MonoBehaviour
{
    public int totalSun = 200;
    public TextMeshProUGUI thsSun;
    public TextMeshProUGUI hunSun;
    public TextMeshProUGUI tenSun;
    public TextMeshProUGUI oneSun;

    public int sunIncome;
    public int sunOutcome;

    private void Update()
    {
        UpdateSunCnt();
    }

    public void UpdateSunCnt()
    {
        int thousands = totalSun / 1000;
        int hundreds = (totalSun % 1000) / 100;
        int tens = ((totalSun % 1000) % 100) / 10;
        int ones = ((totalSun % 1000) % 100) % 10;

        thsSun.text = thousands.ToString();
        hunSun.text = hundreds.ToString();
        tenSun.text = tens.ToString();
        oneSun.text = ones.ToString();

    }

    public bool QueryUp(int cost)
    {
        return cost <= totalSun;
    }

    public bool CostSun(int costSun)
    {
        if(costSun > totalSun) 
            return false;

        totalSun -= costSun;
        UpdateSunCnt();

        return true;
    }
}
