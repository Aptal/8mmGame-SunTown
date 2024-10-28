using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;


public enum PosType
{
    errorType,
    hub,
    store,
    grass
}

public class Unit : MonoBehaviour
{
    public float moveSpeed = 0.6f;
    [Header("kStore(羊在草地和仓库间的系数，speed*k)")]

    protected float kStore = 0.8f;
    [Header("kHub(羊在中枢和仓库间的系数，speed*k)")]

    protected float kHub = 0.6f;
    protected Coroutine moveCoroutine;

    public int productV = 1;

    public int sunLimit = 10;

    [SerializeField]
    public int hasSun = 0;
    public TextMeshProUGUI sheepSunCnt;//显示当前羊群携带阳光数量

    public int sheepCnt = 6;

    public int posIndex = -2;
    public PosType preType = PosType.errorType;
    public PosType curType = PosType.errorType;
    protected float totalTime = 0;

    public SpriteRenderer spriteRenderer { get; protected set; }

    [SerializeField] protected Sprite sheepSprite;  // 原羊的图片

    public float animationSpeed = 12f;
    [SerializeField] public Sprite[] chargingFrame;
    [SerializeField] public Sprite[] eatingFrame;
    [SerializeField] public Sprite[] movingFrame;

    [SerializeField] SheepBumpCtrl bumpFrame;

    [SerializeField] protected Sprite dieSheepSprite;   // 复活羊的图片
    //[SerializeField] protected Sprite[] reLifeSheepFrame;

    [SerializeField] protected Sprite flagSprite;   // 旗帜的图片
    //[SerializeField] protected Sprite[] flagFrame; 

    public bool isFlag = false;  // 是否处于旗帜状态
    public bool isBeingDragged = false; // 是否正在被拖动

    protected Vector3 stopPosition; // 记录碰撞时的位置
    protected Collider2D sheepCollider;

    public bool canCtrl = true;
    public bool isSelected = false;
    public Color selectedColor = Color.yellow;

    // 音乐设置
    [SerializeField] protected AudioClip selectSound; // 选中sheep
    [SerializeField] protected AudioClip moveSound; // 移动sheep
    [Range(0.0f, 1.0f)] // 在编辑器中添加滑块来调整音量
    private float moveSoundVolume = 0.5f; // 移动声音的默认音量

    [SerializeField] protected AudioClip crashSound;
    [SerializeField] protected AudioClip reLifeSound;


    protected AudioSource audioSource; // 用于播放音频

    public void StopSounds()
    {
        if(audioSource != null)
            audioSource.Stop();
    }

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.25f;
    }

    void Start()
    {
        canCtrl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sheepCollider = GetComponent<Collider2D>();
    }


    protected void OnMouseEnter()
    {
        if (!canCtrl) return;
        if(spriteRenderer.sortingOrder > 10)
            transform.localScale += Vector3.one * 0.1f;
    }

    protected void OnMouseExit()
    {
        if (!canCtrl) return;
        if(spriteRenderer.sortingOrder > 10)
            transform.localScale -= Vector3.one * 0.1f;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        Unit otherSheep = other.GetComponent<Unit>();

        // 确保两个碰撞对象都是羊群，且当前羊群没有变成旗帜
        if (otherSheep != null && !isFlag && !otherSheep.isFlag && !isPlayingCollisionAnimation && !otherSheep.isPlayingCollisionAnimation)
        {
            HandleCollisionWithOtherSheep(otherSheep);
        }
    }

    // 是否正在播放碰撞动画
    public bool isPlayingCollisionAnimation = false;

    // 播放碰撞动画
    public void PlayCollisionAnimation(Unit otherSheep)
    {
        //canCtrl = false;
        //otherSheep.canCtrl = false;

        // 根据碰撞的羊的类型确定要播放的碰撞动画帧
        Sprite[] collisionFrames = null;
        if (this is SunSheepCtrl && otherSheep is SunSheepCtrl)
        {
            collisionFrames = bumpFrame.sunSunBoomFrame;
        }
        else if (this is SunSheepCtrl && otherSheep is RunSheepCtrl)
        {
            collisionFrames = bumpFrame.sunRunBoomFrame;
        }
        else if (this is SunSheepCtrl && otherSheep is ShadowSheepCtrl)
        {
            collisionFrames = bumpFrame.sunShadowBoomFrame;
        }
        else if (this is ShadowSheepCtrl && otherSheep is RunSheepCtrl)
        {
            collisionFrames = bumpFrame.shadowRunBoomFrame;
        }
        else if (this is ShadowSheepCtrl && otherSheep is ShadowSheepCtrl)
        {
            collisionFrames = bumpFrame.shadowShadowBoomFrame;
        }
        else if (this is RunSheepCtrl && otherSheep is RunSheepCtrl)
        {
            collisionFrames = bumpFrame.runRunBoomFrame;
        }

        Unit sheepToRevive = Random.value > 0.5f ? this : otherSheep;
        sheepToRevive.isPlayingCollisionAnimation = true;
        if(sheepToRevive.moveCoroutine != null)
        {
            StopCoroutine(sheepToRevive.moveCoroutine);
        }
        StartCoroutine(PlayCollisionAnimationCoroutine(collisionFrames, sheepToRevive));
    }

    IEnumerator PlayCollisionAnimationCoroutine(Sprite[] collisionFrames, Unit sheepToRevive)
    {
        if (collisionFrames != null && collisionFrames.Length > 0)
        {
            float animationDuration = collisionFrames.Length / animationSpeed;
            float elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                int frameIndex = (int)(elapsedTime * animationSpeed) % collisionFrames.Length;
                sheepToRevive.spriteRenderer.sprite = collisionFrames[frameIndex];
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        yield return StartCoroutine(ExecuteAfterAnimation(sheepToRevive));
    }

    IEnumerator ExecuteAfterAnimation(Unit sheepToRevive)
    {
        yield return null;
        sheepToRevive.TurnIntoFlag();
        sheepToRevive.isPlayingCollisionAnimation = false;
    }

    IEnumerator PlayFlagAnimation(Unit sheepToRevive)
    {
        float animationDuration = bumpFrame.flagFrame.Length / animationSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            int frameIndex = (int)(elapsedTime * animationSpeed) % bumpFrame.flagFrame.Length;
            sheepToRevive.spriteRenderer.sprite = bumpFrame.flagFrame[frameIndex];
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // 处理复活后的操作
    IEnumerator HandleRevival()
    {
        float animationDuration = bumpFrame.reLifeSheepFrame.Length / animationSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            int frameIndex = (int)(elapsedTime * animationSpeed) % bumpFrame.reLifeSheepFrame.Length;
            spriteRenderer.sprite = bumpFrame.reLifeSheepFrame[frameIndex];
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 恢复羊的状态
        isFlag = false;
        spriteRenderer.color = Color.white;
        canCtrl = true;
        sheepCollider.enabled = true;
    }

    // 处理羊群之间的碰撞逻辑
    protected void HandleCollisionWithOtherSheep(Unit otherSheep)
    {
        // 将当前羊群和其他羊群的阳光清零
        this.hasSun = 0;
        otherSheep.hasSun = 0;

        // 播放碰撞动画
        PlayCollisionAnimation(otherSheep);

/*        // 随机选择的羊变成旗帜
        Unit flagSheep = Random.value > 0.5f ? this : otherSheep;
        flagSheep.audioSource.Stop(); //关闭移动声音
        flagSheep.audioSource.PlayOneShot(crashSound);

        flagSheep.TurnIntoFlag();*/
    }

    // 将羊群变成旗帜
    protected void TurnIntoFlag()
    {
        audioSource.Stop(); //关闭移动声音
        audioSource.PlayOneShot(crashSound);
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);

        isFlag = true;
        //canCtrl = false; // 不允许继续控制
        //sheepCollider.enabled = false; // 暂时禁用碰撞
        //GetComponent<SpriteRenderer>().sprite = flagSprite; // 改变图片为旗帜

        stopPosition = transform.position; 
        // 停止移动，将位置固定
        StopCoroutine(moveCoroutine);
        transform.position = stopPosition;
    }

    // 当玩家点击旗帜并将其放置在草地时，恢复为羊的状态
    public void PlaceOnGrassTile(Vector3 grassTilePosition)
    {
        transform.position = grassTilePosition; // 将旗帜移动到指定草地
        audioSource.PlayOneShot(reLifeSound);
        StartCoroutine(HandleRevival());
    }

    IEnumerator WaitReLife(float duration)
    {
        //spriteRenderer.sprite = dieSheepSprite;
        audioSource.PlayOneShot(reLifeSound);
        yield return new WaitForSeconds(duration);
        //// 恢复羊的状态
        //isFlag = false;
        //spriteRenderer.color = Color.white;
        //canCtrl = true;
        //spriteRenderer.sprite = sheepSprite;
        //sheepCollider.enabled = true; // 重新启用碰撞
    }

    
    public void SelectSheep()
    {
        if (isSelected) return;
        isSelected = true;
        spriteRenderer.color = selectedColor; 
        if(GameManager.Instance.selectedUnit ==  null)
            GameManager.Instance.selectedUnit = this;
        if (GameManager.Instance.selectedUnit != this)
        {
            GameManager.Instance.selectedUnit.spriteRenderer.color = Color.white;
            GameManager.Instance.selectedUnit.isSelected = false;
            GameManager.Instance.selectedUnit = this;
        }

        audioSource.PlayOneShot(selectSound);
        ShowWalkableRoad();
    }

    protected PosType GetCurInfo()
    {
        if (Vector2.Distance(transform.position, GameManager.Instance.hubTile.transform.position) < 0.5f)
        {
            posIndex = -1;
            return PosType.hub;
        }

        for (int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            if (Vector2.Distance(transform.position, GameManager.Instance.grassTiles[i].transform.position) < 0.5f)
            {
                posIndex = i;
                return PosType.grass;
            }
        }
        // temp store
        for (int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            if (Vector2.Distance(transform.position, GameManager.Instance.storeTiles[i].transform.position) < 0.5f)
            {
                posIndex = i;
                return PosType.store;
            }
        }
        return PosType.errorType;
    }

    protected void ShowWalkableRoad()
    {
        curType = GetCurInfo();

        List<int> indexList = new List<int>();
        if(curType == PosType.hub)
        {
            indexList = GameManager.Instance.hubTile.arrivePos;
            for (int i = 0; i < indexList.Count; ++i)
            {
                GameManager.Instance.storeTiles[indexList[i] - GameManager.Instance.grassTiles.Length].canGo = true;
                GameManager.Instance.storeTiles[indexList[i] - GameManager.Instance.grassTiles.Length].HighlightTile();
            }
        }
        else if(curType == PosType.store)
        {
            indexList = GameManager.Instance.storeTiles[posIndex].arrivePos;
            for (int i = 0; i < indexList.Count; ++i)
            {
                if (indexList[i] >= GameManager.Instance.grassTiles.Length + GameManager.Instance.storeTiles.Length)
                {
                    GameManager.Instance.hubTile.canGo = true;
                    GameManager.Instance.hubTile.HighlightTile();
                }
                else if (indexList[i] >= GameManager.Instance.grassTiles.Length)
                {
                    GameManager.Instance.storeTiles[indexList[i] - GameManager.Instance.grassTiles.Length].canGo = true;
                    GameManager.Instance.storeTiles[indexList[i] - GameManager.Instance.grassTiles.Length].HighlightTile();
                }
                else
                {
                    GameManager.Instance.grassTiles[indexList[i]].canGo = true;
                    GameManager.Instance.grassTiles[indexList[i]].HighlightTile();
                }
            }
        }
        else
        {
            indexList = GameManager.Instance.grassTiles[posIndex].arrivePos;
            for (int i = 0; i < indexList.Count; ++i)
            {
                if (indexList[i] >= GameManager.Instance.grassTiles.Length)
                {
                    GameManager.Instance.storeTiles[indexList[i] - GameManager.Instance.grassTiles.Length].canGo = true;
                    GameManager.Instance.storeTiles[indexList[i] - GameManager.Instance.grassTiles.Length].HighlightTile();
                }
                else
                {
                    GameManager.Instance.grassTiles[indexList[i]].canGo = true;
                    GameManager.Instance.grassTiles[indexList[i]].HighlightTile();
                }
            }
        }
        /*if (curType == PosType.store && Vector2.Distance(transform.position, GameManager.Instance.hubTile.transform.position) <= moveRange)
        {
            GameManager.Instance.hubTile.canGo = true;
            GameManager.Instance.hubTile.HighlightTile();
        }

        //grass
        for (int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            //Debug.Log("grass : " + GameManager.Instance.grassTiles[i].transform.position);
            if ((curType == PosType.grass || curType == PosType.store) && Vector2.Distance(transform.position, GameManager.Instance.grassTiles[i].transform.position) <= moveRange)
            {
                GameManager.Instance.grassTiles[i].canGo = true;
                GameManager.Instance.grassTiles[i].HighlightTile();
            }
        }
        // temp store
        for (int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            //Debug.Log("store : " + GameManager.Instance.storeTiles[i].transform.position);
            if (Vector2.Distance(transform.position, GameManager.Instance.storeTiles[i].transform.position) <= moveRange)
            {
                GameManager.Instance.storeTiles[i].canGo = true;
                GameManager.Instance.storeTiles[i].HighlightTile();
            }
        }*/

    }

    public void Move(Transform _trans)
    {
        ResetTiles();
        canCtrl = false;
        spriteRenderer.color = Color.white;

        // 播放移动音频并循环
        audioSource.clip = moveSound;
        audioSource.loop = true; // 设置为循环播放
        audioSource.volume = moveSoundVolume;
        audioSource.Play(); // 开始播放音频

        moveCoroutine = StartCoroutine(MoveCo(_trans));
    }

    bool ComparePos(Vector2 pos, Vector2 pos2)
    {
        return pos == pos2;
    }

    IEnumerator MoveCo(Transform _trans)
    {
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = _trans.position;
        Vector2 direction = targetPosition - startPosition;

        int type1 = -1, type2 = -1;
        for(int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            if(type1 == -1 && ComparePos(startPosition, GameManager.Instance.grassTiles[i].transform.position))
            {
                type1 = 1;
            }
            if (type2 == -1 && ComparePos(targetPosition, GameManager.Instance.grassTiles[i].transform.position))
            {
                type2 = 1;
            }
        }
        for (int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            if (type1 == -1 && ComparePos(startPosition, GameManager.Instance.storeTiles[i].transform.position))
            {
                type1 = 2;
            }
            if (type2 == -1 && ComparePos(targetPosition, GameManager.Instance.storeTiles[i].transform.position))
            {
                type2 = 2;
            }
        }
        if (type1 == -1 && ComparePos(startPosition, GameManager.Instance.hubTile.transform.position))
        {
            type1 = 3;
        }
        if (type2 == -1 && ComparePos(targetPosition, GameManager.Instance.hubTile.transform.position))
        {
            type2 = 3;
        }
        float k = 1;
        if(type1 == 3 ||  type2 == 3)
        {
            k = kHub;
        }   
        else if(type1 == 2 || type2 == 2)
        {
            k = kStore;
        }

        float v = moveSpeed * k;
        float elapsedTime = 0f;
        float moveDuration = 1f / v;  // 根据moveSpeed (v)调整移动时间

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle > -90 && angle < 90)
            {
                // 向右侧方向移动，确保羊头正确朝向并镜像翻转
                spriteRenderer.flipX = true;
            }
            else
            {
                // 其他方向根据原始计算调整并恢复镜像状态
                spriteRenderer.flipX = false;
                if (angle > 90 || angle < -90)
                {
                    angle += 180;
                }
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);
            }


            yield return null;
        }

        transform.position = targetPosition; // 保证最终位置精准
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
        audioSource.Stop();

        if (transform.position == _trans.position)
        {
            isSelected = false;
            canCtrl = true;
            if(GetCurInfo() == PosType.store)
            {
                GameManager.Instance.storeTiles[posIndex].hasSheep = true;
            }
        }
        //moveSpeed /= k; //复原
    }

    public bool IsMoving()
    {
        return audioSource.clip == moveSound && audioSource.isPlaying;
    }

    protected void UpdateSheepFrame()
    {
        if(isFlag)
        {
            int frameIndex = (int)(totalTime * animationSpeed) % bumpFrame.flagFrame.Length;
            spriteRenderer.sprite = bumpFrame.flagFrame[frameIndex];
            return;
        }

        // 判断羊群是否在移动
        bool isMoving = IsMoving();

        if (isMoving)
        {
            // 如果在移动，播放 movingFrame
            int frameIndex = (int)(totalTime * animationSpeed) % movingFrame.Length;
            spriteRenderer.sprite = movingFrame[frameIndex];
        }
        else
        {
            // 如果不在移动，判断是否在吃草并播放相应动画帧
            curType = GetCurInfo();
            if (eatingFrame.Length > 0 && curType == PosType.grass && preType == PosType.grass && GameManager.Instance.grassTiles[posIndex].isSunny && !isFlag)
            {
                int frameIndex = (int)(totalTime * animationSpeed) % eatingFrame.Length;
                spriteRenderer.sprite = eatingFrame[frameIndex];
            }
            else if(chargingFrame.Length > 0)
            {
                int frameIndex = (int)(totalTime * animationSpeed) % chargingFrame.Length;
                spriteRenderer.sprite = chargingFrame[frameIndex];
            }
        }
    }

    public  void ResetTiles()
    {
        spriteRenderer.color = Color.white;
        GameManager.Instance.hubTile.ResetTile();
        for(int i = 0; i < GameManager.Instance.storeTiles.Length; i++)
        {
            GameManager.Instance.storeTiles[i].ResetTile();
        }
        for (int i = 0; i < GameManager.Instance.grassTiles.Length; i++)
        {
            GameManager.Instance.grassTiles[i].ResetTile();
        }
    }
}
