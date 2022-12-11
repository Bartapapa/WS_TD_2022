using GSGD1;
using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Mixers")]
    [SerializeField] AudioMixer _masterMixer;

    [Header("Parameters")]
    [SerializeField]
    [Range(0f, 1f)]
    private float _masterVolume = 1f;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource _musicSource, _sfxSource;

    public void SetVolume(float volume)
    {
        _masterVolume = volume;
        _masterMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }

    public void PlaySFX(GameObject owner, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log(owner.name + " wanted to play a clip, but there's nothing there!");
            return;
        }
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        if (music == null)
        {
            Debug.Log("No music clip to be found!");
            return;
        }
        _musicSource.clip = music;
        _musicSource.Play();
    }

    public void NullMusic()
    {
        _musicSource.Stop();
    }
}