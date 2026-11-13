using UnityEngine;

public class ElevatorDoorScript : MonoBehaviour
{
    public Transform ClosedPos;
    public Transform OpenPos;
    public float Closed;

    private void Update()
    {
        this.transform.position = Vector3.Lerp(ClosedPos.position, OpenPos.position, Closed);
    }
}
