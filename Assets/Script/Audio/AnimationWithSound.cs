using UnityEngine;

public class AnimateWithSound : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (animator == null)
        {
            Debug.LogError("无法获取Animator组件");
        }
        if (audioSource == null)
        {
            Debug.LogError("无法获取AudioSource组件");
        }
    }

    void PlayAnimationAndSound()
    {
        animator.Play("05光能中枢启动");
        audioSource.Play();
    }
}