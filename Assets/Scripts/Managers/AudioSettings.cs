using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField]
    private AudioMixer mixer;

    [Header("Sound Settings Slider")]
    [SerializeField]
    private Slider _masterVolumeSlider;

    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _uiVolumeSlider;



    public float MasterVolume { get; set; } = .5f;
    public float BgmVolume { get; set; } = .5f;
    public float SfxVolume { get; set; } = .5f;
    public float UiVolume { get; set; } = .5f;



    private void Start()
    {
        UpdateSoundSettingsUI();
        SetMasterVolume(MasterVolume);
        SetBmgVolume(BgmVolume);
        SetSfxVolume(SfxVolume);
        SetUiVolume(UiVolume);
    }


    private void UpdateSoundSettingsUI()
    {
        _masterVolumeSlider.value = MasterVolume;
        _bgmVolumeSlider.value = BgmVolume;
        _sfxVolumeSlider.value = SfxVolume;
        _uiVolumeSlider.value = UiVolume;
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20f);
        MasterVolume = _masterVolumeSlider.value;
    }

    public void SetBmgVolume(float volume)
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
        BgmVolume = _bgmVolumeSlider.value;
    }

    public void SetSfxVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
        SfxVolume = _sfxVolumeSlider.value;
    }

    public void SetUiVolume(float volume)
    {
        mixer.SetFloat("UI", Mathf.Log10(volume) * 20f);
        UiVolume = _uiVolumeSlider.value;
    }
}
