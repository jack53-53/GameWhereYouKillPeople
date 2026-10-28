using Unity.VisualScripting;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager instance { get; private set; }
    [SerializeField] private AudioSource m_AudioSource;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Toca um som aonde voce especificar
    /// </summary>
    /// <param name="volume">volume,0, 1.</param>
    /// <param name="PitchBaixo">O quao baixo o pitch vai, minimo de -3</param>
    /// <param name="PitchAlto">O quao Alto o pitch vai, Maximo de 3</param>
    public void PlaySoundClip(AudioClip Som, Transform lugar, float volume, float PitchBaixo, float PitchAlto)
    {
        AudioSource audioSource = Instantiate(m_AudioSource, lugar.position, Quaternion.identity);
        audioSource.clip = Som;
        audioSource.volume = volume;
        audioSource.pitch = UnityEngine.Random.Range(PitchBaixo, PitchAlto); //lembrar de deixar entre -3 e 3
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }
}
