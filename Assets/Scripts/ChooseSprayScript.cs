using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChooseSprayScript : MonoBehaviour
{
    public Material[] Sprays;
    public DecalProjector MR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MR.material = Sprays[Random.Range(0, Sprays.Length)];
    }

}
