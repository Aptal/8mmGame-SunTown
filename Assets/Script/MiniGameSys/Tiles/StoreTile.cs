using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum StoreOpt
{
    disable,
    push,
    pop
};

public class StoreTile : Tile
{
    [SerializeField]
    private int stSunLimit = 20;
    [SerializeField]
    private int sthasSun = 0;
    [SerializeField]
    private TextMeshProUGUI storeSunCnt;//显示当前临时仓库存储阳光数量

    [SerializeField]
    private int sheep2storeV = 1;
    [SerializeField]
    private int store2sheepV = 1;
    //控制存储、取出交互按钮
    [SerializeField]
    public Button pushButtion;
    [SerializeField]
    public Button popButtion;

    public StoreOpt opt = StoreOpt.disable;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //DisableButtion();
        //Vector3 f = new Vector3(2, 0, 0);
        //storeSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        //pullButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position) - f;
        //pushButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position) + f;
        //Debug.Log("pull "+pullButtion.transform.position);
        //Debug.Log("push " + pushButtion.transform.position);
        storeSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    void Update()
    {
        storeSunCnt.text = sthasSun.ToString();
    }

    public void EnablePopButtion()
    {
        opt = StoreOpt.pop;
    }

    public void EnablePushButtion()
    {
        opt = StoreOpt.push;
    }

    public void EnableButtion()
    {
        pushButtion.gameObject.SetActive(true);
        popButtion.gameObject.SetActive(true);
        Vector3 f = new Vector3(50, 50, 0);
        popButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position) - f;
        pushButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position) + f;
    }

    public void DisableButtion()
    {
        popButtion.gameObject.SetActive(false);
        pushButtion.gameObject.SetActive(false);
    }

    public int StoreSun(int sheepHasSun)
    {
        int ret = 0;
        int pass = Mathf.Min(sheepHasSun, sheep2storeV);
        if (sthasSun + pass <= stSunLimit)
        {
            ret = pass;
        }
        else
        {
            ret = stSunLimit - sthasSun;
        }
        sthasSun += ret;
        return ret;
    }

    public int ExtractSun(int sheepNeedSun)
    {
        int ret = 0;
        int pass = Mathf.Min(store2sheepV, sheepNeedSun);
        if(pass <= sthasSun)
        {
            ret = pass;
        }   
        else
        {
            ret = sthasSun;
        }
        sthasSun -= ret;
        return ret;
    }
}
