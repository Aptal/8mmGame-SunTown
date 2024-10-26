using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // 音效混合器
    public AudioMixer soundMixer;
    // 音乐混合器
    public AudioMixer musicMixer;

    // 单例模式
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 设置音效音量
    public void SetSoundVolume(float volume)
    {
        soundMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }

    // 设置音乐音量
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    // 静音音效
    public void MuteSound()
    {
        soundMixer.SetFloat("SoundVolume", -80);
    }

    // 取消音效静音
    public void UnmuteSound()
    {
        soundMixer.SetFloat("SoundVolume", 0);
    }

    // 静音音乐
    public void MuteMusic()
    {
        musicMixer.SetFloat("MusicVolume", -80);
    }

    // 取消音乐静音
    public void UnmuteMusic()
    {
        musicMixer.SetFloat("MusicVolume", 0);
    }
}