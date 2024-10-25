using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
public class AnimationPlayerScript : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        animator.SetBool("isButtonClick", true);
    }

    
}