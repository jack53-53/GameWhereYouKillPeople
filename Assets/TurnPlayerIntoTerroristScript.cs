using UnityEngine;

public class TurnPlayerIntoTerroristScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<playerscript>() != null)
        {
            playerscript ps = other.gameObject.GetComponent<playerscript>();
            ps.IsCivillian = false;
            Destroy(this.gameObject);
        }
    }
}
