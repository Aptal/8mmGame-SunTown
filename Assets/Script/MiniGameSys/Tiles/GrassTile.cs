using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    public bool isSunny = true;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 5;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞的对象是否是阴影板
        if (other.CompareTag("Shadow"))
        {
            // 草地被阴影遮挡，设置为无阳光照射
            isSunny = false;
            spriteRenderer.color = Color.blue;
        }
    }

    // 当阴影板离开草地时调用
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shadow"))
        {
            // 草地恢复阳光
            isSunny = true;
            spriteRenderer.color = Color.white;
        }
    }

    //private void OnMouseUpAsButton()
    //{
    //    // 如果有旗帜拖拽到草地，则恢复为羊
    //    Unit flagSheep = FindObjectOfType<Unit>(); // 获取当前场景中的羊
    //    if (flagSheep != null && flagSheep.isFlag)
    //    {
    //        flagSheep.PlaceOnGrassTile(transform.position); // 将旗帜放置到草地上
    //    }
    //}
}
