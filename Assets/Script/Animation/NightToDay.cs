using UnityEngine;

public class AnimationControllerNightToDay : MonoBehaviour
{
    private Animator animator;
    public GameObject timePassCanvas;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.length > 0 && stateInfo.normalizedTime >= 1f)
        {
            // ∂Øª≠≤•∑≈ÕÍ±œ
            Debug.Log("Animation completed!");
            if (timePassCanvas != null)
            {
                PlayerPrefs.SetString("animatN2D", "yes");
                PlayerPrefs.Save();
                timePassCanvas.SetActive(false);
            }
        }
    }
}