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
    public int stSunLimit = 20;
    [SerializeField]
    private int sthasSun = 0;
    [SerializeField]
    private TextMeshProUGUI storeSunCnt;//显示当前临时仓库存储阳光数量

    public int sheep2storeV = 1;
    public int store2sheepV = 1;
    //控制存储、取出交互按钮
    [SerializeField] public Button pushButtion;
    [SerializeField] public Button popButtion;

    [SerializeField] private Sprite[] pushSunFrame;
    [SerializeField] private Sprite[] popSunFrame;
    [SerializeField] private Image workingImg;
    protected float totalTime = 0;

    public StoreOpt opt = StoreOpt.disable;
    public bool hasSheep = false;

    [SerializeField]
    public AudioClip noWorkSound;
    [SerializeField]
    protected AudioClip workSound;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = open2Close[open2Close.Length - 1];

        Vector3 f = new Vector3(-0.5f, -1, 0);
        storeSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position - f);

        Vector3 f1 = new Vector3(1.2f, 0, 0);
        Vector3 f2 = new Vector3(1.2f, -1f, 0);
        popButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position - f1);
        pushButtion.transform.position = Camera.main.WorldToScreenPoint(transform.position - f2);

        Vector3 f3 = new Vector3(0, -2f, 0);
        workingImg.transform.position = Camera.main.WorldToScreenPoint(transform.position - f3);
        audioSource.volume = 0.05f;
    }

    void Update()
    {
        storeSunCnt.text = sthasSun.ToString();
        if (hasSheep && !pushButtion.gameObject.activeSelf)
        {
            EnableButtion();
        }
        else if(!hasSheep && pushButtion.gameObject.activeSelf)
        {
            DisableButtion();
        }

        totalTime += Time.deltaTime;
        if(totalTime >= 1) totalTime = 0;

        if (opt == StoreOpt.pop)
        {
            PlayAnimation(popSunFrame);
            
        }
        else if (opt == StoreOpt.push)
        {
            PlayAnimation(pushSunFrame);
        }
        else
        {
            workingImg.gameObject.SetActive(false);
        }
    }

    protected void PlayAnimation(Sprite[] frames)
    {
        //isPlayingAnimation = true;
        Debug.Log("store play animation");
        if(workingImg.gameObject && !workingImg.gameObject.activeSelf)
            workingImg.gameObject.SetActive(true);
        int frameIndex = (int)(totalTime * animationSpeed) % frames.Length;
        workingImg.sprite = frames[frameIndex];
        //StartCoroutine(PlayAnimationCoroutine(frames));
    }

    IEnumerator PlayAnimationCoroutine(Sprite[] frames)
    {
        if (frames != null && frames.Length > 0)
        {
            float animationDuration = frames.Length / animationSpeed;
            float elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                int frameIndex = (int)(elapsedTime * animationSpeed) % frames.Length;
                workingImg.sprite = frames[frameIndex];
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        //isPlayingAnimation = false;
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //羊群到达临时仓库
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.activeSelf)
            {
                StartCoroutine(Close2Open(changingTime));
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        //羊群离开临时仓库
        if (other.CompareTag("Player"))
        {
            if(this.gameObject.activeSelf)
            {
                StartCoroutine(Open2Close(changingTime));
            }
        }
    }

    private void DisableButtion()
    {
        StopAllCoroutines();
        popButtion.gameObject.SetActive(false);
        pushButtion.gameObject.SetActive(false);
        workingImg.gameObject.SetActive(false);
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
