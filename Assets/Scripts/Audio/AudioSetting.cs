using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioSetting : MonoBehaviour
{
    [SerializeField]
    [InlineEditor]
    private SOFloatData _bgmVolume;
    public SOFloatData BGMVolume => _bgmVolume;
    [SerializeField]
    [InlineEditor]
    private SOFloatData _sfxVolume;
    public SOFloatData SFXVolume => _sfxVolume;
    [SerializeField]
    [InlineEditor]
    private SOBoolData _muteBGM;
    public SOBoolData MuteBGM => _muteBGM;
    [SerializeField]
    [InlineEditor]
    private SOBoolData _muteSfx;
    public SOBoolData MuteSFX => _muteSfx;

    public void Initialize()
    {

    }

    private void OnValidate()
    {

    }
    public void SetBGMVolume(float volume)
    {
        _bgmVolume.Value = volume;
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume.Value = volume;
    }

    public void SetMuteBGM()
    {
        _muteBGM.Value = !_muteBGM.Value;
    }

    public void SetMuteSfx()
    {
        _muteSfx.Value = !_muteSfx.Value;
    }


}
