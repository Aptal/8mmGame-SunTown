using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //begin game
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //通过点击按钮实现界面跳转
    public void Jump()
    {
        SceneManager.LoadScene(1);
    }
    //continue game

    //game settings

    //exit game
    public void ExitGame()
    {
        Application.Quit();
    }
}
