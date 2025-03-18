using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetting : MonoBehaviour
{
    [SerializeField]
    private SOFloatData _bgmVolume;
    public SOFloatData BGMVolume => _bgmVolume;
    [SerializeField]
    private SOFloatData _sfxVolume;
    public SOFloatData SFXVolume => _sfxVolume;
    [SerializeField]
    private SOBoolData _muteBGM;
    public SOBoolData MuteBGM => _muteBGM;
    [SerializeField]
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
