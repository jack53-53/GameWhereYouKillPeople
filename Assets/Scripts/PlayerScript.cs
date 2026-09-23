using System.Collections;
using TMPro;
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
    public int SelectedWeapon; //0 = sem arma, 1 = pistola, 2 = rifle, 3 = granada, 4 = bazooka, 5 = BFG //talvez trocar a granada por uma shotgun?

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
    public float grandaCL;
    public float rocketCL;
    public float doideraCL;
    private float _tiroCL;
    [Header("DANO DE CADA ARMA")]

    public int meleeDMG;
    public int pistolDMG;
    public int rifleDMG;
    public int granadaDMG;
    public int rocketDMG;
    public int doideraDMG;
    private bool Reloading;
    //TODO: A PARTIR DAQUI ISSO NAO TEM EFEITO NO JOGO
    [Header("QUANTO CADA ARMA CONSEGUE LEVAR NO PENTE")]
    public int pistolMAG;
    public int rifleMAG;
    public int granadaMAG;
    public int rocketMAG;
    public int doideraMAG;
    private int _pistolMAG;
    private int _rifleMAG;
    private int _granadaMAG;
    private int _rocketMAG;
    private int _doideraMAG;
    [Header("QUANTO DE MUNIÇÃO DE CADA ARMA O PLAYER CONSEGUE LEVAR")]
    public int pistolMAX;
    public int rifleMAX;
    public int granadaMAX;
    public int rocketMAX;
    public int doideraMAX;
    [Header("quanto de munição o jogador tem agora")]
    public int _pistolMAX;
    public int _rifleMAX;
    public int _granadaMAX;
    public int _rocketMAX;
    public int _doideraMAX;
    public TextMeshProUGUI debugTXT;
    public TextMeshProUGUI vidaTXT;
    public TextMeshProUGUI ammoTXT;
    private string writeToScreen;
    

    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        Color color = KillEffect.color;
        color.a = 0f;
        KillEffect.color = color;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
         _pistolMAG = pistolMAG;
    _rifleMAG = rifleMAG;
    _granadaMAG = granadaMAG;
    _rocketMAG = rocketMAG;
    _doideraMAG = doideraMAG;
}

    void Update()
    {
        vidaTXT.text = HP.ToString();
        debugTXT.text = writeToScreen;
        if (Mouse.current != null)
            Fired = Mouse.current.leftButton.isPressed;
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
        _tiroCL -= Time.fixedDeltaTime;
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


        float targetRoll = -Dire.x * 7f;


        if (Dire.x > 0)
        {
            targetRoll = -7f;
        }
        else if (Dire.x < 0)
        {
            targetRoll = 7f;
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
        //Debug.Log("cooldown tiro:" + _tiroCL );
        //Debug.Log("atirando?:" + Fired.ToString() );

        PlayerCamera.transform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, cameraRoll);

        transform.Rotate(Vector3.up * Look.x * Sens);

        if (Reloading)
        {
            switch (SelectedWeapon)
            {
                case 0:
                    //inspecionar?
                    break;

                case 1:
                    if (_pistolMAG < pistolMAG)
                    {
                        if (_pistolMAX > 0)
                        {
                            int amountToReload = Mathf.Min(pistolMAG - _pistolMAG, _pistolMAX);
                            _pistolMAG += amountToReload;
                            _pistolMAX -= amountToReload;
                        }
                        else
                        {
                            Debug.Log("SEM BALA");
                        }
                        ammoTXT.text = (_pistolMAG.ToString() + "/" + _pistolMAX.ToString());
                    }
                    break;

                case 2:
                    if (_rifleMAG < rifleMAG)
                    {
                        if (_rifleMAX > 0)
                        {
                            int amountToReload = Mathf.Min(rifleMAG - _rifleMAG, _rifleMAX);
                            _rifleMAG += amountToReload;
                            _rifleMAX -= amountToReload;
                        }
                        else
                        {
                            Debug.Log("SEM BALA");
                        }
                        ammoTXT.text = (_rifleMAG.ToString() + "/" + _rifleMAX.ToString());
                    }
                    break;

                case 3:
                    if (_granadaMAG < granadaMAG)
                    {
                        if (_granadaMAX > 0)
                        {
                            int amountToReload = Mathf.Min(granadaMAG - _granadaMAG, _granadaMAX);
                            _granadaMAG += amountToReload;
                            _granadaMAX -= amountToReload;
                        }
                        else
                        {
                            Debug.Log("SEM BALA");
                        }
                        ammoTXT.text = (_granadaMAG.ToString() + "/" + _granadaMAX.ToString());
                    }
                    break;

                case 4:
                    if (_rocketMAG < rocketMAG)
                    {
                        if (_rocketMAX > 0)
                        {
                            int amountToReload = Mathf.Min(rocketMAG - _rocketMAG, _rocketMAX);
                            _rocketMAG += amountToReload;
                            _rocketMAX -= amountToReload;
                        }
                        else
                        {
                            Debug.Log("SEM BALA");
                        }
                        ammoTXT.text = (_rocketMAG.ToString() + "/" + _rocketMAX.ToString());
                    }
                    break;

                case 5:
                    if (_doideraMAG < doideraMAG)
                    {
                        if (_doideraMAX > 0)
                        {
                            int amountToReload = Mathf.Min(doideraMAG - _doideraMAG, _doideraMAX);
                            _doideraMAG += amountToReload;
                            _doideraMAX -= amountToReload;
                        }
                        else
                        {
                            Debug.Log("SEM BALA");
                        }
                        ammoTXT.text = (_doideraMAG.ToString() + "/" + _doideraMAX.ToString());
                    }
                    break;
            }
        }
        Reloading = false;
        Reloading = false;

        if (Fired)
        {
            switch (SelectedWeapon)
            {
                case 0:
                    writeToScreen = "MELEE";
                    if (_tiroCL <= 0)
                    {
                        atirar(meleeDMG);
                        _tiroCL = meleeCL;
                    }
                    break;
                case 1:
                    writeToScreen = "PISTOLA";
                    ammoTXT.text = (_pistolMAG.ToString() + "/" + _pistolMAX.ToString());
                    if (_tiroCL <= 0)
                    {
                        if (_pistolMAG > 0)
                        {
                            atirar(pistolDMG);
                            _tiroCL = pistolaCL;
                            _pistolMAG--;
                        }
                        else
                        {
                            Reloading = true;
                        }
                    }
                    break;

                case 2:
                    writeToScreen = "RIFLE";
                    ammoTXT.text = (_rifleMAG.ToString() + "/" + _rifleMAX.ToString());
                    if (_tiroCL <= 0)
                    {
                        if(_rifleMAG > 0)
                        {
                            atirar(rifleDMG);
                            _tiroCL = rifleCL;
                            _rifleMAG--;
                        }
                        else
                        {
                            Reloading = true;
                        }
                    }
                    break;
                case 3:
                    writeToScreen = "GRANDA";
                    if (_tiroCL <= 0)
                    {
                        //atirar(DMG);
                        //_tiroCL = grandaCL;
                        //TODO:SPAWNAR PREFAB GRANADA
                    }
                    break;

                case 4:
                    writeToScreen = "ROCKET";
                    ammoTXT.text = (_rocketMAG.ToString() + "/" + _rocketMAX.ToString());
                    if (_tiroCL <= 0)
                    {
                        atirar(rocketDMG);
                        _tiroCL = rocketCL;
                    }
                    else
                    {
                        Reloading = true;
                    }
                    break;
                case 5:
                    writeToScreen = "DOIDERA";
                    ammoTXT.text = (_doideraMAG.ToString() + "/" + _doideraMAX.ToString());
                    if (_tiroCL <= 0)
                    {
                        atirar(doideraDMG);
                        _tiroCL = doideraCL;
                    }
                    else
                    {
                        Reloading = true;
                    }
                    break;
            }
            }
        }
        //else
        //{
        //    Debug.DrawRay(
        //        PlayerCamera.transform.position,
        //        PlayerCamera.transform.TransformDirection(Vector3.forward) * 1000,
        //        Color.white
        //    );
        //}
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
    }


    private void EfeitoNoAmmo()
    {
        //caso queiramos um efeito quando o jogador esta sem munição e tenta atirar, talvez so um som
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

    public void OnWeapon0(InputValue e)
    {
        SelectedWeapon = 0;
    }
    public void OnWeapon1(InputValue e)
    {
        SelectedWeapon = 1;
    }
    public void OnWeapon2(InputValue e)
    {
        SelectedWeapon = 2;
    }
    public void OnWeapon3(InputValue e)
    {
        SelectedWeapon = 3;
    }
    public void OnWeapon4(InputValue e)
    {
        SelectedWeapon = 4;
    }
    public void OnWeapon5(InputValue e)
    {
        SelectedWeapon = 5;
    }

    public void OnReload(InputValue e)
    {
        Reloading = e.isPressed;
        Debug.Log("tentando recaregar");
    }

    //public void OnAttack(InputValue e)
    //{
    //    Fired = e.isPressed; //NÃO TA VOLTANDO A SER FALSE
    //}

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