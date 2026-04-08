using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for listening to scene changes

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource mediaSource;
    [SerializeField] private AudioSource soundFxObject;

    [Header("Audio Clips")]
    public AudioClip door_close_clip;
    public AudioClip door_open_clip;
    [SerializeField] private AudioClip[] sceneMusics;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 1. Subscribe to the Scene Load event
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 2. Unsubscribe when destroyed
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 3. This automatically runs EVERY TIME a scene loads or reloads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.buildIndex);
    }

    public void PlaySceneMusic(int sceneIndex)
    {
        // Safety check to prevent errors if you haven't assigned music for a scene yet
        if (sceneIndex >= 0 && sceneIndex < sceneMusics.Length)
        {
            AudioClip correctClip = sceneMusics[sceneIndex];

            // Only change and restart the music if it's DIFFERENT from what's currently playing.
            // This prevents the song from awkwardly restarting from 0:00 if you 
            // reload the level after dying!
            if (mediaSource.clip != correctClip)
            {
                mediaSource.clip = correctClip;
                mediaSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("AudioManager: No scene music assigned for build index " + sceneIndex);
        }
    }

    public void PlayBossMusic(AudioClip bossClip)
    {
        // Switch to the boss music if it isn't playing already
        if (mediaSource.clip != bossClip)
        {
            mediaSource.clip = bossClip;
            mediaSource.Play();
        }
    }

    public void PlaySoundFx(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float audioClip_Length = audioSource.clip.length;
        Destroy(audioSource.gameObject, audioClip_Length);
    }

    public void door_close_sfx() { PlaySoundFx(door_close_clip, transform, 1f); }
    public void door_open_sfx() { PlaySoundFx(door_open_clip, transform, 1f); }
}