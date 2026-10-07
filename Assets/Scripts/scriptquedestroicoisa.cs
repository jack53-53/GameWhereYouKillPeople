using UnityEngine;

public class scriptquedestroicoisa : MonoBehaviour
{

    public GameObject coisaASerDestruida;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(coisaASerDestruida);
    }
}
