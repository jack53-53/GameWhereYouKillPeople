using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptSairDeCasa : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<playerscript>() != null)
        {
            playerscript PS = other.GetComponent<playerscript>();
            if(PS.IsCivillian == false)
            {
                SceneManager.LoadScene("Level1Torre");
            }
        }
    }
}
