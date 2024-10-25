using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneJump : MonoBehaviour
{
    //ͨ�������ťʵ�ֽ�����ת
    public void JumpToMainScene()
    {
        SceneManager.LoadScene(1);

    }
    public void JumpToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void JumpToMiniGame()
    {
        SceneManager.LoadScene(2);
    }
    // 动画事件调用的函数
    public void JumpToMiniGameLater()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }
    // 协程用于延迟场景加载
    private System.Collections.IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}