using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    // 音效Toggle
    public Toggle soundToggle;
    // 音效音量滑条
    public Slider soundVolumeSlider;
    // 音乐Toggle
    public Toggle musicToggle;
    // 音乐音量滑条
    public Slider musicVolumeSlider;

    private void Start()
    {
        // 初始化音效Toggle和滑条状态
        float soundVolume;
        AudioManager.Instance.soundMixer.GetFloat("SoundVolume", out soundVolume);
        soundVolumeSlider.value = Mathf.Pow(10, soundVolume / 20);
        soundToggle.isOn = soundVolume > -80;

        // 为音效Toggle添加事件监听
        soundToggle.onValueChanged.AddListener(OnSoundToggleValueChanged);
        // 为音效音量滑条添加事件监听
        soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeSliderValueChanged);

        // 初始化音乐Toggle和滑条状态
        float musicVolume;
        AudioManager.Instance.musicMixer.GetFloat("MusicVolume", out musicVolume);
        musicVolumeSlider.value = Mathf.Pow(10, musicVolume / 20);
        musicToggle.isOn = musicVolume > -80;

        // 为音乐Toggle添加事件监听
        musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
        // 为音乐音量滑条添加事件监听
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
    }

    // 音效Toggle值改变事件处理方法
    private void OnSoundToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.UnmuteSound();
        }
        else
        {
            AudioManager.Instance.MuteSound();
        }
    }

    // 音效音量滑条值改变事件处理方法
    private void OnSoundVolumeSliderValueChanged(float volume)
    {
        AudioManager.Instance.SetSoundVolume(volume);
    }

    // 音乐Toggle值改变事件处理方法
    private void OnMusicToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.UnmuteMusic();
        }
        else
        {
            AudioManager.Instance.MuteMusic();
        }
    }

    // 音乐音量滑条值改变事件处理方法
    private void OnMusicVolumeSliderValueChanged(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }
}