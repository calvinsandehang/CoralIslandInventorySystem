using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the playback of background music within the game.
/// This class implements the Singleton pattern to ensure that there's only one instance of it.
/// </summary>
public class BGMPlayer : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// AudioSource component to play background music.
    /// </summary>
    private AudioSource audioSource;
    private AudioSystemManager audioSystemManager;
    private float currentBGMVolume;
    #endregion

    private void Start()
    {
        AudioSystemManager.Instance.PlayBGM(AudioDatabaseType.BGM, "BGM0", 1f, true);
    }
    #region Initialization

    public void Initialize(AudioSystemManager audioManager)
    {
        audioSystemManager = audioManager;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    #endregion


    /// <summary>
    /// Plays the specified music clip as background music.
    /// </summary>
    /// <param name="musicClip">The AudioClip to play.</param>
    /// <param name="loop">Whether the music should loop.</param>
    public void PlayBackgroundMusic(AudioClip musicClip, float volume, bool loop = true)
    {
        if (musicClip == null)
        {
            Debug.LogError("Music clip is null");
            return;
        }

        bool isBgmMuted = audioSystemManager.AudioSetting.MuteBGM.Value;
        float bgmVolume = audioSystemManager.AudioSetting.BGMVolume.Value;

        if (isBgmMuted)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = volume * bgmVolume;
        }

        audioSource.clip = musicClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    /// <summary>
    /// Pauses the currently playing background music.
    /// </summary>
    public void PauseBackgroundMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// Resumes the paused background music.
    /// </summary>
    public void ResumeBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// Stops the background music.
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Checks if the background music is currently playing.
    /// </summary>
    /// <returns>Returns true if the music is playing, otherwise false.</returns>
    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    /// <summary>
    /// Sets the volume of the background music.
    /// </summary>
    /// <param name="volume">The volume level to set.</param>
    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }

    /// <summary>
    /// Toggles the mute state of the background music.
    /// </summary>
    public void ToggleMusic(bool status)
    {
        if (audioSource != null)
        {
            audioSource.mute = status;
        }
    }

    public IEnumerator FadeAudio(float endVolume, float fadeDuration)
    {
        float timeElapsed = 0f;
        float startVolume = audioSource.volume;

        if (endVolume > startVolume)
        {
            audioSource.Play();
        }

        while (timeElapsed < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = endVolume;

        if (endVolume == 0f)
        {
            audioSource.Stop();
        }
    }
}
