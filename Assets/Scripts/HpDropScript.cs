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
            Destroy(gameObject);
        }
    }
}
