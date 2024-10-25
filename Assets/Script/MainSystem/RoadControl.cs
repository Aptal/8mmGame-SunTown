using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControl : MonoBehaviour
{
    public List<(int ID, int U, int V)> RoadList;
    public int badRoadCnt = 0;


    public (int i, int u, int v) RandomDestoryRoad()
    {
        int desRoadIndex = Random.Range(0, 24);
        int destoryRoadpos1 = -1, destoryRoadpos2 = -1;
        switch (desRoadIndex)
        {
            case 0:
                destoryRoadpos1 = 0; destoryRoadpos2 = 1; break;
            case 1:
                destoryRoadpos1 = 1; destoryRoadpos2 = 2; break;
            case 2:
                destoryRoadpos1 = 0; destoryRoadpos2 = 3; break;
            case 3:
                destoryRoadpos1 = 0; destoryRoadpos2 = 8; break;
            case 4:
                destoryRoadpos1 = 1; destoryRoadpos2 = 8; break;
            case 5:
                destoryRoadpos1 = 1; destoryRoadpos2 = 9; break;
            case 6:
                destoryRoadpos1 = 2; destoryRoadpos2 = 9; break;
            case 7:
                destoryRoadpos1 = 2; destoryRoadpos2 = 4; break;
            case 8:
                destoryRoadpos1 = 3; destoryRoadpos2 = 8; break;
            case 9:
                destoryRoadpos1 = 8; destoryRoadpos2 = 9; break;
            case 10:
                destoryRoadpos1 = 4; destoryRoadpos2 = 9; break;
            case 11:
                destoryRoadpos1 = 3; destoryRoadpos2 = 5; break;
            case 12:
                destoryRoadpos1 = 3; destoryRoadpos2 = 10; break;
            case 13:
                destoryRoadpos1 = 8; destoryRoadpos2 = 10; break;
            case 14:
                destoryRoadpos1 = 9; destoryRoadpos2 = 11; break;
            case 15:
                destoryRoadpos1 = 4; destoryRoadpos2 = 11; break;
            case 16:
                destoryRoadpos1 = 4; destoryRoadpos2 = 7; break;
            case 17:
                destoryRoadpos1 = 5; destoryRoadpos2 = 10; break;
            case 18:
                destoryRoadpos1 = 11; destoryRoadpos2 = 10; break;
            case 19:
                destoryRoadpos1 = 11; destoryRoadpos2 = 7; break;
            case 20:
                destoryRoadpos1 = 10; destoryRoadpos2 = 6; break;
            case 21:
                destoryRoadpos1 = 11; destoryRoadpos2 = 6; break;
            case 22:
                destoryRoadpos1 = 5; destoryRoadpos2 = 6; break;
            case 23:
                destoryRoadpos1 = 6; destoryRoadpos2 = 7; break;
            default:
                break;
        }
        if (destoryRoadpos1 > destoryRoadpos2)
        {
            int temp = destoryRoadpos1;
            destoryRoadpos1 = destoryRoadpos2;
            destoryRoadpos2 = temp;
        }
        return (desRoadIndex, destoryRoadpos1, destoryRoadpos2);
    }

    public void AddRoad()
    {
        RoadList.Clear();
        for(int i = 0; i < badRoadCnt; i++)
        {
            RoadList.Add(RandomDestoryRoad());
        }
    }

    public void FixRoad(int id)
    {
        RoadList.RemoveAll(item => item.ID == id);
    }
}
