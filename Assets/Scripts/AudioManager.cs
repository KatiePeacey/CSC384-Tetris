using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private float normalPitch = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    public void SetMusicPitch(float pitch)
    {
        if (musicSource != null) {
            musicSource.pitch = pitch;
        }
    }

    public void ResetMusicPitch()
    {
        SetMusicPitch(normalPitch);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null) {
            sfxSource.PlayOneShot(clip);
        }
    }
}
