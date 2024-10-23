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
    private TextMeshProUGUI storeSunCnt;//��ʾ��ǰ��ʱ�ֿ�洢��������

    [SerializeField]
    private int sheep2storeV = 1;
    [SerializeField]
    private int store2sheepV = 1;
    //���ƴ洢��ȡ��������ť
    [SerializeField]
    public Button pushButtion;
    [SerializeField]
    public Button popButtion;

    public StoreOpt opt = StoreOpt.disable;

    [SerializeField]
    public AudioClip noWorkSound;
    [SerializeField]
    protected AudioClip workSound;



    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = open2Close[open2Close.Length - 1];
        storeSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    void Update()
    {
        storeSunCnt.text = sthasSun.ToString();
    }

    public void EnablePopButtion()
    {
        opt = StoreOpt.pop;
        audioSource.PlayOneShot(workSound);
    }

    public void EnablePushButtion()
    {
        opt = StoreOpt.push;
        audioSource.PlayOneShot(workSound);
    }

    private void EnableButtion()
    {
        pushButtion.gameObject.SetActive(true);
        popButtion.gameObject.SetActive(true);
        Vector3 f = new Vector3(50, 0, 0);
        popButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position) - f;
        pushButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position) + f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //��Ⱥ������ʱ�ֿ�
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.activeSelf)
            {
                StartCoroutine(Close2Open(changingTime));
                if(spriteRenderer.sprite == open2Close[0])
                {
                    EnableButtion();
                }
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        //��Ⱥ�뿪��ʱ�ֿ�
        if (other.CompareTag("Player"))
        {
            if(this.gameObject.activeSelf)
            {
                StartCoroutine(Open2Close(changingTime));
                if(spriteRenderer.sprite == open2Close[open2Close.Length - 1])
                {
                    DisableButtion();
                }
            }
        }
    }

    private void DisableButtion()
    {
        popButtion.gameObject.SetActive(false);
        pushButtion.gameObject.SetActive(false);
        opt = StoreOpt.disable;
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
