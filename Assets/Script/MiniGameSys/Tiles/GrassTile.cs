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
        // �����ײ�Ķ����Ƿ�����Ӱ��
        if (other.CompareTag("Shadow"))
        {
            // �ݵر���Ӱ�ڵ�������Ϊ����������
            isSunny = false;
            spriteRenderer.color = Color.blue;
        }
    }

    // ����Ӱ���뿪�ݵ�ʱ����
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shadow"))
        {
            // �ݵػָ�����
            isSunny = true;
            spriteRenderer.color = Color.white;
        }
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
