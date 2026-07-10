using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Other")]
    [SerializeField] AudioMixer mixer;

    [Header("Music")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip musicClip;

    [Header("SFX")]
    [SerializeField] AudioSource sFXSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic(musicClip);
    }

    private void Update()
    {
        UpdateVolume();
    }

    void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sFXClip)
    {
        sFXSource.PlayOneShot(sFXClip);
    }

    void UpdateVolume()
    {
        mixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master", 1)) * 20);
        mixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music", 1)) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX", 1)) * 20);
    }
}
