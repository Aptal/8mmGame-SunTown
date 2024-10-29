using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempleControl : MonoBehaviour
{
    public Image weatherImg;
    public Sprite normalSprite;

    public int nowWeatherType = -1;
    public int dayweatherType = -1;

    public void SunnyButton()
    {
        if (TimeControl.Instance.faithCtrl.faithValue < 50) return;
        nowWeatherType = 0;
        ConfirmWeatherButton();
    }

    public void ShadowButton()
    {
        if (TimeControl.Instance.faithCtrl.faithValue < 50) return;
        nowWeatherType = 1;
        ConfirmWeatherButton();
    }

    public void RainButton()
    {
        if (TimeControl.Instance.faithCtrl.faithValue < 50) return;
        nowWeatherType = 2;
        ConfirmWeatherButton();
    }

    public void ConfirmWeatherButton()
    {
        weatherImg.sprite = TimeControl.Instance.weatherSprites[nowWeatherType];
    }

    public void ApplyButton()
    {
        TimeControl.Instance.weatherType = nowWeatherType;
        TimeControl.Instance.UpdateWeather();
    }

    public void CancelButton()
    {
        TimeControl.Instance.weatherType = dayweatherType;
        weatherImg.sprite = normalSprite;
        TimeControl.Instance.UpdateWeather();
    }

    public void TempleButton()
    {
        dayweatherType = TimeControl.Instance.weatherType;
    }
}
