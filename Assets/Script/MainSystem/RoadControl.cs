using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControl : MonoBehaviour
{
    public List<(int ID, int U, int V)> RoadList = new List<(int ID, int U, int V)>();
    public int badRoadCnt = 0;
    public bool hasFix = false;

    public (int i, int u, int v) RandomDestroyRoad()
    {
        int desRoadIndex = Random.Range(0, 24);
        int destroyRoadPos1 = -1, destroyRoadPos2 = -1;
        switch (desRoadIndex)
        {
            case 0: destroyRoadPos1 = 0; destroyRoadPos2 = 1; break;
            case 1: destroyRoadPos1 = 1; destroyRoadPos2 = 2; break;
            case 2: destroyRoadPos1 = 0; destroyRoadPos2 = 3; break;
            case 3: destroyRoadPos1 = 0; destroyRoadPos2 = 8; break;
            case 4: destroyRoadPos1 = 1; destroyRoadPos2 = 8; break;
            case 5: destroyRoadPos1 = 1; destroyRoadPos2 = 9; break;
            case 6: destroyRoadPos1 = 2; destroyRoadPos2 = 9; break;
            case 7: destroyRoadPos1 = 2; destroyRoadPos2 = 4; break;
            case 8: destroyRoadPos1 = 3; destroyRoadPos2 = 8; break;
            case 9: destroyRoadPos1 = 8; destroyRoadPos2 = 9; break;
            case 10: destroyRoadPos1 = 4; destroyRoadPos2 = 9; break;
            case 11: destroyRoadPos1 = 3; destroyRoadPos2 = 5; break;
            case 12: destroyRoadPos1 = 3; destroyRoadPos2 = 10; break;
            case 13: destroyRoadPos1 = 8; destroyRoadPos2 = 10; break;
            case 14: destroyRoadPos1 = 9; destroyRoadPos2 = 11; break;
            case 15: destroyRoadPos1 = 4; destroyRoadPos2 = 11; break;
            case 16: destroyRoadPos1 = 4; destroyRoadPos2 = 7; break;
            case 17: destroyRoadPos1 = 5; destroyRoadPos2 = 10; break;
            case 18: destroyRoadPos1 = 11; destroyRoadPos2 = 10; break;
            case 19: destroyRoadPos1 = 11; destroyRoadPos2 = 7; break;
            case 20: destroyRoadPos1 = 10; destroyRoadPos2 = 6; break;
            case 21: destroyRoadPos1 = 11; destroyRoadPos2 = 6; break;
            case 22: destroyRoadPos1 = 5; destroyRoadPos2 = 6; break;
            case 23: destroyRoadPos1 = 6; destroyRoadPos2 = 7; break;
            default: break;
        }
        if (destroyRoadPos1 > destroyRoadPos2)
        {
            int temp = destroyRoadPos1;
            destroyRoadPos1 = destroyRoadPos2;
            destroyRoadPos2 = temp;
        }
        return (desRoadIndex, destroyRoadPos1, destroyRoadPos2);
    }

    public void AddRoad()
    {
        RoadList.Clear();
        HashSet<int> addedRoads = new HashSet<int>();

        while (RoadList.Count < badRoadCnt)
        {
            var road = RandomDestroyRoad();
            if (!addedRoads.Contains(road.i))
            {
                RoadList.Add(road);
                addedRoads.Add(road.i);
            }
        }
    }

    public void FixRoad(int id)
    {
        if (!hasFix)
            RoadList.RemoveAll(item => item.ID == id);
        hasFix = true;
    }
}
