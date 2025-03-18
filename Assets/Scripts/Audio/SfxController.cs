using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{

    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    [SerializeField] private int _poolCount = 5;

    private Stack<AudioPlayer> _audioPlayerPool;
    private bool isSfxMuted = false;

    private AudioSystemManager audioSystemManager;

    public void Initialize(AudioSystemManager audioSystemManager)
    {
        this.audioSystemManager = audioSystemManager;
        InitializePool();
    }


    void InitializePool()
    {
        _audioPlayerPool = new Stack<AudioPlayer>();
        for (int i = 0; i < _poolCount; i++)
        {
            CreateNewAudioPlayer();
        }
    }

    private void CreateNewAudioPlayer()
    {
        var newAudioPlayer = Instantiate(_audioPlayerPrefab, transform); // Parent to AudioManager
        newAudioPlayer.InitializeAudioPlayer(this);
        newAudioPlayer.gameObject.SetActive(false); // Initially deactivate
        _audioPlayerPool.Push(newAudioPlayer);
    }

    public void PlayAudio(AudioClip clip, float volume, Transform audioSourceTransform)
    {
        AudioPlayer audioPlayer = GetAvailableAudioPlayer();

        if (isSfxMuted)
            volume = 0;

        float finalVolume = volume * audioSystemManager.AudioSetting.SFXVolume.Value;

        if (audioPlayer != null)
        {
            audioPlayer.gameObject.SetActive(true);
            audioPlayer.PlayAudioClip(clip, finalVolume, audioSourceTransform);
        }
    }
    public void PlayAudio(AudioClip clip, float volume)
    {
        AudioPlayer audioPlayer = GetAvailableAudioPlayer();

        if (isSfxMuted)
            volume = 0;

        float finalVolume = volume * audioSystemManager.AudioSetting.SFXVolume.Value;

        if (audioPlayer != null)
        {
            audioPlayer.gameObject.SetActive(true);
            audioPlayer.PlayAudioClip(clip, finalVolume);
        }
    }

    private AudioPlayer GetAvailableAudioPlayer()
    {
        if (_audioPlayerPool.Count > 0)
        {
            return _audioPlayerPool.Pop();
        }
        else
        {
            CreateNewAudioPlayer();
            return _audioPlayerPool.Pop();
        }
    }

    // **** Placeholder for Recycling: Listen for 'audio finished' signal  on each AudioPlayer ****
    public void RecycleAudioPlayer(AudioPlayer audioPlayer)
    {
        _audioPlayerPool.Push(audioPlayer);
        audioPlayer.gameObject.SetActive(false);
    }

    public void ToggleSfx(bool status)
    {
        isSfxMuted = status;
    }
}

internal class ShowInInspectorAttribute : Attribute
{
}