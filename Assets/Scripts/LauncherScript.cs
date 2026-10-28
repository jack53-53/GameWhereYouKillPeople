using Unity.VisualScripting;
using UnityEngine;

public class LauncherScript : MonoBehaviour
{
    public float force;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null || other.gameObject.GetComponentInChildren<Rigidbody>() != null)
        {
            Rigidbody RB = other.gameObject.GetComponent<Rigidbody>();
            if(RB == null)
            {
                RB = other.gameObject.GetComponentInChildren<Rigidbody>();
            }
            RB.angularVelocity = Vector3.up * force;
        }
    }
}
