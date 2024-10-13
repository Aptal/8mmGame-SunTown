using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    //[SerializeField] private Sprite[] sprites;

    public bool isSunny = true;
    public bool canGo = false;
    public Color highlightColor;
    public LayerMask obLayerMask;
    static private int debugcnt = 0;

    void Start()
    {
        //Debug.Log("road : " + debugcnt++);
        spriteRenderer = GetComponent<SpriteRenderer>();

        CheckObstacle();
    }

    protected void OnMouseEnter()
    {
        transform.localScale += Vector3.one * 0.25f;
        //spriteRenderer.sortingOrder = 25;
    }

    protected void OnMouseExit()
    {
        transform.localScale -= Vector3.one * 0.25f;
        //spriteRenderer.sortingOrder = 0;
    }

    protected void CheckObstacle()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x, obLayerMask);
        //if(collider != null) //no cross
        //canGo = true;
    }

    public void HighlightTile()
    {
        if(canGo)
            spriteRenderer.color = highlightColor;
        else 
            spriteRenderer.color = Color.white;
    }

    public void ResetTile()
    {
        spriteRenderer.color = Color.white;
    }

    protected void OnMouseDown()
    {
        if (canGo && GameManager.Instance.selectedUnit != null && GameManager.Instance.selectedUnit.canCtrl)
            GameManager.Instance.selectedUnit.Move(this.transform);
    }
}