using UnityEngine;

public class AnimationControllerDayToNight : MonoBehaviour
{
    private Animation animation;

    void Start()
    {
        animation = GetComponent<Animation>();
    }

    // TO play Animation
    public void PlayAnimation()
    {
        animation.Play("YourAnimationClipName");
    }

    // To stop Animation
    public void StopAnimation()
    {
        animation.Stop();
    }

    // To check if Animation's playing or not
    public bool IsAnimationPlaying()
    {
        return animation.isPlaying;
    }
}