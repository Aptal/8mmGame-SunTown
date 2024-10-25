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

    [SerializeField] private AudioClip least30Sound;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource audioSource;

    private void Awake()
    {
        uiTimeText.text = "00:00";
        Duration = remainingDuration = 0;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.7f;
    }

    private void ResetTimer()
    {
        uiTimeText.text = "00:00";
        Duration = remainingDuration = 0;
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
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
            if(remainingDuration <= 30)
            {
                audioSource.PlayOneShot(least30Sound);
            }
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
        ResetTimer();
        PlayerPrefs.SetInt("MiniGameGotSun", GameManager.Instance.hubTile.totalSun);
    }

    private void OnDestroy()
    {
        StopAllCoroutines() ;
    }
}
