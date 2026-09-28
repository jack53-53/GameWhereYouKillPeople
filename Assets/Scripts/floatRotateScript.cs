using UnityEngine;

public class floatRotateScript : MonoBehaviour
{
    private Vector3 initialPos;
    public float rotSpeed;
    public float floatSpeed;
    private bool subindo;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void FixedUpdate()
    {
        transform.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
        if (subindo)
        {
            //Mathf.Lerp(2, -2, Time.deltaTime); //ideia boa mas sou muito burro pra fazer funcionar
            transform.position = new Vector3(transform.position.x, transform.position.y + floatSpeed * Time.deltaTime, transform.position.z);
            if (transform.position.y > initialPos.y + 0.1f)
            {
                subindo = false;
            }
        }
        else if(!subindo)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - floatSpeed * Time.deltaTime, transform.position.z);
            if (transform.position.y <  initialPos.y - 0.1f)
            {
                subindo = true;
            }
        }
    }
}
