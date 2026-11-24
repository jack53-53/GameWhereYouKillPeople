using UnityEngine;

public class CameraDebugging : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();

        Debug.Log("Camera: " + cam.name);
        Debug.Log("Enabled: " + cam.enabled);
        Debug.Log("Target: " + cam.targetTexture);

        if (cam.targetTexture != null)
        {
            Debug.Log("Created: " + cam.targetTexture.IsCreated());
            Debug.Log("Resolution: " +
                cam.targetTexture.width + "x" +
                cam.targetTexture.height);
        }
    }
}