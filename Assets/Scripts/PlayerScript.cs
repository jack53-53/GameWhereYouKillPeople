using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerscript : MonoBehaviour
{
    public float Sens = 0.1f;
    public float Speed = 5f;
    public int DMG;
    public RawImage KillEffect;

    public Rigidbody RB;
    public Camera PlayerCamera;

    private Vector2 Dire;
    private Vector2 Look;
    private bool Fired;
    private int StageKillEffect; //suboptimal

    public GameObject BulletHolePrefab;

    private float cameraPitch;
    LayerMask layerMask;

    private bool Interagiu;

    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        Color color = KillEffect.color;
        color.a = 0f;
        KillEffect.color = color;
    }

    void Update()
    {

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

        PlayerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);

        transform.Rotate(Vector3.up * Look.x * Sens);

       

        if (Fired) //decal do buraco do tiro
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
        KillEffect.CrossFadeAlpha(0.7f, 0.2f, false);
        Debug.Log("É suposto mudar algo");

        yield return new WaitForSeconds(0.2f);

        KillEffect.CrossFadeAlpha(0.9f, 0.2f, false);

        yield return new WaitForSeconds(0.2f);

        KillEffect.CrossFadeAlpha(0f, 2f, false);
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