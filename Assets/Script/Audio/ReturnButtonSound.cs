using UnityEngine;
public class ReturnButtonScript : MonoBehaviour
{
    public AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.Find("AudioManagerObject").GetComponent<AudioManager>();
    }
    public void OnReturnButtonClick()
    {
        // 触发事件，这样会调用AudioManager中的PlayAudio函数
        audioManager.onReturnButtonClicked.Invoke();
        // 这里可以继续执行禁用面板等其他操作
        // 假设panel是返回键所在的面板对象
        GameObject panel = transform.parent.gameObject;
        panel.SetActive(false);
    }
}