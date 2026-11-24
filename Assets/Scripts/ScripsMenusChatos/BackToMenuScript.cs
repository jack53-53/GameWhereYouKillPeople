using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuScript : MonoBehaviour
{
    public void Voltar()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
