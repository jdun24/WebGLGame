using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using packet;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [Header("Mutable")]
    public AudioSource soundFXObject;

    [Header("ReadOnly")]
    private Dictionary<Enum, AudioClip> soundMap;

    private List<AudioSource> activeAudioSources = new();

    // -------------------------------------------------------------------
    // Handle events

    public void PlaySound(Enum type, Transform transform, float volume, float delay = 0f, bool loop = false)
    {
        // Spawn in object
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);

        // Assign the AudioClip
        if (soundMap.TryGetValue(type, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;

            if (audioSource.clip != null)
            {
                if (delay > 0)
                {
                    audioSource.PlayDelayed(delay);
                }
                else
                {
                    audioSource.Play();
                }
                Destroy(audioSource.gameObject, audioSource.clip.length + delay);
                activeAudioSources.Add(audioSource);
            }
            else
            {
                Debug.LogError("Audio clip is null or not found: " + type);
                Destroy(audioSource.gameObject);
            }
        }
        else
        {
            Debug.LogError("Sound effect not found: " + type);
            Destroy(audioSource.gameObject);
        }
    }

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        soundMap = new Dictionary<Enum, AudioClip>();
        InitializeSounds();
    }

    private void InitializeSounds()
    {
        LoadSoundsForEnumType(typeof(SFX.Music.Asteroid));
        LoadSoundsForEnumType(typeof(SFX.Music.Cave));
        LoadSoundsForEnumType(typeof(SFX.Cave));
        LoadSoundsForEnumType(typeof(SFX.Player));
        LoadSoundsForEnumType(typeof(SFX.Satellite));
        LoadSoundsForEnumType(typeof(SFX.Robot.Status));
        LoadSoundsForEnumType(typeof(SFX.Robot.Pickup));
        LoadSoundsForEnumType(typeof(SFX.Robot.Dropped));
        LoadSoundsForEnumType(typeof(SFX.MiniGame));
        LoadSoundsForEnumType(typeof(SFX.Extractor.Status));
        LoadSoundsForEnumType(typeof(SFX.UI));
    }

    private void LoadSoundsForEnumType(Type enumType)
    {
        foreach (Enum effect in Enum.GetValues(enumType))
        {
            var resourcePath = GetResourcePath(effect);
            if (!string.IsNullOrEmpty(resourcePath))
            {
                var clip = Resources.Load<AudioClip>(resourcePath);
                if (clip != null)
                {
                    soundMap.Add(effect, clip);
                    // Debug.Log("Loaded sound for " + effect + ": " + resourcePath);
                }
                else
                {
                    Debug.LogError("Failed to load AudioClip for " + effect + ": " + resourcePath);
                }
            }
            else
            {
                Debug.LogWarning("No resource path found for " + effect);
            }
        }
    }

    private string GetResourcePath(Enum effect)
    {
        var type = effect.GetType();
        var memInfo = type.GetMember(effect.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(SFX.ResourcePathAttribute), false);
        return attributes.Length > 0 ? ((SFX.ResourcePathAttribute)attributes[0]).resourcePath : null;
    }

    public void StopAllSounds()
    {
        for (int i = activeAudioSources.Count - 1; i >= 0; i--)
        {
            AudioSource source = activeAudioSources[i];
            if (source != null)
            {
                source.Stop();
                GameObject.Destroy(source.gameObject);
            }
        }
        activeAudioSources.Clear(); // Clear the list after stopping all sounds
    }

    public void StopSoundsOfType(Type enumType)
    {
        foreach (var entry in soundMap)
        {
            if (entry.Key.GetType() == enumType)
            {
                StopAudioSource(entry.Key);
            }
        }
    }

    public void StopSound(Enum soundEnum)
    {
        StopAudioSource(soundEnum);
    }

    private void StopAudioSource(Enum soundEnum)
    {
        List<AudioSource> sourcesToRemove = new List<AudioSource>();

        for (int i = activeAudioSources.Count - 1; i >= 0; i--)
        {
            AudioSource source = activeAudioSources[i];
            // Check if the source is null or has been destroyed
            if (source == null || source.Equals(null))
            {
                activeAudioSources.RemoveAt(i);
                continue;
            }

            if (source.clip == soundMap[soundEnum])
            {
                source.Stop();
                GameObject.Destroy(source.gameObject);  // Destroy the object
                sourcesToRemove.Add(source);
            }
        }

        // Cleanly remove the sources from the active list
        foreach (AudioSource source in sourcesToRemove)
        {
            activeAudioSources.Remove(source);
        }
    }

    public void PlayRandomSoundOfType(Type enumType, Transform transform, float volume, float delay = 0f)
    {
        List<AudioClip> clipsOfType = new List<AudioClip>();

        // Gather all clips that match the provided enum type
        foreach (var entry in soundMap)
        {
            if (entry.Key.GetType() == enumType)
            {
                clipsOfType.Add(entry.Value);
            }
        }

        // Check if there are any clips to play
        if (clipsOfType.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, clipsOfType.Count);  // Get a random index
            AudioClip randomClip = clipsOfType[randomIndex];  // Select a random clip

            // Create and configure the AudioSource
            AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
            audioSource.clip = randomClip;
            audioSource.volume = volume;

            if (audioSource.clip != null)
            {
                    if (delay > 0)
                    {
                        audioSource.PlayDelayed(delay);
                    }
                    else
                    {
                        audioSource.Play();
                    }

                    Destroy(audioSource.gameObject, audioSource.clip.length + delay);
                    activeAudioSources.Add(audioSource);
            }
            else
            {
                Debug.LogError("Randomly selected audio clip is null");
                Destroy(audioSource.gameObject);  // Destroy immediately if clip is invalid
            }
        }
        else
        {
            Debug.LogWarning("No audio clips found for enum type: " + enumType.Name);
        }
    }

}
