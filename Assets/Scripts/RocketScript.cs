using System.Collections;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public float DMG;
    public Rigidbody RB;
    public Transform PlayerTransform;
    public float Speed;
    public float TempoDet;
    [Tooltip("Porcentagem de dano que o player nao toma")]
    public int DefesaPlayer;
    public float radius = 5.0f;
    public float power = 10.0f;
    public ForceMode forceMode = ForceMode.Impulse;
    public GameObject ExplosionPrefab;
    public GameObject ExplosionDecal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyScript>() == null)
        {
            StartCoroutine(Espera()); //a logica do missel segue dentro do script do inimigo quando ele é um inimigo
        }
    }

    public void DestroirEEfeito()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator Espera()
    {
        ApplyDamage();
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        yield return 0; //esperemos 1 frame pro ragdoll poder pegar a força
        ApplyExplosionForce();
        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;

        Vector3 origin = transform.position;

        if (Physics.Raycast(origin, transform.forward, out hit, 1f))
        {
            SpawnDecal(hit);
        }

        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.left), out hit2, 1f))
        {
            SpawnDecal(hit2);
        }

        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.right), out hit3, 1f))
        {
            SpawnDecal(hit3);
        }

        if (Physics.Raycast(origin, Vector3.down, out hit4, 1f))
        {
            SpawnDecal(hit4);
        }
        Destroy(this.gameObject);
    }

    public void ApplyExplosionForce()
    {
        Vector3 explosionPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponentInParent<Rigidbody>();

            if (rb != null && !rb.isKinematic)
            {
                rb.AddExplosionForce(power, explosionPosition, radius, 0f, forceMode);
            }
        }
    }

    void SpawnDecal(RaycastHit hit)
    {
        Quaternion rotation = Quaternion.LookRotation(hit.normal);
        GameObject ex = Instantiate(
            ExplosionDecal,
            hit.point + hit.normal * 0.01f,
            rotation
        );
    }

    public void ApplyDamage()
    {
        Vector3 explosionPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        foreach (Collider collider in colliders)
        {

            playerscript p = collider.GetComponentInParent<playerscript>();

            if (p != null)
            {
                float dstc = Vector3.Distance(explosionPosition, p.transform.position);

                p.HP -= (int)Mathf.Round(Mathf.Lerp(DMG, 0f, dstc / radius)) / DefesaPlayer;
            }

            EnemyScript e = collider.GetComponentInParent<EnemyScript>();

            if (e != null)
            {
                float dstc = Vector3.Distance(explosionPosition, e.transform.position);

                e.TakeDamage((int)Mathf.Round(Mathf.Lerp(DMG, 0f, dstc / radius)), this.transform);
            }

            BreakableScript b = collider.GetComponentInParent<BreakableScript>();

            if (b != null)
            {
                float dstc = Vector3.Distance(explosionPosition, b.transform.position);

                b.HP -= (int)Mathf.Round(Mathf.Lerp(DMG, 0f, dstc / radius));
            }
        }
    }
}
