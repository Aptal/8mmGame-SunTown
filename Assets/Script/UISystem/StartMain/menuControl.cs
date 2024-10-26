using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuControl : MonoBehaviour
{
    //begin game
    public void StartGame()
    {
        DeleteFile("Assets/Resources/UpdateData/SaveData/Data1.txt");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //continue game
    public void ContinueGame()
    {
        Debug.Log("data 1");
        TextAsset data1 = Resources.Load<TextAsset>("UpdateData/SaveData/Data1");
        if (data1 == null)
        {
            StartGame();
        }
        else 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    //game settings

    //exit game
    public void ExitGame()
    {
        Application.Quit();
    }

    public bool DeleteFile(string path)
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            Debug.LogError("删除文件失败，没有足够权限访问文件路径：" + path);
            return false;
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("删除文件失败，文件不存在：" + path);
            return false;
        }
    }
}
