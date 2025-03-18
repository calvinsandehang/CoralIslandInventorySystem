using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, ISubscriber
{
    private SfxController audioManager;
    private AudioSource audioSource;
    private Coroutine audioFinishedCheckCoroutine;
    private AudioSystemManager audioSystemManager;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSystemManager = AudioSystemManager.Instance;
    }

    private void Start()
    {
        Subscribe();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    IEnumerator AudioFinishedCheck()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Notify Audio Manager
        if (audioManager != null)
        {
            audioManager.RecycleAudioPlayer(this);
        }

        StopCoroutine(audioFinishedCheckCoroutine);
    }

    public void InitializeAudioPlayer(SfxController audioManager)
    {
        this.audioManager = audioManager;
    }

    public void PlayAudioClip(AudioClip clip, float volume, Transform audioSourceTransform)
    {
        bool isSfxMuted = audioSystemManager.AudioSetting.MuteSFX.Value;
        float sfxVolume = audioSystemManager.AudioSetting.SFXVolume.Value;

        if (isSfxMuted)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = volume * sfxVolume;
        }

        audioSource.clip = clip;
        audioSource.transform.position = audioSourceTransform.position;
        audioSource.spatialBlend = 1;
        audioSource.Play();

        audioFinishedCheckCoroutine = StartCoroutine(AudioFinishedCheck());
    }

    public void PlayAudioClip(AudioClip clip, float volume)
    {
        bool isSfxMuted = audioSystemManager.AudioSetting.MuteSFX.Value;
        float sfxVolume = audioSystemManager.AudioSetting.SFXVolume.Value;

        if (isSfxMuted)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = volume * sfxVolume;
        }

        audioSource.clip = clip;
        audioSource.Play();

        audioFinishedCheckCoroutine = StartCoroutine(AudioFinishedCheck());
    }

    private void StopAudioClip(AudioClip clip)
    {
        if (audioSource.clip == clip)
        {
            audioSource.Stop();
        }
    }

    private void OnSfxVolumeChanged(float volume)
    {
        audioSource.volume = volume;
    }

    private void OnSfxMuted(bool status)
    {
        audioSource.mute = status;
    }

    public void Subscribe()
    {
        CoralIslandEvent.OnToggleSfxSound += OnSfxMuted;
        CoralIslandEvent.OnOnSfxVolumeChanged += OnSfxVolumeChanged;
        CoralIslandEvent.OnStopSfx += StopAudioClip;
    }

    public void Unsubscribe()
    {
        CoralIslandEvent.OnToggleSfxSound -= OnSfxMuted;
        CoralIslandEvent.OnOnSfxVolumeChanged -= OnSfxVolumeChanged;
        CoralIslandEvent.OnStopSfx -= StopAudioClip;
    }


}
