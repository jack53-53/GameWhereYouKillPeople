using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{

    [Header("Movimento")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Olhadas")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    private float xRotation = 0f;
    private CharacterController controller;
    private Vector3 velocity;
    public float PistolCooldown; //cooldown between each shot, in seconds
    private float TotPistCol;
    [Header("PoucoImportante")]
    public MeshRenderer BangRender;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        TotPistCol = PistolCooldown;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        PistolCooldown-= Time.deltaTime;

        //PRA GIRAR A ARMA, ELA TEM QUE SEGUIR O X DA CAMERA, MOVENDO O Y DELA PRA IR PRA CIMA E PRA BAIXO E GIRANDO O Z
        //feito, era so colocar parent
        // GunT.transform.position = new Vector3(playerCamera.transform.position.x, GunT.transform.position.y, GunT.transform.position.z);
    }

    void FixedUpdate()
    {
        BangRender.enabled = false;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnFire()
    {
        if(PistolCooldown <= 0){
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayerMask.GetMask("Wall")))

        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        PistolCooldown = TotPistCol;
        BangRender.enabled = true;
        }
    }
}