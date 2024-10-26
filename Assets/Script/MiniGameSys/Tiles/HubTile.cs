using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HubTile : Tile
{
    //static int hub = 0;
    [SerializeField]
    public int sheep2hubV = 3;
    [SerializeField]
    public int totalSun = 0;

    [SerializeField]
    protected AudioClip gameInSound;

    private void Start()
    {
        //Debug.Log("hub : " + hub++);
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(gameInSound);
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.showCnt.text = totalSun.ToString();
    }

    public int ReceiveSun(int sheepHasSun)
    {
        int pass = Mathf.Min(sheepHasSun, sheep2hubV);
        totalSun += pass;
        return pass;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sheepArriveSound);
        }
    }
}
