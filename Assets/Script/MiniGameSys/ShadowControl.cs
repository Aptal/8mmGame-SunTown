using System.Collections;
using UnityEngine;
using TMPro;

public class ShadowControl : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shadowImg;
    [SerializeField]
    [Header("��ת�ٶȣ�����/�룩")]
    private float defaultRotationSpeed = -3f; // Ĭ���ٶȣ���ʱ�룬3��/��

    [SerializeField]
    [Header("��ǰ��ת�ٶȣ�����/�룩")]
    private float rotationSpeed; // ��ǰ��ת�ٶ�
    [SerializeField]
    [Header("���ٶ�����")]
    private AnimationCurve accelerationCurve; // ���ٶ�����

    [Header("���Ч����ʼʱ�䣨�룩")]
    [SerializeField] private float randomEffectStart1 = 130f; // 2:10
    [SerializeField] private float randomEffectStart2 = 70f;  // 1:10

    [Header("���Ч���ָ�ʱ��")]
    [SerializeField] private float effectRecoverStart1 = 100f; // 1:40
    [SerializeField] private float effectRecoverStart2 = 40f;  // 0:40

    private bool isEffectActive = false;
    private bool isEffectRecovering = false;
    private string currentEffect = "";
    private int preEffectIndex = -1;

    // UIԪ��
    [SerializeField] private GameObject effectIcon; // ��ʾͼ���UIԪ��
    private string overName = "�쳣����";

    // music
    protected AudioSource audioSource; // ���ڲ�����Ƶ
    [SerializeField] protected AudioClip vFastSound;
    [SerializeField] protected AudioClip vLowSound;
    [SerializeField] protected AudioClip vStopSound;
    [SerializeField] protected AudioClip vContrastSound;

    void Start()
    {
        shadowImg.transform.position = GameManager.Instance.hubTile.transform.position;
        rotationSpeed = defaultRotationSpeed; // ��ʼ�ٶ�

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    void Update()
    {
        // ����ʱ����
        int remainingTime = GameManager.Instance.timer1.Duration;

        if (remainingTime <= 0f)
        {
            return; // ��Ϸ����
        }

        // �������Ч����2:10��1:10������10���ڹ��ɵ�����¼���
        if (!isEffectActive && (remainingTime == randomEffectStart1 || remainingTime == randomEffectStart2))
        {
            ActivateRandomEffect();
        }

        // �ָ���Ĭ��״̬��1:40��0:40������10���ڹ��ɻ�Ĭ��״̬��
        if ( !isEffectRecovering && (remainingTime == effectRecoverStart1 || remainingTime == effectRecoverStart2))
        {
            ShowEffectIcon(overName);
            StartCoroutine(RecoverEffect());
        }

        // ƽ����ת�����
        shadowImg.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    // �������Ч��
    private void ActivateRandomEffect()
    {
        isEffectActive = true;
        int effectIndex = Random.Range(0, 4); // ���ѡ����ת��2���١�0.5���١�ֹͣ
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
                StartCoroutine(GraduallyReachEffect(-rotationSpeed, 10f)); // ��ת��10���ڴﵽ��תЧ��
                currentEffect = "��ת";
                audioSource.PlayOneShot(vContrastSound);
                break;
            case 1:
                StartCoroutine(GraduallyReachEffect(rotationSpeed * 2f, 10f)); // 2���٣�10���ڴﵽ2����
                currentEffect = "2����";
                audioSource.PlayOneShot(vFastSound);
                break;
            case 2:
                StartCoroutine(GraduallyReachEffect(rotationSpeed * 0.5f, 10f)); // 0.5���٣�10���ڴﵽ0.5����
                currentEffect = "0.5����";
                audioSource.PlayOneShot(vLowSound);
                break;
            case 3:
                StartCoroutine(GraduallyReachEffect(0f, 10f)); // ֹͣ��10���ڼ��ٵ�0
                currentEffect = "ֹͣ";
                audioSource.PlayOneShot(vStopSound);
                break;
        }
        preEffectIndex = effectIndex;
        ShowEffectIcon(currentEffect);
        //StartCoroutine(EffectDuration(20f)); // ���Ч������20��
    }

    // �������Ч��20��
    private IEnumerator EffectDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isEffectActive = false; // ���Ч��������׼���ָ�
    }

    // �𽥴ﵽЧ��
    private IEnumerator GraduallyReachEffect(float targetSpeed, float duration)
    {
        float startSpeed = rotationSpeed;

        // ���� duration ʱ�����ƽ������
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            // ʹ�ü��ٶ�����ƽ�������ٶ�
            rotationSpeed = Mathf.Lerp(startSpeed, targetSpeed, accelerationCurve.Evaluate(progress));
            yield return null;
        }

        // ȷ�������ٶȾ�ȷ�趨ΪĿ���ٶ�
        rotationSpeed = targetSpeed;
    }

    // �ָ���Ĭ��״̬����ʱ����ת���ٶ�3��/�룩
    private IEnumerator RecoverEffect()
    {
        isEffectRecovering = true;
        yield return StartCoroutine(GraduallyReachEffect(defaultRotationSpeed, 10f)); // ��10���ڻָ�����ʼ�ٶ� -3��/��
        rotationSpeed = defaultRotationSpeed;
        isEffectRecovering = false;
        currentEffect = "";
    }

    // ��ʾЧ����ʾͼ��
    private void ShowEffectIcon(string text)
    {
        /*// ��ʾͼ��
        effectIcon.SetActive(true);
        effectIcon.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // 5�������ͼ��
        StartCoroutine(HideEffectIcon());*/
        //Debug.Log(effectIcon.transform.position);
        
        //effectIcon.transform.position = Camera.main.ScreenToWorldPoint(offScreenPosition);
        effectIcon.SetActive(true);
        effectIcon.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // �������붯��
        StartCoroutine(SlideInEffect());
    }

    private IEnumerator SlideInEffect()
    {
        Vector3 offScreenPosition = new Vector3(0, 5, 0);
        Vector3 targetPosition = effectIcon.transform.position + offScreenPosition;

        //Debug.Log(targetPosition);
        //Debug.Log(effectIcon.transform.position);
        float duration = 8f; // �������ʱ��
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            effectIcon.transform.position = Vector3.Lerp(effectIcon.transform.position, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectIcon.transform.position = targetPosition; // ȷ������λ��׼ȷ

        // 5�������ͼ��
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
        float duration = 8f; // ��������ʱ��
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            effectIcon.transform.position = Vector3.Lerp(effectIcon.transform.position, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        effectIcon.SetActive(false); // ����ͼ��
        isEffectActive = false;
    }
}
