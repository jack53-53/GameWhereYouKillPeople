using UnityEngine;

public class HpDropScript : MonoBehaviour
{
    public int healAmmount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerscript p = collision.gameObject.GetComponent<playerscript>();
            p.HP += healAmmount;
            p.StartCoroutine(p.KillScreenEffect(p.KillEffect, p.MultiplierKillEffectSpeed * 0.2f, Color.lightGreen));
            Destroy(gameObject);
        }

    }
}
