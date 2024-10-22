using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    public bool isSunny = true;
    public Color shadowColor = Color.gray;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 5;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �����ײ�Ķ����Ƿ�����Ӱ��
        if (other.CompareTag("Shadow"))
        {
            // �ݵر���Ӱ�ڵ�������Ϊ����������
            isSunny = false;
            spriteRenderer.color = shadowColor;
        }
    }

    // ����Ӱ���뿪�ݵ�ʱ����
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shadow"))
        {
            // �ݵػָ�����
            isSunny = true;
            ResetTile();
        }
    }
    public void ResetTile()
    {
        if (isSunny)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = shadowColor;
        canGo = false;
    }
    //private void OnMouseUpAsButton()
    //{
    //    // �����������ק���ݵأ���ָ�Ϊ��
    //    Unit flagSheep = FindObjectOfType<Unit>(); // ��ȡ��ǰ�����е���
    //    if (flagSheep != null && flagSheep.isFlag)
    //    {
    //        flagSheep.PlaceOnGrassTile(transform.position); // �����ķ��õ��ݵ���
    //    }
    //}
}
