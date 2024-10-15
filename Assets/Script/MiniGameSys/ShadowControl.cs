using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class ShadowControl : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer shadowImg;

    private float targetRotation = 90f; // 目标旋转角度
    private float duration = 30f;       // 半分钟（30秒）旋转
    private float rotationSpeed;        // 每秒旋转的角度


    void Start()
    {
        shadowImg.transform.position = GameManager.Instance.hubTile.transform.position;

        // 计算每秒旋转多少度
        rotationSpeed = targetRotation / duration;
    }

    void Update()
    {
        // 平滑旋转阴影板，每帧旋转一定角度
        shadowImg.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

}
