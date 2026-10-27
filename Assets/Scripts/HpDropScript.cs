using UnityEngine;

public class HpDropScript : MonoBehaviour
{
    public int healAmmount;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerscript p = other.gameObject.GetComponent<playerscript>();
            p.HP += healAmmount; 
            p.StartCoroutine(p.KillScreenEffect(p.KillEffect, p.MultiplierKillEffectSpeed * 0.2f, Color.lightGreen));
            Destroy(gameObject);
        }
    }
}
