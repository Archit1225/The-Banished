using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip door_close_clip;
    public AudioClip door_open_clip;
    public static AudioManager instance;

    [SerializeField] private AudioSource mediaSource;
    [SerializeField] private AudioSource soundFxObject;

    [Header("Scene Music")]
    [SerializeField] private AudioClip[] sceneMusics;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }
    private void Start()
    {
        SceneMusic(SceneManager.GetActiveScene().buildIndex);
    }

    public void SceneMusic(int sceneNo)
    {
        mediaSource.clip = sceneMusics[sceneNo];
        mediaSource.Play();
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

    public void door_close_sfx()
    {
        PlaySoundFx(door_close_clip, transform, 1f);
    }
    public void door_open_sfx()
    {
        PlaySoundFx(door_open_clip, transform, 1f);
    }
}
