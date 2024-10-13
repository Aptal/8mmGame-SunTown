using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    static int grass = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //private void OnMouseDown()
    //{
    //    Debug.Log(123);
    //}
}
