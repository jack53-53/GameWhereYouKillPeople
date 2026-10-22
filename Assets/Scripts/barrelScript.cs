using UnityEngine;

public class barrelScript : MonoBehaviour
{
    public GameObject missel;
    public void Explode()
    {
        GameObject go = Instantiate(missel, this.transform.position, Quaternion.identity, this.gameObject.transform);
        RocketScript RS = go.GetComponentInChildren<RocketScript>();
        //RS.radius = RS.radius * 2;
        //RS.power = RS.power * 2;
        Destroy(this.gameObject);
    }
}