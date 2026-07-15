using System.Collections;
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
    [SerializeField] public AudioClip buttonClip;
    [SerializeField] public AudioClip materialMineClip;
    [SerializeField] public AudioClip materialBreakClip;
    [SerializeField] public AudioClip walkClip;
    [SerializeField] public AudioClip swooshClip;
    [SerializeField] public AudioClip wolfDamageClip;
    [SerializeField] public AudioClip damageClip;
    [SerializeField] public AudioClip recruitClip;
    [SerializeField] public AudioClip unRecruitClip;
    [SerializeField] public AudioClip gatesClip;
    [SerializeField] public AudioClip dayStartClip;
    [SerializeField] public AudioClip summaryClip;

    bool playingWalk;

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
        if (sFXClip != null)
        {
            sFXSource.PlayOneShot(sFXClip);
        }        
    }

    public IEnumerator WalkLoop()
    {
        if (!playingWalk)
        {
            playingWalk = true;
            PlaySFX(walkClip);
            yield return new WaitForSeconds(walkClip.length);
            playingWalk = false;
        }
    }

    void UpdateVolume()
    {
        mixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master", 1)) * 20);
        mixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music", 1)) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX", 1)) * 20);
    }
}
