using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptSairDeCasa : MonoBehaviour
{
    public TextMeshProUGUI Texto;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<playerscript>() != null)
        {
            playerscript PS = other.GetComponent<playerscript>();
            if(PS.IsCivillian == false)
            {
                if (Texto != null)
                {
                    Texto.text = "LOADING...";
                }
                SceneManager.LoadScene("Level1Torre");
            }
        }
    }
}
