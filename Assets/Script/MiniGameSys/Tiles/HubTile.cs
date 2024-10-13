using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubTile : Tile
{
    //static int hub = 0;
    [SerializeField]
    public int sheep2hubV = 3;
    [SerializeField]
    private int totalSun = 0;

    private void Start()
    {
        //Debug.Log("hub : " + hub++);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ReceiveSun(int sheepHasSun)
    {
        int pass = Mathf.Min(sheepHasSun, sheep2hubV);
        totalSun += pass;
        return pass;
    }
}
