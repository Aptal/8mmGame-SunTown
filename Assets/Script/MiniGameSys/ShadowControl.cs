using System.Collections;
using UnityEngine;
using TMPro;

public class ShadowControl : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shadowImg;
    [SerializeField]
    [Header("旋转速度（度数/秒）")]
    private float defaultRotationSpeed = -3f; // 默认速度：逆时针，3度/秒

    [SerializeField]
    [Header("当前旋转速度（度数/秒）")]
    private float rotationSpeed; // 当前旋转速度
    [SerializeField]
    [Header("加速度曲线")]
    private AnimationCurve accelerationCurve; // 加速度曲线

    [Header("随机效果开始时间（秒）")]
    [SerializeField] private float randomEffectStart1 = 130f; // 2:10
    [SerializeField] private float randomEffectStart2 = 70f;  // 1:10

    [Header("随机效果恢复时间")]
    [SerializeField] private float effectRecoverStart1 = 100f; // 1:40
    [SerializeField] private float effectRecoverStart2 = 40f;  // 0:40

    private bool isEffectActive = false;
    private bool isEffectRecovering = false;
    private string currentEffect = "";
    private int preEffectIndex = -1;

    // UI元素
    [SerializeField] private GameObject effectIcon; // 提示图标的UI元素
    private string overName = "异常结束";

    // music
    protected AudioSource audioSource; // 用于播放音频
    [SerializeField] protected AudioClip vFastSound;
    [SerializeField] protected AudioClip vLowSound;
    [SerializeField] protected AudioClip vStopSound;
    [SerializeField] protected AudioClip vContrastSound;

    void Start()
    {
        shadowImg.transform.position = GameManager.Instance.hubTile.transform.position;
        rotationSpeed = defaultRotationSpeed; // 初始速度

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    void Update()
    {
        // 倒计时管理
        int remainingTime = GameManager.Instance.timer1.Duration;

        if (remainingTime <= 0f)
        {
            return; // 游戏结束
        }

        // 启动随机效果（2:10和1:10启动，10秒内过渡到随机事件）
        if (!isEffectActive && (remainingTime == randomEffectStart1 || remainingTime == randomEffectStart2))
        {
            ActivateRandomEffect();
        }

        // 恢复至默认状态（1:40和0:40启动，10秒内过渡回默认状态）
        if ( !isEffectRecovering && (remainingTime == effectRecoverStart1 || remainingTime == effectRecoverStart2))
        {
            ShowEffectIcon(overName);
            StartCoroutine(RecoverEffect());
        }

        // 平滑旋转光伏板
        shadowImg.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    // 启动随机效果
    private void ActivateRandomEffect()
    {
        isEffectActive = true;
        int effectIndex = Random.Range(0, 4); // 随机选择逆转、2倍速、0.5倍速、停止
        Debug.Log("debug1: " + effectIndex + ", " + preEffectIndex);
        if(effectIndex == preEffectIndex)
        {
            effectIndex = Random.Range(0, 3);
            Debug.Log("debug2: " + effectIndex);
            if (effectIndex == preEffectIndex)
            {
                effectIndex = 3;
            }
        }
        switch (effectIndex)
        {
            case 0:
                StartCoroutine(GraduallyReachEffect(-rotationSpeed, 10f)); // 逆转：10秒内达到逆转效果
                currentEffect = "逆转";
                audioSource.PlayOneShot(vContrastSound);
                break;
            case 1:
                StartCoroutine(GraduallyReachEffect(rotationSpeed * 2f, 10f)); // 2倍速：10秒内达到2倍速
                currentEffect = "2倍速";
                audioSource.PlayOneShot(vFastSound);
                break;
            case 2:
                StartCoroutine(GraduallyReachEffect(rotationSpeed * 0.5f, 10f)); // 0.5倍速：10秒内达到0.5倍速
                currentEffect = "0.5倍速";
                audioSource.PlayOneShot(vLowSound);
                break;
            case 3:
                StartCoroutine(GraduallyReachEffect(0f, 10f)); // 停止：10秒内减速到0
                currentEffect = "停止";
                audioSource.PlayOneShot(vStopSound);
                break;
        }
        preEffectIndex = effectIndex;
        ShowEffectIcon(currentEffect);
        //StartCoroutine(EffectDuration(20f)); // 随机效果持续20秒
    }

    // 持续随机效果20秒
    private IEnumerator EffectDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isEffectActive = false; // 随机效果结束，准备恢复
    }

    // 逐渐达到效果
    private IEnumerator GraduallyReachEffect(float targetSpeed, float duration)
    {
        float startSpeed = rotationSpeed;

        // 按照 duration 时间进行平滑过渡
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            // 使用加速度曲线平滑调整速度
            rotationSpeed = Mathf.Lerp(startSpeed, targetSpeed, accelerationCurve.Evaluate(progress));
            yield return null;
        }

        // 确保最终速度精确设定为目标速度
        rotationSpeed = targetSpeed;
    }

    // 恢复至默认状态（逆时针旋转，速度3度/秒）
    private IEnumerator RecoverEffect()
    {
        isEffectRecovering = true;
        yield return StartCoroutine(GraduallyReachEffect(defaultRotationSpeed, 10f)); // 在10秒内恢复到初始速度 -3度/秒
        rotationSpeed = defaultRotationSpeed;
        isEffectRecovering = false;
        currentEffect = "";
    }

    // 显示效果提示图标
    private void ShowEffectIcon(string text)
    {
        /*// 显示图标
        effectIcon.SetActive(true);
        effectIcon.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // 5秒后隐藏图标
        StartCoroutine(HideEffectIcon());*/
        //Debug.Log(effectIcon.transform.position);
        
        //effectIcon.transform.position = Camera.main.ScreenToWorldPoint(offScreenPosition);
        effectIcon.SetActive(true);
        effectIcon.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // 启动滑入动画
        StartCoroutine(SlideInEffect());
    }

    private IEnumerator SlideInEffect()
    {
        Vector3 offScreenPosition = new Vector3(0, 5, 0);
        Vector3 targetPosition = effectIcon.transform.position + offScreenPosition;

        //Debug.Log(targetPosition);
        //Debug.Log(effectIcon.transform.position);
        float duration = 8f; // 滑入持续时间
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            effectIcon.transform.position = Vector3.Lerp(effectIcon.transform.position, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectIcon.transform.position = targetPosition; // 确保最终位置准确

        // 5秒后隐藏图标
        //yield return new WaitForSeconds(3f);
        StartCoroutine(HideEffectIcon());
    }

    private IEnumerator HideEffectIcon()
    {
        /*yield return new WaitForSeconds(5f);
        effectIcon.SetActive(false);*/
        yield return new WaitForSeconds(1f);

        Vector3 offScreenPosition = new Vector3(0, 5, 0);
        Vector3 targetPosition = effectIcon.transform.position - offScreenPosition;
        float duration = 8f; // 滑出持续时间
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            effectIcon.transform.position = Vector3.Lerp(effectIcon.transform.position, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        effectIcon.SetActive(false); // 隐藏图标
        isEffectActive = false;
    }
}
