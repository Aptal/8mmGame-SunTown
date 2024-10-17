using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI uiTimeText;

    public int Duration { get; private set; }

    [Header("游戏持续时间")]
    [SerializeField]
    private int remainingDuration;

    private void Awake()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        uiTimeText.text = "00:00";
        Duration = remainingDuration = 0;
    }

    public Timer SetDuration(int seconds)
    {
        Duration = remainingDuration = seconds;
        return this;
    }

    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingDuration > 0)
        {
            UpdateUI(remainingDuration);
            remainingDuration--;
            Duration = remainingDuration;
            yield return new WaitForSeconds(1f);
        }
        End();
    }

    private void UpdateUI(int seconds)
    {
        uiTimeText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
    }

    public void End()
    {
        ResetTimer() ;
    }

    private void OnDestroy()
    {
        StopAllCoroutines() ;
    }
}
