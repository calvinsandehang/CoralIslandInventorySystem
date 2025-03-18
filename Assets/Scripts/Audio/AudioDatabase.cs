// [DefaultExecutionOrder(-999999999)] // Ensure this script executes before all others
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a database of audio clips to ensure efficient audio clip retrieval.
/// This class initializes a dictionary from a list of audio data scriptable objects.
/// </summary>
public class AudioDatabase : MonoBehaviour
{
    /// <summary>
    /// List of SOAudioData containing audio clips and their associated metadata.
    /// </summary>
    public List<SOAudioData> AudioDataList = new List<SOAudioData>();

    /// <summary>
    /// Dictionary mapping audio clip names to their corresponding AudioClip objects.
    /// </summary>
    public Dictionary<string, AudioClip> AudioDataDictionary = new Dictionary<string, AudioClip>();

    /// <summary>
    /// Initializes the audio database by populating the dictionary with entries from the AudioDataList.
    /// </summary>
    public void InitializeDatabase()
    {
        // Clear existing entries to avoid duplicates when re-initializing.
        AudioDataDictionary.Clear();

        foreach (var audioData in AudioDataList)
        {
            if (audioData != null && !AudioDataDictionary.ContainsKey(audioData.Name))
            {
                AudioDataDictionary.Add(audioData.Name, audioData.Clip);
            }
        }
    }

    /// <summary>
    /// Retrieves an AudioClip by its name.
    /// </summary>
    /// <param name="name">The name of the audio clip to retrieve.</param>
    /// <returns>The AudioClip if found; otherwise, null.</returns>
    public AudioClip GetAudioClip(string name)
    {
        if (AudioDataDictionary.TryGetValue(name, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            Debug.LogError($"AudioDatabase: No audio clip found with name: {name}");
            return null;
        }
    }
}