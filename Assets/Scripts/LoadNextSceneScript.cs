using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneScript : MonoBehaviour
{
    public TextMeshProUGUI Texto;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Texto.text = "LOADING...";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
