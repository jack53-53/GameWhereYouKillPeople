using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorScript : MonoBehaviour
{
    public Vector3 DamageLocation;
    public Transform PTransform;
    public Transform DamageImagePivot;
    public RawImage DamageIndicator;
    private float TimerDesligar;
    // Update is called once per frame
    void Update()
    {
        Vector3 Direction = (DamageLocation - PTransform.position).normalized;
        float angle = (Vector3.SignedAngle(Direction, PTransform.forward, Vector3.up));
        DamageImagePivot.transform.localEulerAngles = new Vector3(0,0,angle);
        TimerDesligar -= Time.deltaTime;
        if(TimerDesligar < 0 && DamageIndicator.color.a == 1f)
        {
            //StartCoroutine(DesligarIndicador(Mult, TempoAtivo, EnemyTransform));//TODO: CONSERTAR ESSA PORRA
            //outro problema: quando o indicador é forçado ficar ativo ele nao atualiza a localização do inimigo
        }
    }

    public void IniciarAnimacao(float Mult, float TempoAtivo, Vector3 EnemyTransform)
    {
        TimerDesligar = TempoAtivo;
        if(DamageIndicator.color.a == 0)
        {
            StartCoroutine(AtivarIndicador(Mult, TempoAtivo, EnemyTransform));
        }

    }

    public IEnumerator AtivarIndicador(float Mult, float TempoAtivo, Vector3 EnemyTransform)
    {
        DamageLocation = EnemyTransform;
        float t = 0f;
        Color c = DamageIndicator.color;
        t = 0f;
        while (DamageIndicator.color.a < 1f)
        {
            t += Time.deltaTime * Mult;
            c = DamageIndicator.color;
            c.a = Mathf.Lerp(0, 1f, t);
            DamageIndicator.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(TempoAtivo);
        while (DamageIndicator.color.a > 0f)
        {
            t += Time.deltaTime * Mult;
            c = DamageIndicator.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            DamageIndicator.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(TempoAtivo);

    }
    private IEnumerator DesligarIndicador(float Mult, float TempoAtivo, Vector3 EnemyTransform)
    {
        DamageLocation = EnemyTransform;
        float t = 0f;
        Color c = DamageIndicator.color;
        t = 0f;
        while (DamageIndicator.color.a > 0)
        {
            t += Time.deltaTime * Mult;
            c = DamageIndicator.color;
            c.a = Mathf.Lerp(1f, 0, t);
            DamageIndicator.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(TempoAtivo);

    }

}
