using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSheepCtrl : Unit
{
    void Start()
    {
        canCtrl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sheepSprite;
        spriteRenderer.sortingOrder = 25;
        sheepCollider = GetComponent<Collider2D>();
        sheepSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);

    }

    void Update()
    {
        sheepSunCnt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        sheepSunCnt.text = hasSun.ToString();
        if (preType == PosType.errorType)
            preType = GetCurInfo();
        totalTime += Time.deltaTime;
        // 每秒生产光能

        if (totalTime >= 1)
        {
            totalTime = 0;

            curType = GetCurInfo();

            if (curType == PosType.store && preType == PosType.store)
            {

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
                if (curType == PosType.grass && preType == PosType.grass && (!GameManager.Instance.grassTiles[posIndex].isSunny) && !isFlag)
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
