using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnScript : MonoBehaviour
{
    public AudioSource MusicaMorte;
    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        MusicaMorte.Play();
        MusicaMorte.loop = true;
        MusicaMorte.volume = GameManagerScript.instance.SoundMusicVolume;
    }
    public void Renascer()
    {
        SceneManager.LoadScene(GameManagerScript.instance.Cena);
    }
}
