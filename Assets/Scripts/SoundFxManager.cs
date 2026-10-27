using Unity.VisualScripting;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager instance;
    [SerializeField] private AudioSource m_AudioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundClip(AudioClip Som, Transform lugar, float volume)
    {
        AudioSource audioSource = Instantiate(m_AudioSource, lugar.position, Quaternion.identity);
        audioSource.clip = Som;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }
}
