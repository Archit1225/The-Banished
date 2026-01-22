using UnityEngine;

public class AudioManager : MonoBehaviour
{
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
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        SceneMusic(0);
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
}
