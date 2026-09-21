using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerscript : MonoBehaviour
{
    public float Sens = 0.1f;

    public int HP;
    public float Speed = 5f;
    public int DMG;
    public RawImage KillEffect;
    public float MultiplierKillEffectSpeed;

    public Rigidbody RB;
    public Camera PlayerCamera;
    private int SelectedWeapon; //0 = sem arma, 1 = pistola, 2 = rifle, 3 = granada, 4 = bazooka, 5 = BFG

    private Vector2 Dire;
    public float jumpStrenght = 0f;
    public float timeBetweenJumps = 0f;
    private float _timerBetweenJumps = 0f;
    private Vector2 Look;
    private bool Fired;
    private float t;

    public GameObject BulletHolePrefab;
    public GameObject SprayPrefab;
    private float cameraRoll = 0f;

    private float cameraPitch;
    LayerMask layerMask;

    private bool Interagiu;
    private bool Pulou;
    private bool Sprayou;
    [Header("COOLDOWN ENTRE TIROS DE CADA ARMA")]
    public float meleeCL;
    public float pistolaCL;
    public float rifleCL;
    public float rocketCL;
    public float doideraCL;
    private float _tiroCL;

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

        if (Sprayou)
        {
            RaycastHit hit;
            if(Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, 5f, layerMask))
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up,hit.normal);
                GameObject SprayPng = Instantiate(SprayPrefab,hit.point + hit.normal * 0.001f,rotation);
                // Destroy(SprayPng);
            }
            Sprayou = false;
        }
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

        if (cameraRoll != targetRoll)
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
            switch (SelectedWeapon)
            {
                case 1:
                    if(_tiroCL <= 0)
                    {

                    }
                    break;

            }

        }
    }
        private void atirar(int DMG)
        {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            GameObject bulletHole = Instantiate(BulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(-hit.normal));//TODO:atirar uma vez, esperar o cooldown de cada arma e atirar dnv
            Destroy(bulletHole, 10f);
            if (hit.transform.gameObject)
            {
                if (hit.transform.gameObject.GetComponent<EnemyScript>() != null)
                {
                    EnemyScript p = hit.transform.gameObject.GetComponent<EnemyScript>();
                    p.HP -= DMG;
                    if (p.HP <= 0)
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

    IEnumerator KillScreenEffect()
    {
        float t = 0f;
        Color c = KillEffect.color;
        while (KillEffect.color.a != 0.5f)
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
        while (KillEffect.color.a != 0.7f)
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

    public void takeDamage(int DMG)
    {
        HP -= DMG;
        StartCoroutine(DamageCamera());
    }

    IEnumerator DamageCamera()
    {
        float targetRoll = UnityEngine.Random.Range(cameraPitch - 10f, cameraPitch - 30f);
        float duration = 0.01f;
        float timer = 0f;
        float _cameraPitch = cameraPitch;
        

        float startingRoll = cameraPitch;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraPitch = Mathf.Lerp(
                startingRoll,
                targetRoll,
                timer / duration
            );

            yield return null;
        }

        cameraPitch = targetRoll;
        StartCoroutine(CameraRecover(_cameraPitch));
    }

    IEnumerator CameraRecover(float X) //pra camera fazer o recover, eu to salvando o valor original antes de mudar, e dai eu to fazendo a mesma função dnv so que sem o recover
    {
        float targetRoll = X;
        float duration = 0.1f;
        float timer = 0f;

        float startingRoll = cameraPitch;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraPitch = Mathf.Lerp(
                startingRoll,
                targetRoll,
                timer / duration
            );

            yield return null;
        }

        cameraPitch = targetRoll;
    }

    public void OnAttack(InputAction.CallbackContext e)
    {
        //Fired = e.isPressed; //tem que fazer ser automatico
        if (e.performed)
        {
            Fired = true;
        }
        else if (e.canceled)
        {
            Fired = false;
        }
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

    public void OnSpray(InputValue e)
    {
        Sprayou = e.isPressed;
    }
}