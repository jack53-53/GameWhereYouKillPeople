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
        ApplyExplosionForce();
        Destroy(gameObject);
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

                e.HP -= (int)Mathf.Round(Mathf.Lerp(DMG, 0f, dstc / radius));
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
