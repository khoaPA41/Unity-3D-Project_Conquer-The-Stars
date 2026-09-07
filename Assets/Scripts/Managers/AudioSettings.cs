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
    private Slider masterVolumeSlider;

    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;



    public float _masterVolume { get; set; } = .5f;
    public float _bgmVolume { get; set; } = .5f;
    public float _sfxVolume { get; set; } = .5f;
    public float _uiVolume { get; set; } = .5f;



    private void Start()
    {
        UpdateSoundSettingsUI();
        SetMasterVolume(_masterVolume);
        SetBmgVolume(_bgmVolume);
        SetSfxVolume(_sfxVolume);
        SetUiVolume(_uiVolume);
    }


    private void UpdateSoundSettingsUI()
    {
        masterVolumeSlider.value = _masterVolume;
        bgmVolumeSlider.value = _bgmVolume;
        sfxVolumeSlider.value = _sfxVolume;
        uiVolumeSlider.value = _uiVolume;
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20f);
        _masterVolume = masterVolumeSlider.value;
    }

    public void SetBmgVolume(float volume)
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
        _bgmVolume = bgmVolumeSlider.value;
    }

    public void SetSfxVolume(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
        _sfxVolume = sfxVolumeSlider.value;
    }

    public void SetUiVolume(float volume)
    {
        mixer.SetFloat("UI", Mathf.Log10(volume) * 20f);
        _uiVolume = uiVolumeSlider.value;
    }
}
