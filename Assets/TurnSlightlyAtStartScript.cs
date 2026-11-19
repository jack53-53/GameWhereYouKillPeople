using UnityEngine;

public class TurnSlightlyAtStartScript : MonoBehaviour
{

    public float TurnAmmount = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.Rotate(0, UnityEngine.Random.Range(-TurnAmmount, TurnAmmount), 0);
    }
}
