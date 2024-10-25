using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameData : MonoBehaviour
{
}

// sheep data
public class UnitData
{
    public float moveSpeed;
    public int productV;
    public int sunLimit;

    public int[] sheepCnt;

    public UnitData()
    {
        moveSpeed = 0f;
        productV = 0;
        sunLimit = 0;

        sheepCnt = new int[3];
    }

    public UnitData(float moveSpeed, int productV, int sunLimit, int[] sheepCnt)
    {
        this.moveSpeed = moveSpeed;
        this.productV = productV;
        this.sunLimit = sunLimit;

        this.sheepCnt = sheepCnt;
    }
}

// store data
public class StoreData
{
    public int storeLimit;
    public int pushSpeed;
    public int popSpeed;

    public StoreData()
    {
        storeLimit = 0;
        pushSpeed = 0;
        popSpeed = 0;
    }

    public StoreData(int storeLimit, int pushSpeed, int popSpeed)
    {
        this.storeLimit = storeLimit;
        this.pushSpeed = pushSpeed;
        this.popSpeed = popSpeed;
    }
}

public class HubData
{
    public int sheep2hubSpeed;
    public int gotSun;

    public HubData()
    {
        sheep2hubSpeed = 0;
        gotSun = 0;
    }

    public HubData(int sheep2hubSpeed)
    {
        this.sheep2hubSpeed = sheep2hubSpeed;
    }
}

public class RoadData
{
    public List<(int ID, int U, int V)> RoadList;

    public RoadData()
    {
        RoadList = new List<(int ID, int U, int V)>();
    }

    public RoadData(List<(int ID, int U, int V)> roadList)
    {
        RoadList = roadList;
    }
}