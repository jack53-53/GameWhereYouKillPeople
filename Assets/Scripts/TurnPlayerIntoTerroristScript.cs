using UnityEngine;

public class TurnPlayerIntoTerroristScript : MonoBehaviour
{
    public AudioClip Narracao;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<playerscript>() != null)
        {
            SoundFxManager.instance.PlaySoundClip(Narracao, other.transform, GameManagerScript.instance.SoundFxVolume, 1f, 1f);
            playerscript ps = other.gameObject.GetComponent<playerscript>();
            ps.IsCivillian = false;
            Destroy(this.gameObject);
        }
    }
}
