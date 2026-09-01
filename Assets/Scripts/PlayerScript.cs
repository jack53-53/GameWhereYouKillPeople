using UnityEngine;
using UnityEngine.InputSystem;

public class playerscript : MonoBehaviour
{
    public float Sens = 0.1f;
    public float Speed = 5f;

    public Rigidbody RB;
    public Camera PlayerCamera;

    private Vector2 Dire;
    private Vector2 Look;
    private bool Fired;

    public GameObject BulletHolePrefab;

    private float cameraPitch;
    LayerMask layerMask;

    private bool Interagiu;

    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        // Movement relative to where the player is facing
        Vector3 move =
            transform.right * Dire.x +
            transform.forward * Dire.y;

        Vector3 velocity = RB.linearVelocity;

        RB.linearVelocity = new Vector3(
            move.x * Speed,
            velocity.y,
            move.z * Speed
        );


        // Vertical camera rotation
        cameraPitch -= Look.y * Sens;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        PlayerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);

        // Horizontal player rotation
        transform.Rotate(Vector3.up * Look.x * Sens);

        if (Fired)
        {
             RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))

        {
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
                    GameObject bulletHole = Instantiate(
            BulletHolePrefab,
            hit.point + hit.normal * 0.001f,
            Quaternion.LookRotation(-hit.normal)
        );

        Destroy(bulletHole, 10f);
        }
        else
        {
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        Fired = false;
        }
    }

    public void OnAttack(InputValue e)
    {
        Fired = e.isPressed;
    }

    public void OnMove(InputValue e)
    {
        Dire = e.Get<Vector2>();
    }

    public void OnLook(InputValue e)
    {
        Look = e.Get<Vector2>();
    }
    public void OnInteract(InputValue e)
    {
        Interagiu = e.isPressed;
    }
}