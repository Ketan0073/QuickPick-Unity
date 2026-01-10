using UnityEngine;

public class AudioManager : MonoBehaviour

{
    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip gameOverSound;
    
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    
    static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static void PlaySound(AudioClip clip)
    {
        if (instance != null && instance.sfxSource != null && clip != null)
            instance.sfxSource.PlayOneShot(clip);
    }
}

