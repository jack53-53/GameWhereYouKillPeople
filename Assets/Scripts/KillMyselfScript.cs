using UnityEngine;

public class KillMyselfScript : MonoBehaviour
{
    public float TempoAteEuMeMatar;
    private void Update()
    {
        TempoAteEuMeMatar -= Time.deltaTime;
        if(TempoAteEuMeMatar < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
