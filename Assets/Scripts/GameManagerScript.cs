using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }
    public float SoundVolume;
    public float SoundFxVolume;
    public float SoundMusicVolume;
    public float Sensitivity;
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
