using UnityEngine;

public class HpDropScript : MonoBehaviour
{
    public int healAmmount;
    public AudioClip SomHealing;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundFxManager.instance.PlaySoundClip(SomHealing, this.transform, GameManagerScript.instance.SoundFxVolume * 4f, 1f, 2f);
            playerscript p = collision.gameObject.GetComponent<playerscript>();
            p.HP += healAmmount;
            p.StartCoroutine(p.KillScreenEffect(p.KillEffect, p.MultiplierKillEffectSpeed * 0.2f, Color.lightGreen));
            Destroy(gameObject);
        }

    }
}
