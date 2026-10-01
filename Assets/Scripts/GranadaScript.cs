using Unity.VisualScripting;
using UnityEngine;

public class GranadaScript : MonoBehaviour
{
    public float DMG;
    public Rigidbody RB;
    public float TempoDet;
    public float radius = 5.0f;
    public float power = 10.0f;
    public float upwardsModifier = 3.0f;
    public ForceMode forceMode = ForceMode.Impulse;

    private void Start()
    {
        RB.AddForce(Vector3.forward * Time.deltaTime * upwardsModifier, ForceMode.Impulse);
        //transform.Translate(Vector3.forward * Time.deltaTime * upwardsModifier, Space.Self);
    }

    void Update()
    {
        if (TempoDet <= 0)
        {
            ApplyExplosionForce();
        //TODO: falta o efeito da explosao
            Destroy(gameObject);
        }
        TempoDet -= Time.deltaTime;
    }

    public void ApplyExplosionForce()
    {
        Vector3 explosionPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition,radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponentInParent<Rigidbody>();

            if (rb != null && !rb.isKinematic)
            {
                rb.AddExplosionForce(power,explosionPosition,radius,upwardsModifier, forceMode);
            }

            playerscript p = collider.GetComponentInParent<playerscript>();

            if (p != null)
            {
                float dstc = Vector3.Distance(explosionPosition, p.transform.position );

                p.HP -= (int)Mathf.Round(Mathf.Lerp(DMG, 0f, dstc / radius));
            }

            EnemyScript e = collider.GetComponentInParent<EnemyScript>();

            if (e != null)
            {
                float dstc = Vector3.Distance(explosionPosition, e.transform.position );

                e.HP -= (int)Mathf.Round( Mathf.Lerp(DMG, 0f, dstc / radius));
            }

            BreakableScript b = collider.GetComponentInParent<BreakableScript>();

            if (b != null)
            {
                float dstc = Vector3.Distance( explosionPosition, b.transform.position);

                b.HP -= (int)Mathf.Round( Mathf.Lerp(DMG, 0f, dstc / radius) );
            }
        }
    }
}
