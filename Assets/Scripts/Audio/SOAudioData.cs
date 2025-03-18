using UnityEngine;

[CreateAssetMenu(fileName = "AudioData_", menuName = "Audio/AudioData")]
public class SOAudioData : ScriptableObject
{
    // public int ID;
    public string Name;
    public AudioClip Clip;
}

