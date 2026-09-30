using Unity.VisualScripting;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    public int HP;

    private void Update()
    {
        if(HP <= 0)
        {
            //colocar efeito talvez dependendo de qual objeto é
            Destroy(gameObject);
        }
    }
}
