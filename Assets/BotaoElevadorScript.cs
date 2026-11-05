using Unity.VisualScripting;
using UnityEngine;

public class BotaoElevadorScript : MonoBehaviour
{
    public Transform PlaceToGo;
    private Transform OriginalPlace;
    private bool Subindo;
    private float t;
    public float Velocidade;
    public float CoolDown = 2f;

    private void Start()
    {
        OriginalPlace = transform;
    }

    private void Update()
    {
        if (Subindo && transform.position != PlaceToGo.position && CoolDown < 0)
        {
            this.transform.position = Vector3.Lerp(OriginalPlace.position, PlaceToGo.position, t);
            t += Time.deltaTime * Velocidade;
            Debug.Log("Subindo");
        }
        else if (!Subindo && OriginalPlace.position != transform.position && CoolDown < 0)
        {
            this.transform.position = Vector3.Lerp(PlaceToGo.position, OriginalPlace.position, t);
            t += Time.deltaTime * Velocidade;
            Debug.Log("Descendo");
        }
        CoolDown -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<playerscript>() != null)
        {
            Subindo = !Subindo;
        }
    }
}
