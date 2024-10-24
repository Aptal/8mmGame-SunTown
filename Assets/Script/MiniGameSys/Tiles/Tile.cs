using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    [SerializeField] protected Sprite[] open2Close;
    protected float changingTime = 2.0f;

    public bool canGo = false;
    protected Color highlightColor = Color.yellow;
    public LayerMask obLayerMask;

    public List<int> arrivePos = new List<int>();
    public int id = -1;

    public float animationSpeed = 12f;

    protected AudioSource audioSource; // 用于播放音频
    [SerializeField] protected AudioClip sheepArriveSound;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //CheckObstacle();
    }

    protected void OnMouseEnter()
    {
        if(spriteRenderer.sortingOrder < 10)
        {
            transform.localScale += Vector3.one * 0.25f;
            spriteRenderer.sortingOrder = 10;
        }
    }

    protected void OnMouseExit()
    {
        if(spriteRenderer.sortingOrder >= 10)
        {
            transform.localScale -= Vector3.one * 0.25f;
            spriteRenderer.sortingOrder = 5;
        }
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
        canGo = false;
    }

    protected IEnumerator Close2Open(float duration)
    {
        float elapsedTime = 0f;
        int totalSprites = open2Close.Length;

        // 确保仓库是关闭的，初始化为最后一帧的 sprite
        spriteRenderer.sprite = open2Close[totalSprites - 1];

        // 在指定的持续时间内逐步更改精灵
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 计算当前精灵索引
            int currentSpriteIndex = Mathf.FloorToInt((1 - t) * (totalSprites - 1));
            spriteRenderer.sprite = open2Close[currentSpriteIndex];

            yield return null; // 等待下一帧
        }

        // 确保动画结束时使用最后一帧
        spriteRenderer.sprite = open2Close[0];
    }

    protected IEnumerator Open2Close(float duration)
    {
        float elapsedTime = 0f;
        int totalSprites = open2Close.Length;

        // 确保仓库是关闭的，初始化为最后一帧的 sprite
        spriteRenderer.sprite = open2Close[0];

        // 在指定的持续时间内逐步更改精灵
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 计算当前精灵索引
            int currentSpriteIndex = Mathf.FloorToInt(t * (totalSprites - 1));
            spriteRenderer.sprite = open2Close[currentSpriteIndex];

            yield return null; // 等待下一帧
        }

        // 确保动画结束时使用最后一帧
        spriteRenderer.sprite = open2Close[totalSprites - 1];
    }

}