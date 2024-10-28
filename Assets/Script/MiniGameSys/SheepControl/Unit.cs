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
    [Header("kStore(���ڲݵغͲֿ���ϵ����speed*k)")]

    protected float kStore = 0.8f;
    [Header("kHub(��������Ͳֿ���ϵ����speed*k)")]

    protected float kHub = 0.6f;
    protected Coroutine moveCoroutine;

    public int productV = 1;

    public int sunLimit = 10;

    [SerializeField]
    public int hasSun = 0;
    public TextMeshProUGUI sheepSunCnt;//��ʾ��ǰ��ȺЯ����������

    public int sheepCnt = 6;

    public int posIndex = -2;
    public PosType preType = PosType.errorType;
    public PosType curType = PosType.errorType;
    protected float totalTime = 0;

    public SpriteRenderer spriteRenderer { get; protected set; }

    [SerializeField] protected Sprite sheepSprite;  // ԭ���ͼƬ

    public float animationSpeed = 12f;
    [SerializeField] public Sprite[] chargingFrame;
    [SerializeField] public Sprite[] eatingFrame;
    [SerializeField] public Sprite[] movingFrame;

    [SerializeField] SheepBumpCtrl bumpFrame;

    [SerializeField] protected Sprite dieSheepSprite;   // �������ͼƬ
    //[SerializeField] protected Sprite[] reLifeSheepFrame;

    [SerializeField] protected Sprite flagSprite;   // ���ĵ�ͼƬ
    //[SerializeField] protected Sprite[] flagFrame; 

    public bool isFlag = false;  // �Ƿ�������״̬
    public bool isBeingDragged = false; // �Ƿ����ڱ��϶�

    protected Vector3 stopPosition; // ��¼��ײʱ��λ��
    protected Collider2D sheepCollider;

    public bool canCtrl = true;
    public bool isSelected = false;
    public Color selectedColor = Color.yellow;

    // ��������
    [SerializeField] protected AudioClip selectSound; // ѡ��sheep
    [SerializeField] protected AudioClip moveSound; // �ƶ�sheep
    [Range(0.0f, 1.0f)] // �ڱ༭������ӻ�������������
    private float moveSoundVolume = 0.5f; // �ƶ�������Ĭ������

    [SerializeField] protected AudioClip crashSound;
    [SerializeField] protected AudioClip reLifeSound;


    protected AudioSource audioSource; // ���ڲ�����Ƶ

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

        // ȷ��������ײ��������Ⱥ���ҵ�ǰ��Ⱥû�б������
        if (otherSheep != null && !isFlag && !otherSheep.isFlag && !isPlayingCollisionAnimation && !otherSheep.isPlayingCollisionAnimation)
        {
            HandleCollisionWithOtherSheep(otherSheep);
        }
    }

    // �Ƿ����ڲ�����ײ����
    public bool isPlayingCollisionAnimation = false;

    // ������ײ����
    public void PlayCollisionAnimation(Unit otherSheep)
    {
        //canCtrl = false;
        //otherSheep.canCtrl = false;

        // ������ײ���������ȷ��Ҫ���ŵ���ײ����֡
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

    // �������Ĳ���
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

        // �ָ����״̬
        isFlag = false;
        spriteRenderer.color = Color.white;
        canCtrl = true;
        sheepCollider.enabled = true;
    }

    // ������Ⱥ֮�����ײ�߼�
    protected void HandleCollisionWithOtherSheep(Unit otherSheep)
    {
        // ����ǰ��Ⱥ��������Ⱥ����������
        this.hasSun = 0;
        otherSheep.hasSun = 0;

        // ������ײ����
        PlayCollisionAnimation(otherSheep);

/*        // ���ѡ�����������
        Unit flagSheep = Random.value > 0.5f ? this : otherSheep;
        flagSheep.audioSource.Stop(); //�ر��ƶ�����
        flagSheep.audioSource.PlayOneShot(crashSound);

        flagSheep.TurnIntoFlag();*/
    }

    // ����Ⱥ�������
    protected void TurnIntoFlag()
    {
        audioSource.Stop(); //�ر��ƶ�����
        audioSource.PlayOneShot(crashSound);
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);

        isFlag = true;
        //canCtrl = false; // �������������
        //sheepCollider.enabled = false; // ��ʱ������ײ
        //GetComponent<SpriteRenderer>().sprite = flagSprite; // �ı�ͼƬΪ����

        stopPosition = transform.position; 
        // ֹͣ�ƶ�����λ�ù̶�
        StopCoroutine(moveCoroutine);
        transform.position = stopPosition;
    }

    // ����ҵ�����Ĳ���������ڲݵ�ʱ���ָ�Ϊ���״̬
    public void PlaceOnGrassTile(Vector3 grassTilePosition)
    {
        transform.position = grassTilePosition; // �������ƶ���ָ���ݵ�
        audioSource.PlayOneShot(reLifeSound);
        StartCoroutine(HandleRevival());
    }

    IEnumerator WaitReLife(float duration)
    {
        //spriteRenderer.sprite = dieSheepSprite;
        audioSource.PlayOneShot(reLifeSound);
        yield return new WaitForSeconds(duration);
        //// �ָ����״̬
        //isFlag = false;
        //spriteRenderer.color = Color.white;
        //canCtrl = true;
        //spriteRenderer.sprite = sheepSprite;
        //sheepCollider.enabled = true; // ����������ײ
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

        // �����ƶ���Ƶ��ѭ��
        audioSource.clip = moveSound;
        audioSource.loop = true; // ����Ϊѭ������
        audioSource.volume = moveSoundVolume;
        audioSource.Play(); // ��ʼ������Ƶ

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
        float moveDuration = 1f / v;  // ����moveSpeed (v)�����ƶ�ʱ��

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle > -90 && angle < 90)
            {
                // ���Ҳ෽���ƶ���ȷ����ͷ��ȷ���򲢾���ת
                spriteRenderer.flipX = true;
            }
            else
            {
                // �����������ԭʼ����������ָ�����״̬
                spriteRenderer.flipX = false;
                if (angle > 90 || angle < -90)
                {
                    angle += 180;
                }
                spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);
            }


            yield return null;
        }

        transform.position = targetPosition; // ��֤����λ�þ�׼
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
        //moveSpeed /= k; //��ԭ
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

        // �ж���Ⱥ�Ƿ����ƶ�
        bool isMoving = IsMoving();

        if (isMoving)
        {
            // ������ƶ������� movingFrame
            int frameIndex = (int)(totalTime * animationSpeed) % movingFrame.Length;
            spriteRenderer.sprite = movingFrame[frameIndex];
        }
        else
        {
            // ��������ƶ����ж��Ƿ��ڳԲݲ�������Ӧ����֡
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
