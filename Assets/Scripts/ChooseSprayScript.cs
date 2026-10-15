using UnityEngine;

public class ChooseSprayScript : MonoBehaviour
{
    public Material[] Sprays;
    public MeshRenderer MR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MR.material = Sprays[Random.Range(0, Sprays.Length)];
    }

}
