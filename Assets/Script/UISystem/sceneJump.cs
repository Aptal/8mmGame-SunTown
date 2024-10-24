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
}
