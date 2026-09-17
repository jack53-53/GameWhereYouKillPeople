using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class playerscript : MonoBehaviour
{
    public float Sens = 0.1f;
    public float Speed = 5f;
    public int DMG;
    public RawImage KillEffect;
    public float MultiplierKillEffectSpeed;

    public Rigidbody RB;
    public Camera PlayerCamera;

    private Vector2 Dire;
    public float jumpStrenght = 0f;
    public float timeBetweenJumps = 0f;
    private float _timerBetweenJumps = 0f;
    private Vector2 Look;
    private bool Fired;
    private float t;

    public GameObject BulletHolePrefab;
    private float cameraRoll = 0f;

    private float cameraPitch;
    LayerMask layerMask;

    private bool Interagiu;
    private bool Pulou;

    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        Color color = KillEffect.color;
        color.a = 0f;
        KillEffect.color = color;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
        if (Pulou)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y + jumpStrenght, transform.position.z);RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.2f) && _timerBetweenJumps < 0)
            {
                RB.AddForce(new Vector3(0, jumpStrenght, 0));
                //Pulou = false; //buffer(?) //ficou muito ruim
                _timerBetweenJumps = timeBetweenJumps;
            }
            Pulou = false;
        }
        _timerBetweenJumps -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector3 move =
            transform.right * Dire.x +
            transform.forward * Dire.y;

        Vector3 velocity = RB.linearVelocity;

        RB.linearVelocity = new Vector3(
            move.x * Speed,
            velocity.y,
            move.z * Speed
        );

        cameraPitch -= Look.y * Sens;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);


        float targetRoll = -Dire.x * 10f;


        if (Dire.x > 0)
        {
            targetRoll = -10f;
        }
        else if (Dire.x < 0)
        {
            targetRoll = 10f;
        }

        if(cameraRoll != targetRoll)
        {
            t = Time.deltaTime * 1;
           cameraRoll = Mathf.Lerp(cameraRoll, targetRoll, t * 8f);
        }
        else
        {
            t = 0f;
        }

        PlayerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, cameraRoll);

        transform.Rotate(Vector3.up * Look.x * Sens);


        if (Fired)
        {
            RaycastHit hit;
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))

            {
                Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                GameObject bulletHole = Instantiate(BulletHolePrefab,hit.point + hit.normal * 0.001f,Quaternion.LookRotation(-hit.normal));
                Destroy(bulletHole, 10f);
                if (hit.transform.gameObject)
                {
                    if(hit.transform.gameObject.GetComponent<EnemyScript>() != null)
                    {
                        EnemyScript p = hit.transform.gameObject.GetComponent<EnemyScript>();
                        p.HP -= DMG;
                        if(p.HP <= 0)
                        {
                            StartCoroutine(KillScreenEffect());
                            //piscar a tela azul estilo gta
                            //70% clareza
                            //90% clareza
                            //desce ate zero
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }
            Fired = false;
        }
    }

    IEnumerator KillScreenEffect()
    {
        float t = 0f;
        Color c = KillEffect.color;
        while (KillEffect.color.a != 0.7f)
        {
        t += Time.deltaTime * MultiplierKillEffectSpeed;
        c = KillEffect.color;
        c.a = Mathf.Lerp(0,0.7f,t /1);
        KillEffect.color = c;
        }
        yield return null;
        while (KillEffect.color.a != 0f)
        {
        t -= Time.deltaTime * MultiplierKillEffectSpeed;
        c = KillEffect.color;
        c.a = Mathf.Lerp(0,0.7f,t /1);
        KillEffect.color = c;
        }
        yield return null;
        while (KillEffect.color.a != 0.9f)
        {
        t += Time.deltaTime * MultiplierKillEffectSpeed;
        c = KillEffect.color;
        c.a = Mathf.Lerp(0,0.9f,t /1);
        KillEffect.color = c;
        }
        yield return null;       
        while (KillEffect.color.a != 0f)
        {
        t -= Time.deltaTime * MultiplierKillEffectSpeed;
        c = KillEffect.color;
        c.a = Mathf.Lerp(0,0.7f,t /1);
        KillEffect.color = c;
        }
        yield return null;

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

    public void OnJump(InputValue e)
    {
        Pulou = e.isPressed;
    }
}