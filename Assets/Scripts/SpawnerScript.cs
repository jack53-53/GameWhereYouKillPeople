using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public float TempoRespawn;
    private float _TempoRespawn;
    public GameObject PrefabASerSPawnada;
    public int QuantidadeMunicao;
    private void Start()
    {
        _TempoRespawn = TempoRespawn;
    }
    private void Update()
    {
        if(gameObject.transform.childCount == 0 && _TempoRespawn <= 0)
        {
            GameObject GO = Instantiate(PrefabASerSPawnada, this.transform.position,Quaternion.identity, this.gameObject.transform);
            if (GO.GetComponent<DropScript>() != null)
            {
                DropScript DS = GO.GetComponent<DropScript>();
                DS.municao = QuantidadeMunicao;
            }
            _TempoRespawn = TempoRespawn;
        }
        _TempoRespawn -= Time.deltaTime;
    }
}
