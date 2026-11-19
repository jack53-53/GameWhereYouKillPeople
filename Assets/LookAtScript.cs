using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        if(Target != null)
        {
            transform.LookAt(Target);
        }
    }
}
