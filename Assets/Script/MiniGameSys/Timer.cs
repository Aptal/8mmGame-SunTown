using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI uiTimeText;

    public int Duration { get; private set; }

    public int remainingDuration;

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
    public Canvas targetCanvas;
    public void End()
    {
        ResetTimer();
        
        PlayerPrefs.SetInt("MiniGameGotSun", GameManager.Instance.hubTile.totalSun);
        PlayerPrefs.Save();
        //PauseButton();
        GameManager.Instance.StopAllAction();
        GameManager.Instance.gameObject.SetActive(false);
        targetCanvas.gameObject.SetActive(true);
        //SceneManager.LoadScene(1);
    }

    public void GetSunButton()
    {
        GameManager.Instance.hubTile.totalSun = 500;
        End();
    }

    
    public void FinishButton()
    {
        PlayerPrefs.SetString("SceneInfo", "FinishMiniGame");
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        StopAllCoroutines() ;
    }
}
