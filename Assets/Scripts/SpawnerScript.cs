using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public float TempoRespawn;
    private float _TempoRespawn;
    public GameObject PrefabASerSPawnada;
    private void Start()
    {
        _TempoRespawn = TempoRespawn;
    }
    private void Update()
    {
        if(gameObject.transform.childCount == 0 && _TempoRespawn <= 0)
        {
            Instantiate(PrefabASerSPawnada, this.transform.position,Quaternion.identity, this.gameObject.transform);
            _TempoRespawn = TempoRespawn;
        }
        _TempoRespawn -= Time.deltaTime;
    }
}
