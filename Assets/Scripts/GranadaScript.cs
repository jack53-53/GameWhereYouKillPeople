using Unity.VisualScripting;
using UnityEngine;

public class GranadaScript : MonoBehaviour
{
    public float DMG;
    public Rigidbody RB;
    public float TempoDet;
    [Header("Explosion Settings")]
    public float radius = 5.0f;
    public float power = 10.0f;
    public float upwardsModifier = 3.0f;
    public ForceMode forceMode = ForceMode.Force;

    [Header("Optional Settings")]
    public LayerMask affectedLayers = ~0; // All layers by default

    void Update()
    {
        ApplyExplosionForce();
    }

    public void ApplyExplosionForce()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius, affectedLayers);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upwardsModifier, forceMode);
            }
            if(collider.TryGetComponent<playerscript>(out playerscript p)){
                float dstc = Vector3.Distance(collider.gameObject.transform.position, rb.position);
                p.HP -= Mathf.Round(Mathf.Lerp(DMG, 0f, dstc));
            }
            if(collider.TryGetComponent<EnemyScript>(out EnemyScript e))
            {
                e.HP -= DMG;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
