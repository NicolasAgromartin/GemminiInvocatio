using System.Collections;
using UnityEngine;
using UnityEngine.Audio;




public class AudioManager : Singleton<AudioManager>
{
    private VolumeSettings volumeSettings;
    [SerializeField] private AudioMixer mixer;

    [Header("Audio Snapshots")]
    [SerializeField] private AudioMixerSnapshot sceneLoadingSnapshot;

    [Header("Music Snapshots")]
    [SerializeField] private AudioMixerSnapshot necromancyBGM_snapshot;
    [SerializeField] private AudioMixerSnapshot exteriorBGM_snapshot;
    [SerializeField] private AudioMixerSnapshot interiorBGM_snapshot;



    #region Life Cycle
    new private void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneLoader.OnSceneStartLoading += HandleSceneLoadStart;
        SceneLoader.OnSceneLoading += HandleSceneLoading;
        SceneLoader.OnSceneLoaded += HandleLoadedScene;
    }
    private void OnDisable()
    {
        SceneLoader.OnSceneStartLoading -= HandleSceneLoadStart;
        SceneLoader.OnSceneLoading -= HandleSceneLoading;
        SceneLoader.OnSceneLoaded -= HandleLoadedScene;
    }
    private void Start()
    {
        InitSceneVolume();
    }
    #endregion


    // audio mixer 


    private void HandleSceneLoadStart()
    {
    }
    private void HandleSceneLoading()
    {
    }
    private void HandleLoadedScene()
    {
        InitSceneVolume();
    }
    private void InitSceneVolume()
    {
        volumeSettings = FindAnyObjectByType<VolumeSettings>(FindObjectsInactive.Include);

        if (volumeSettings == null) return;

        float master = PlayerPrefs.GetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], 1f);
        float volume = PlayerPrefs.GetFloat(Dictionaries.SoundOptions[SoundSettings.Music_Volume], .5f);
        float sfx = PlayerPrefs.GetFloat(Dictionaries.SoundOptions[SoundSettings.SFX_Volume], .8f);

        volumeSettings.SetInitialSliders(master, volume, sfx);

        volumeSettings.ChangeVolume(master, Dictionaries.SoundOptions[SoundSettings.Master_Volume]);
        volumeSettings.ChangeVolume(volume, Dictionaries.SoundOptions[SoundSettings.Music_Volume]);
        volumeSettings.ChangeVolume(sfx, Dictionaries.SoundOptions[SoundSettings.SFX_Volume]);
    }




    #region Fade
    private readonly float fadeSpeed = 1f;
    private IEnumerator FadeIn()
    {
        float currentVolume = PlayerPrefs.GetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], 1f);

        while (currentVolume < 0)
        {
            currentVolume += Time.deltaTime * fadeSpeed; // Misma velocidad que tu FadeOut
            
            mixer.SetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], VolumeSettings.ToDecibel(currentVolume));
            yield return null;
        }

        mixer.SetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], VolumeSettings.ToDecibel(currentVolume));
    }
    private IEnumerator FadeOut()
    {
        Debug.Log("fadingout");

        float currentVolume = PlayerPrefs.GetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], 1f);

        while (currentVolume > 0)
        {
            currentVolume -= Time.deltaTime * fadeSpeed;
            mixer.SetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], VolumeSettings.ToDecibel(currentVolume));
            yield return null;
        }

        currentVolume = 0f;
        mixer.SetFloat(Dictionaries.SoundOptions[SoundSettings.Master_Volume], VolumeSettings.ToDecibel(currentVolume));
    }
    #endregion
}
