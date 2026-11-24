using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public bool Start;
    public bool Covert;
    public AudioClip CovertEasterEgg;
    public AudioSource AS;
    public bool Settings;
    public bool Settings2;
    public bool Exit;
    public bool load;
    public GameObject[] ShitToDisableWhenSettings;
    public GameObject[] ShitToEnableWhenSettings;
    [SerializeField]
    public UnityEngine.UI.Slider Musica;
    public UnityEngine.UI.Slider FX;
    public AudioSource Soundtrack;
    public GameManagerScript GMS;
    public TMP_Dropdown Janela;

    void Update()
    {
        if (Start)
        {
            SceneManager.LoadScene(1);
        }
        else if (Covert)
        {
            AS.clip = CovertEasterEgg;
            AS.Play();
            Covert = false;
        }
        else if (Settings)
        {
            //coisa pra um caralho
            int i = 0;
            while (i < ShitToDisableWhenSettings.Length)
            {
                ShitToDisableWhenSettings[i].gameObject.SetActive(false);
                i++;
            }
            i = 0;
            while (i < ShitToEnableWhenSettings.Length)
            {
                ShitToEnableWhenSettings[i].gameObject.SetActive(true);
                i++;
            }
            Musica.value = GMS.SoundMusicVolume;
            FX.value = GMS.SoundFxVolume;
            Settings = false;
        }
        else if (Settings2)
        {
            int i = 0;
            while (i < ShitToDisableWhenSettings.Length)
            {
                ShitToDisableWhenSettings[i].gameObject.SetActive(true);
                i++;
            }
            i = 0;
            while (i < ShitToEnableWhenSettings.Length)
            {
                ShitToEnableWhenSettings[i].gameObject.SetActive(false);
                i++;
            }
            GMS.SoundMusicVolume = Musica.value;
            GMS.SoundFxVolume = FX.value;
            Soundtrack.volume = GMS.SoundMusicVolume;
            switch (Janela.value)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
            }
            Settings2 = false;
        }
        else if (Exit)
        {
            Application.Quit();
        }
        else if (load)
        {
            //ainda mais codigo, nao vamos ter saves, ne? 23/11
        }
    }
    public void StartThisShitUp()
    {
        Start = true;
    }
    public void Covertt()
    {
        Covert = true;
    }
    public void Settingss()
    {
        Settings = true;
    }
    public void foge()
    {
        Exit = true;
    }
    public void carregar()
    {
        load = true;
    }
    public void descarregar()
    {
        Settings2 = true;
    }
}
