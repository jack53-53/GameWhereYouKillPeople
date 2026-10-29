using UnityEngine;

public class DoorParentScript : MonoBehaviour
{
    public float Open;
    public ElevatorDoorScript Esquerda;
    public ElevatorDoorScript Direira;
    private bool IsPlayerFar;
    public float DoorSpeed;

    private void Update()
    {
        Esquerda.Closed = Open;
        Direira.Closed = Open;
        if (IsPlayerFar)
        {
            if (Open > 0)
            {
                Open -= DoorSpeed;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPlayerFar = false;
            if(Open < 1)
            {
                Open += DoorSpeed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPlayerFar = true;
        }

    }
}
