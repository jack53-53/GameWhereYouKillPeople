using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }
    public float SoundVolume;
    public float SoundFxVolume;
    public float SoundMusicVolume;
    public float Sensitivity;
    public Scene Cena;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
