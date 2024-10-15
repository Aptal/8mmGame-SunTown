using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SunSheepCtrl : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        canCtrl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 25;
        transform.position = GameManager.Instance.grassTiles[Random.Range(0, 8)].transform.position;
        sheepSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        sheepSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        sheepSunCnt.text = hasSun.ToString();
        if (preType == PosType.errorType)
            preType = GetCurInfo();
        totalTime += Time.deltaTime;
        // 每秒生产光能

        if(totalTime >= 1)
        {            
            totalTime = 0;

            curType = GetCurInfo();
            
            if(curType == PosType.store && preType == PosType.store)
            {
                if(!GameManager.Instance.storeTiles[posIndex].pushButtion.IsActive())
                    GameManager.Instance.storeTiles[posIndex].EnableButtion();
                if (GameManager.Instance.storeTiles[posIndex].opt == StoreOpt.push)
                {
                    hasSun -= GameManager.Instance.storeTiles[posIndex].StoreSun(hasSun);
                }
                else if (GameManager.Instance.storeTiles[posIndex].opt == StoreOpt.pop)
                {
                    hasSun += GameManager.Instance.storeTiles[posIndex].ExtractSun(sunLimit - hasSun);
                }
            }   
            else
            {
                for(int i = 0; i < 4; ++i)
                {
                    if (GameManager.Instance.storeTiles[i].pushButtion.IsActive())
                        GameManager.Instance.storeTiles[i].DisableButtion();
                }
                if (curType == PosType.grass && preType == PosType.grass && GameManager.Instance.grassTiles[posIndex].isSunny)
                {
                    hasSun = Mathf.Min(hasSun + productV, sunLimit);
                }
                else if (curType == PosType.hub && preType == PosType.hub)
                {
                    hasSun -= GameManager.Instance.hubTile.ReceiveSun(hasSun);
                }
            }

            

            preType = curType;
        }
    }
}
