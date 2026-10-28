using System.Collections;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerscript : MonoBehaviour
{

    public int HP;
    public float Speed = 5f;
    public int DMG;
    public RawImage KillEffect;
    public float MultiplierKillEffectSpeed;
    public float SkullEffectSpeed;
    public float ArrowEffectSpeed;

    public Rigidbody RB;
    public Camera PlayerCamera;
    public int SelectedWeapon; //0 = sem arma, 1 = pistola, 2 = rifle, 3 = granada, 4 = bazooka, 5 = BFG //talvez trocar a granada por uma shotgun?
    public string[] palavrasDebug;
    public float palavrasDebugCL;
    private float _palavrasDebugCL;
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
    public float bobSpeed = 5f;
    public float bobAmmount = 1f;

    // public float localXCameraBreath;
    // public float localYCameraBreath;
    // private float _localXCameraBreath;
    // private float _localYCameraBreath;
    // private bool indoPraEsquerda;
    public int linhasDePalavrasNaTelaDurantePoder;

    private float cameraPitch;
    [Tooltip("quem o jogador CONSEGUE dar dano")]
    public LayerMask layerMask;

    private bool Interagiu;
    private bool Pulou;
    private bool Sprayou;
    [Tooltip("tempo que demora pra cada 1 de vida descer quando vc tem demais")]
    public float overHealTime;
    public GameObject granadaPrefab;
    public GameObject RocketPrefab;
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
    [Header("Tempo pra cada arma sair do bolso")]
    public float PistolDeployTime;
    public float RifleDeployTime;
    public float GranadaDeployTime;
    public float RocketDeployTime;
    public float DoideraDeployTime;
    public float MeleeDeployTime;

    [Header("tempo de recarga de cada arma")]
    public float pistolReload;
    public float rifleReload;
    public float granadaReload;
    public float rocketReload;
    public float doideraReload;
    [Header("Modelo do viewmodel de cada arma")]
    public GameObject MeleeModel;
    public GameObject PistolModel;
    public GameObject RifleModel;
    public GameObject GranadaModel;
    public GameObject RocketModel;
    public GameObject DoideraModel;
    public Transform placeToSpawnStuff;
    [Header("Local da onde o muzzle vai aparecer")]
    public Transform PistolMuzzle;
    public Transform RifleMuzzle;
    public Transform RocketMuzzle;
    public Transform DoideraMuzzle;
    [Header("SE TIVER DIFERENTES PNGS PRA ARMA TEM QUE FICAR AQ")]
    public GameObject MuzzlePrefab;
    public RawImage Mira;
    public TextMeshProUGUI debugTXT;
    public TextMeshProUGUI vidaTXT;
    public TextMeshProUGUI ammoTXT;
    public TextMeshProUGUI _fpsText;
    public RawImage SKULL;
    private bool hasPlayedSkull = false;
    private int _lastHP = -1;
    [SerializeField]
    private ParticleSystem BulletImpact;
    [SerializeField]
    private TrailRenderer BulletTrailRender;
    private float updateInterval = 1.0f;
    private float _currentFPS;
    private string writeToScreen;
    private bool lifeIsGoingDown;
    private bool canFire = true;
    private bool CameraIsRecovering;
    public float TempoParaDamageIndicatorDesaparecer;
    public DamageIndicatorScript DMGSetaScript;
    public GameObject BloodSplatterEffect;
    private bool IsDeploying;
    private float deployTimer;
    public Color CorKillScreenEffect;
    public float TempoPraVerSeOEfeitoQuebrou;
    private float _TempoPraVerSeOEfeitoQuebrou;
    public bool IsCivillian;
    public bool IsPaused;
    public Slider SliderFX;
    public Slider SliderSens;

    [Header("Sons")]
    public AudioClip WalkingOnWood;
    public AudioClip WalkingOnConcrete;
    public AudioClip WalkingOnMetal;
    public AudioClip AirBorne;
    public AudioClip PistolReadSound;
    public AudioClip PistolShootSound;
    public AudioClip PistolReloadSound;
    public AudioClip PistolEmptySound;
    public AudioClip RifleReadySound;
    public AudioClip RifleShootSound;
    public AudioClip RifleReloadSound;
    public AudioClip RifleEmptySound;
    public AudioClip GranadaThrowSound;
    public AudioClip GranadaReadySound;
    public AudioClip RocketShoot;
    public AudioClip RocketReloadSound;
    public AudioClip RocketEmptySound;
    public AudioClip DrillReadySound;
    public AudioClip DrillShoot;
    public AudioClip DoideraReady;
    public AudioClip DoideraShoot;



    void Start()
    {
        _fpsText.text = "FPS: 0";
        layerMask = LayerMask.GetMask("Default");
        Color color = KillEffect.color;
        color.a = 0f;
        KillEffect.color = color;
        GameManagerScript.instance.Sensitivity = SliderSens.value;
        GameManagerScript.instance.SoundFxVolume = SliderFX.value;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _pistolMAG = pistolMAG;
        _rifleMAG = rifleMAG;
        _granadaMAG = granadaMAG;
        _rocketMAG = rocketMAG;
        _doideraMAG = doideraMAG;
        _TempoPraVerSeOEfeitoQuebrou = TempoPraVerSeOEfeitoQuebrou;
        // _localXCameraBreath = localXCameraBreath;
        // _localYCameraBreath = localYCameraBreath;
    }

    void Update()
    {
        _currentFPS = 1f / Time.deltaTime;
        UpdateFPS();
        if (HP > 100 && !lifeIsGoingDown)
        {
            StartCoroutine(OverhealBGone());
        }
        debugTXT.text = writeToScreen;
        if (Mouse.current != null) //TODO: fazer o mesmo pro comando
        {
            Fired = Mouse.current.leftButton.isPressed; //grande merda, tive que usar o sistema antigo
        }
        if (IsDeploying)
        {
            deployTimer -= Time.deltaTime;

            if (deployTimer <= 0f)
            {
                IsDeploying = false;
            }
        }


        RaycastHit hitou;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hitou, Mathf.Infinity))
        {
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * hitou.distance, Color.yellow);

            if (hitou.collider.gameObject.CompareTag("Enemy"))
            {
                Mira.color = Color.red;
            }
            else
            {
                Mira.color = Color.white;
            }

        }
        if (Keyboard.current != null)
        {
            canFire = !Keyboard.current.cKey.isPressed;
        }
        if (_TempoPraVerSeOEfeitoQuebrou < 0 && KillEffect.color.a > 0)
        {
            Color c = KillEffect.color;
            c.a = 0;
            KillEffect.color = c;
        }
        _TempoPraVerSeOEfeitoQuebrou -= Time.deltaTime;


        if (Speed > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.2f))
            {
                if (hit.collider.TryGetComponent<FloorProperty>(out FloorProperty fp))
                {
                    switch (fp.tipoChao)
                    {
                        case "Metal":
                            Debug.Log("METAL");
                            break;
                        case "Madeira":
                            Debug.Log("Madeira");
                            break;
                    }
                }
                else
                {
                    //Debug.Log("Voando");
                }
            }
        }
        //Debug.Log(canFire);
        // Debug.Log(hasPlayedSkull);
        if (HP < 20 && !hasPlayedSkull)
        {
            // Debug.Log("tentando tocar efeito");
            StartCoroutine(KillScreenEffect(SKULL, SkullEffectSpeed, Color.black));
            hasPlayedSkull = true;
        }
        else if (HP >= 60 && hasPlayedSkull)
        {
            //Debug.Log("Se curou");
            hasPlayedSkull = false;
        }

        float targetRoll = -Dire.x * 7f;

        //coisos da camera
        if (Dire.x > 0)
        {
            targetRoll = -7f;
        }
        else if (Dire.x < 0)
        {
            targetRoll = 7f;
        }
        // else if(Dire.x == 0) //scrap nisso
        // {
        //     //camera sobe pra esquerda, volta pro meio e depois sobre pra direita
        //     if (indoPraEsquerda)
        //     {
        //         //acho que tem que fazer um inumerator, to com muita preguiça
        //         //TODO: fica pra amanha dnv
        //     }
        // }
        cameraPitch -= Look.y * GameManagerScript.instance.Sensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmmount;
        if (IsPaused)
        {
            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            foreach (Transform child in SliderSens.transform)
            {
                child.gameObject.SetActive(true);
            }
            foreach (Transform child in SliderFX.transform)
            {
                child.gameObject.SetActive(true);
            }
            GameManagerScript.instance.Sensitivity = SliderSens.value;
            GameManagerScript.instance.SoundFxVolume = SliderFX.value;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            PlayerCamera.transform.localRotation = Quaternion.Euler(cameraPitch + bob, 0f, cameraRoll);
            foreach (Transform child in SliderSens.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in SliderFX.transform)
            {
                child.gameObject.SetActive(false);
            }
            Time.timeScale = 1f;
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
        if (!IsCivillian)
        {
            if (!canFire)
            {
                int a = 0;
                Color c;
                c = KillEffect.color;
                c.a = 0.5f;
                KillEffect.color = c;
                if (_palavrasDebugCL <= 0)
                {
                    foreach (char character in writeToScreen)
                    {
                        if (character == '\n')
                        {
                            a++;
                        }
                    }
                    writeToScreen += "\n" + palavrasDebug[UnityEngine.Random.Range(0, palavrasDebug.Length)];
                    _palavrasDebugCL = palavrasDebugCL;
                }
                if (a >= linhasDePalavrasNaTelaDurantePoder)
                {
                    writeToScreen = "";
                }
                //TODO:todos os efeitos de quando o jogador ta perdido
            }
            else
            {
                writeToScreen = "";
                Color c;
                c = KillEffect.color;
                if (c.a > 0f)
                {
                    c.a = c.a - 0.01f;
                    KillEffect.color = c;
                }
            }
            _palavrasDebugCL -= Time.deltaTime;

            if (HP != _lastHP)
            {
                vidaTXT.text = HP.ToString();
                _lastHP = HP;
            }

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
                if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, 5f, layerMask))
                {
                    Quaternion rotation = Quaternion.LookRotation(-hit.normal, PlayerCamera.transform.up);

                    GameObject spray = Instantiate(SprayPrefab, hit.point + hit.normal * 0.05f, rotation);
                    Destroy(spray, 60f);
                }
                Sprayou = false;
            }
            switch (SelectedWeapon)
            {
                case 0:
                    MeleeModel.SetActive(true); //MY GAME IS GOOD THE ALGORITHM JUST IGNORES ME
                    PistolModel.SetActive(false); //I SWEAR IM NOT YANDERE DEV
                    RifleModel.SetActive(false);
                    GranadaModel.SetActive(false);
                    RocketModel.SetActive(false);
                    DoideraModel.SetActive(false);
                    ammoTXT.text = "";
                    break;
                case 1:
                    //writeToScreen = "PISTOLA";
                    MeleeModel.SetActive(false); //MY GAME IS GOOD THE ALGORITHM JUST IGNORES ME
                    PistolModel.SetActive(true); //I SWEAR IM NOT YANDERE DEV
                    RifleModel.SetActive(false);
                    GranadaModel.SetActive(false);
                    RocketModel.SetActive(false);
                    DoideraModel.SetActive(false);
                    ammoTXT.text = (_pistolMAG.ToString() + "/" + _pistolMAX.ToString());
                    break;
                case 2:
                    MeleeModel.SetActive(false); //MY GAME IS GOOD THE ALGORITHM JUST IGNORES ME
                    PistolModel.SetActive(false); //I SWEAR IM NOT YANDERE DEV
                    RifleModel.SetActive(true);
                    GranadaModel.SetActive(false);
                    RocketModel.SetActive(false);
                    DoideraModel.SetActive(false);
                    //writeToScreen = "RIFLE";
                    ammoTXT.text = (_rifleMAG.ToString() + "/" + _rifleMAX.ToString());
                    break;
                case 3:
                    MeleeModel.SetActive(false); //MY GAME IS GOOD THE ALGORITHM JUST IGNORES ME
                    PistolModel.SetActive(false); //I SWEAR IM NOT YANDERE DEV
                    RifleModel.SetActive(false);
                    GranadaModel.SetActive(true);
                    RocketModel.SetActive(false);
                    DoideraModel.SetActive(false);
                    //writeToScreen = "GRANADA";
                    ammoTXT.text = (_granadaMAG.ToString() + "/" + _granadaMAX.ToString());
                    break;
                case 4:
                    MeleeModel.SetActive(false); //MY GAME IS GOOD THE ALGORITHM JUST IGNORES ME
                    PistolModel.SetActive(false); //I SWEAR IM NOT YANDERE DEV
                    RifleModel.SetActive(false);
                    GranadaModel.SetActive(false);
                    RocketModel.SetActive(true);
                    DoideraModel.SetActive(false);
                    //writeToScreen = "ROCKET";
                    ammoTXT.text = (_rocketMAG.ToString() + "/" + _rocketMAX.ToString());
                    break;
                case 5:
                    MeleeModel.SetActive(false); //MY GAME IS GOOD THE ALGORITHM JUST IGNORES ME
                    PistolModel.SetActive(false); //I SWEAR IM NOT YANDERE DEV
                    RifleModel.SetActive(false);
                    GranadaModel.SetActive(false);
                    RocketModel.SetActive(false);
                    DoideraModel.SetActive(true);
                    //writeToScreen = "DOIDERA";
                    ammoTXT.text = (_doideraMAG.ToString() + "/" + _doideraMAX.ToString());
                    break;
            }

        }

    }

    void FixedUpdate()
    {
        _tiroCL -= Time.fixedDeltaTime;
        Vector3 move = transform.right * Dire.x + transform.forward * Dire.y;

        Vector3 velocity = RB.linearVelocity;

        RB.linearVelocity = new Vector3(move.x * Speed, velocity.y, move.z * Speed);

        //Debug.Log("cooldown tiro:" + _tiroCL );
        //Debug.Log("atirando?:" + Fired.ToString() );

        transform.Rotate(Vector3.up * Look.x * GameManagerScript.instance.Sensitivity);


        if (Fired && !Reloading && canFire && !IsDeploying && !IsCivillian)
        {
            switch (SelectedWeapon)
            {
                case 0:
                    //writeToScreen = "MELEE";
                    if (_tiroCL <= 0)
                    {
                        atirar(meleeDMG, 0);
                        SoundFxManager.instance.PlaySoundClip(DrillShoot, this.transform, GameManagerScript.instance.SoundFxVolume, 1, 2);
                        _tiroCL = meleeCL;
                    }
                    break;
                case 1:
                    if (_tiroCL <= 0)
                    {
                        if (_pistolMAG > 0)
                        {
                            atirar(pistolDMG, 2);
                            SoundFxManager.instance.PlaySoundClip(PistolShootSound, this.transform, GameManagerScript.instance.SoundFxVolume, 1, 2);
                            Quaternion rot = PistolMuzzle.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 180f), 0f);
                            GameObject muzzle = Instantiate(MuzzlePrefab, PistolMuzzle.position, rot, PistolMuzzle);
                            muzzle.layer = LayerMask.NameToLayer("Viewmodel"); ;
                            Destroy(muzzle, rifleCL);
                            _tiroCL = pistolaCL;
                            _pistolMAG--;
                        }
                        else
                        {
                            EfeitoNoAmmo();
                            TryReload();
                        }
                    }
                    break;

                case 2:
                    if (_tiroCL <= 0)
                    {
                        if (_rifleMAG > 0)
                        {
                            atirar(rifleDMG, 2);
                            SoundFxManager.instance.PlaySoundClip(RifleShootSound, this.transform, GameManagerScript.instance.SoundFxVolume, 1, 2);
                            Quaternion rot = RifleMuzzle.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 180f), 0f);
                            GameObject muzzle = Instantiate(MuzzlePrefab, RifleMuzzle.position, rot, RifleMuzzle);
                            muzzle.layer = LayerMask.NameToLayer("Viewmodel");
                            Destroy(muzzle, rifleCL);
                            _tiroCL = rifleCL;
                            _rifleMAG--;
                        }
                        else
                        {
                            EfeitoNoAmmo();
                            TryReload();
                        }
                    }
                    break;
                case 3:
                    if (_tiroCL <= 0)
                    {
                        if (_granadaMAG > 0)
                        {
                            GameObject GN = Instantiate(granadaPrefab, placeToSpawnStuff.position, Quaternion.identity);
                            SoundFxManager.instance.PlaySoundClip(GranadaThrowSound, this.transform, GameManagerScript.instance.SoundFxVolume, 1, 2);
                            Rigidbody riggiby = GN.GetComponent<Rigidbody>();
                            riggiby.angularVelocity = PlayerCamera.transform.forward * 20 + PlayerCamera.transform.up * 5;
                            _tiroCL = grandaCL;
                            _granadaMAG--;
                        }
                        else
                        {
                            TryReload();
                        }
                    }
                    break;
                case 4:
                    if (_tiroCL <= 0)
                    {
                        if (_rocketMAG > 0)
                        {
                            Instantiate(RocketPrefab, placeToSpawnStuff.position, PlayerCamera.transform.rotation);
                            SoundFxManager.instance.PlaySoundClip(RocketShoot, this.transform, GameManagerScript.instance.SoundFxVolume, 1f, 2f);
                            _tiroCL = rocketCL;
                            _rocketMAG--;
                        }
                        else
                        {
                            EfeitoNoAmmo();
                            TryReload();
                        }
                    }
                    break;
                case 5:
                    if (_tiroCL <= 0)
                    {
                        if (_doideraMAG > 0)
                        {
                            atirar(doideraDMG, 5);
                            SoundFxManager.instance.PlaySoundClip(DoideraShoot, this.transform, GameManagerScript.instance.SoundFxVolume, 1, 2);
                            _tiroCL = doideraCL;
                            _doideraMAG--;

                        }
                        else
                        {
                            EfeitoNoAmmo();
                            TryReload();
                        }
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
    private void atirar(int DMG, int TipoArma)
    {
        StartCoroutine(DamageCamera(0, 2f));
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (TipoArma == 1)
            {
                TrailRenderer trail = Instantiate(BulletTrailRender, PistolMuzzle.transform.position, Quaternion.identity);
                StartCoroutine(Spawntrail(trail, hit));
            }
            else if (TipoArma == 2)
            {
                TrailRenderer trail = Instantiate(BulletTrailRender, RifleMuzzle.transform.position, Quaternion.identity);
                StartCoroutine(Spawntrail(trail, hit));
            }


            if (!hit.collider.gameObject.CompareTag("Enemy"))
            {
                GameObject bulletHole = Instantiate(BulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(-hit.normal));//TOD1:atirar uma vez, esperar o cooldown de cada arma e atirar dnv 
                Destroy(bulletHole, 10f);
            }
            else
            {
                GameObject BloodSplatter = Instantiate(BloodSplatterEffect, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(-hit.normal));
                Destroy(BloodSplatter, 0.3f);
            }
            //Debug.Log("acertei um gameobject");
            if (hit.collider.TryGetComponent<EnemyScript>(out EnemyScript enemy))
            {
                if (enemy.TakeDamage(DMG, this.transform))
                {
                    StartCoroutine(KillScreenEffect(KillEffect, MultiplierKillEffectSpeed, CorKillScreenEffect));
                }
            }
            else if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.angularVelocity += PlayerCamera.transform.forward * 40f;
            }
            if (hit.collider.TryGetComponent<barrelScript>(out barrelScript Barril))
            {
                Barril.Explode();
            }
        }
        else
        {
            Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }

    void TryReload()
    {
        if (Reloading) return;
        Reloading = true;
        StartCoroutine(Reload());
    }

    private void EfeitoNoAmmo()
    {
        SoundFxManager.instance.PlaySoundClip(RifleEmptySound, this.transform, GameManagerScript.instance.SoundFxVolume, 1f, 2f);
        //caso queiramos um efeito quando o jogador esta sem munição e tenta atirar, talvez so um som
    }

    IEnumerator Spawntrail(TrailRenderer Trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 StartPos = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(StartPos, hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = hit.point;
        if (hit.collider != null)
        {
            if (!hit.collider.gameObject.CompareTag("Enemy"))
            {
                Instantiate(BulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        Destroy(Trail.gameObject, Trail.time);
    }

    IEnumerator Reload()
    {
        switch (SelectedWeapon)
        {
            case 0:
                //inspecionar?
                break;

            case 1:
                if (_pistolMAG < pistolMAG)
                {
                    yield return new WaitForSeconds(pistolReload);
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
                    yield return new WaitForSeconds(rifleReload);
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
                    yield return new WaitForSeconds(granadaReload);
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
                    yield return new WaitForSeconds(rocketReload);
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
                    yield return new WaitForSeconds(doideraReload);
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
        Reloading = false;
    }

    private void UpdateFPS()
    {
        _fpsText.text = "Curr FPS: " + Mathf.RoundToInt(_currentFPS);
    }
    public IEnumerator KillScreenEffect(RawImage r, float Mult, Color Cor)
    {
        _TempoPraVerSeOEfeitoQuebrou = TempoPraVerSeOEfeitoQuebrou;
        //Debug.Log("EFEITO");
        float t = 0f;
        Color c = Cor;

        //while (KillEffect.color.a < 0.7f)
        //{
        //    t += Time.deltaTime * MultiplierKillEffectSpeed;
        //    c = KillEffect.color;
        //    c.a = Mathf.Lerp(0, 0.7f, t);
        //    KillEffect.color = c;
        //    yield return null;
        //}

        //t = 0f;
        //while (KillEffect.color.a > 0f)
        //{
        //    t += Time.deltaTime * MultiplierKillEffectSpeed;
        //    c = KillEffect.color;
        //    c.a = Mathf.Lerp(0.7f, 0f, t);
        //    KillEffect.color = c;
        //    yield return null;
        //}

        t = 0f;
        while (r.color.a < 0.3f)
        {
            t += Time.deltaTime * Mult;
            c = Cor;
            c.a = Mathf.Lerp(0, 0.4f, t);
            r.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

        t = 0f;
        while (r.color.a > 0f) //quebrei alguma coisa aqui TOD1: FIX ASAP //consertado
        {
            t += Time.deltaTime * Mult;
            c = Cor;
            c.a = Mathf.Lerp(0.3f, 0f, t);
            r.color = c;
            yield return null;
        }
    }

    public void takeDamage(int DMG, Vector3 EnemyLocation)
    {
        HP -= DMG;
        DMGSetaScript.IniciarAnimacao(ArrowEffectSpeed, TempoParaDamageIndicatorDesaparecer, EnemyLocation);
        StartCoroutine(DamageCamera(-3f, 5f));
    }

    IEnumerator OverhealBGone()
    {
        lifeIsGoingDown = true;
        while (HP > 100)
        {
            HP -= 1;
            yield return new WaitForSeconds(overHealTime);
        }
        lifeIsGoingDown = false;
        yield return null;
    }

    IEnumerator DamageCamera(float AmmountDown, float AmmountUp)
    {
        float targetRoll = UnityEngine.Random.Range(cameraPitch - AmmountDown, cameraPitch - AmmountUp);
        float duration = 0.01f;
        float timer = 0f;
        float _cameraPitch = cameraPitch;


        float startingRoll = cameraPitch;

        if (!CameraIsRecovering)
        {
            while (timer < duration)
            {
                timer += Time.deltaTime;

                cameraPitch = Mathf.Lerp(startingRoll, targetRoll, timer / duration);

                yield return null;
            }

            cameraPitch = targetRoll;
            StartCoroutine(CameraRecover(_cameraPitch));
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator CameraRecover(float X) //pra camera fazer o recover, eu to salvando o valor original antes de mudar, e dai eu to fazendo a mesma função dnv so que sem o recover
    {
        float targetRoll = X;
        float duration = 0.1f;
        float timer = 0f;

        float startingRoll = cameraPitch;
        CameraIsRecovering = true;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraPitch = Mathf.Lerp(startingRoll, targetRoll, timer / duration);

            yield return null;
        }

        cameraPitch = targetRoll;

        CameraIsRecovering = false;
    }
    private float GetDeployTime(int weapon)
    {
        switch (weapon)
        {
            case 0: return MeleeDeployTime;
            case 1: return PistolDeployTime;
            case 2: return RifleDeployTime;
            case 3: return GranadaDeployTime;
            case 4: return RocketDeployTime;
            case 5: return DoideraDeployTime;

            default: return 0f;
        }
    }
    private void SelectWeapon(int weapon)
    {
        if (SelectedWeapon == weapon)
            return;

        SoundFxManager.instance.PlaySoundClip(RifleReadySound, this.transform, GameManagerScript.instance.SoundFxVolume, 1f, 2f);

        SelectedWeapon = weapon;

        deployTimer = GetDeployTime(weapon);
        IsDeploying = deployTimer > 0f;
    }

    public void OnWeapon0(InputValue e)
    {
        SelectWeapon(0);
    }
    public void OnWeapon1(InputValue e)
    {
        SelectWeapon(1);
    }
    public void OnWeapon2(InputValue e)
    {
        SelectWeapon(2);
    }
    public void OnWeapon3(InputValue e)
    {
        SelectWeapon(3);
    }
    public void OnWeapon4(InputValue e)
    {
        SelectWeapon(4);
    }
    public void OnWeapon5(InputValue e)
    {
        SelectWeapon(5);
    }

    public void OnReload(InputValue e)
    {
        if (e.isPressed)
        {
            TryReload();
        }
        //Debug.Log("tentando recaregar");
    }

    //public void OnAttack(InputValue e)
    //{
    //    Fired = e.isPressed; //NÃO TA VOLTANDO A SER FALSE
    //}
    //public void OnCuest(InputValue e)
    //{
    //    canFire = e.isPressed;
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

    public void OnPause(InputValue e)
    {
        IsPaused = !IsPaused;
    }
}