using UnityEngine;

public class barrelScript : MonoBehaviour
{
    public GameObject missel;
    public void Explode()
    {
        Instantiate(missel);
        Destroy(this.gameObject);
    }
}
