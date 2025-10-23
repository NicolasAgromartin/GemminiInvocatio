using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;





public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    [Header("Siders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;



    private void OnEnable()
    {
        masterSlider.onValueChanged.AddListener(value => ChangeVolume(value, Dictionaries.SoundOptions[SoundSettings.Master_Volume]));
        musicSlider.onValueChanged.AddListener(value => ChangeVolume(value, Dictionaries.SoundOptions[SoundSettings.Music_Volume]));
        sfxSlider.onValueChanged.AddListener(value => ChangeVolume(value, Dictionaries.SoundOptions[SoundSettings.SFX_Volume]));
    }
    private void OnDisable()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();

        PlayerPrefs.Save();
    }





    public void SetInitialSliders(float master, float volume, float sfx)
    {
        masterSlider.value = master;
        musicSlider.value = volume;
        sfxSlider.value = sfx;
    }
    public void ChangeVolume(float value, string setting)
    {
        mixer.SetFloat(setting, ToDecibel(value));
        PlayerPrefs.SetFloat(setting, value);
    }
    public static float ToDecibel(float value) => Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
}
