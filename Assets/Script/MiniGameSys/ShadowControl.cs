using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class ShadowControl : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer shadowImg;

    private float targetRotation = 90f; // Ŀ����ת�Ƕ�
    private float duration = 30f;       // ����ӣ�30�룩��ת
    private float rotationSpeed;        // ÿ����ת�ĽǶ�


    void Start()
    {
        shadowImg.transform.position = GameManager.Instance.hubTile.transform.position;

        // ����ÿ����ת���ٶ�
        rotationSpeed = targetRotation / duration;
    }

    void Update()
    {
        // ƽ����ת��Ӱ�壬ÿ֡��תһ���Ƕ�
        shadowImg.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

}
