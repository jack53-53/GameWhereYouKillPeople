using UnityEngine;

public class gibsScript : MonoBehaviour
{
    public Rigidbody RB;
    public float MinVel;
    public float MaxVel;
    private void Start()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = Mathf.Abs(randomDirection.y) + 0.5f;
        randomDirection.Normalize();

        float randomSpeed = Random.Range(MinVel, MaxVel);

        RB.AddForce(randomDirection * randomSpeed, ForceMode.Impulse);
    }
}
