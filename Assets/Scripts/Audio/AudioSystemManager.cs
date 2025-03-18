using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystemManager : MonoBehaviour
{
    public static AudioSystemManager Instance { get; private set; }

    #region Variables
    [SerializeField]
    private BGMPlayer _bgmPlayer;
    public BGMPlayer BGMPlayer => _bgmPlayer;

    [SerializeField]
    private SfxController _SfxController;
    public SfxController SfxController => _SfxController;

    [SerializeField]
    private AudioSetting _audioSetting;
    public AudioSetting AudioSetting => _audioSetting;

    public AudioDatabaseBGM AudioDatabaseBGM { get; private set; }
    public AudioDatabaseUI AudioDatabaseUI { get; private set; }
    public AudioDatabaseSfx AudioDatabaseGameplay { get; private set; }

    public bool HasInitialized { get; private set; }
    #endregion
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        AudioDatabaseBGM = GetComponentInChildren<AudioDatabaseBGM>();
        //AudioDatabaseUI = GetComponentInChildren<AudioDatabaseUI>();
        AudioDatabaseGameplay = GetComponentInChildren<AudioDatabaseSfx>();

        AudioDatabaseBGM.InitializeDatabase();
        //AudioDatabaseUI.InitializeDatabase();
        AudioDatabaseGameplay.InitializeDatabase();

        _bgmPlayer.Initialize(this);
        _SfxController.Initialize(this);
        _audioSetting.Initialize();

        HasInitialized = true;
    }

    public void TestingSfx(AudioDatabaseType audioDatabaseType, string clipName, float volume = 1)
    {
        PlaySfx(audioDatabaseType, clipName, volume);
    }

    public void PlaySfx(AudioDatabaseType audioDatabaseType, string clipName, float volume = 1)
    {
        if (TryGetAudioClipFromDatabase(audioDatabaseType, clipName, out AudioClip clip))
        {
            _SfxController.PlayAudio(clip, volume);
        }
        else
        {
            Debug.LogError($"Audio {clipName} not found in {audioDatabaseType}");
        }
    }

    public void PlaySfx(AudioClip clip, float volume = 1)
    {
        _SfxController.PlayAudio(clip, volume);
    }

    public void PlaySfx3D(AudioDatabaseType audioDatabaseType, string clipName, float volume = 1, Transform audioSourceTransform = null)
    {
        if (TryGetAudioClipFromDatabase(audioDatabaseType, clipName, out AudioClip clip))
        {
            _SfxController.PlayAudio(clip, volume, audioSourceTransform);
        }
        else
        {
            Debug.LogError($"Audio {clipName} not found in {audioDatabaseType}");
        }
    }

    public void PlaySfx3D(AudioClip clip, float volume = 1, Transform audioSourceTransform = null)
    {
        _SfxController.PlayAudio(clip, volume, audioSourceTransform);
    }

    public void PlayBGM(AudioDatabaseType audioDatabaseType, string clipName, float volume, bool loop)
    {
        float finalVolume = volume * _audioSetting.BGMVolume.Value;
        if (TryGetAudioClipFromDatabase(audioDatabaseType, clipName, out AudioClip clip))
        {
            _bgmPlayer.PlayBackgroundMusic(clip, finalVolume, loop);
        }
        else
        {
            Debug.LogError($"Audio {clipName} not found in {audioDatabaseType}");
        }

    }

    public void StopBGM()
    {
        _bgmPlayer.StopBackgroundMusic();
    }

    public void PlayBGM(AudioClip clip, float volume, bool loop)
    {
        float finalVolume = volume * _audioSetting.BGMVolume.Value;
        _bgmPlayer.PlayBackgroundMusic(clip, finalVolume, loop);
    }

    private bool TryGetAudioClipFromDatabase(AudioDatabaseType audioDatabaseType, string clipName, out AudioClip clip)
    {
        clip = null;

        switch (audioDatabaseType)
        {
            case AudioDatabaseType.BGM:
                clip = AudioDatabaseBGM.GetAudioClip(clipName);
                break;
            case AudioDatabaseType.Gameplay:
                clip = AudioDatabaseGameplay.GetAudioClip(clipName);
                break;/* 
            case AudioDatabaseType.UI:
                clip = AudioDatabaseUI.GetAudioClip(clipName);
                break; */
        }

        return clip != null;
    }

    public void FadeInBGM(float targetVolume, float duration)
    {
        StartCoroutine(_bgmPlayer.FadeAudio(targetVolume, duration));
    }

    public void FadeOutBGM(float duration)
    {
        StartCoroutine(_bgmPlayer.FadeAudio(0f, duration));
    }
}
