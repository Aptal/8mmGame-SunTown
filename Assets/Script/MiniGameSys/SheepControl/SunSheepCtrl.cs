using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSheepCtrl : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        canCtrl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GameManager.Instance.grassTiles[Random.Range(0, 8)].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (preType == PosType.errorType)
            preType = GetCurInfo();
        totalTime += Time.deltaTime;
        // ÿ����������
        if(totalTime >= 1)
        {            
            totalTime = 0;

            curType = GetCurInfo();
            if(curType == PosType.grass && preType == PosType.grass)
            {
                hasSun = Mathf.Min(hasSun + productV, sunLimit);
            }

            if(curType == PosType.store && preType == PosType.store)
            {
                hasSun -= GameManager.Instance.storeTiles[posIndex].StoreSun(hasSun);
            }

            if(curType == PosType.hub && preType == PosType.hub)
            {
                hasSun -= GameManager.Instance.hubTile.ReceiveSun(hasSun);
            }

            preType = curType;
        }
    }
}
