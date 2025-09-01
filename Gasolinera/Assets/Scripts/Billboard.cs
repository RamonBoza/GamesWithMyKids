using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;
    void Start() { cam = Camera.main; }

    void LateUpdate()
    {
        if (!cam) return;
        // Mira hacia donde mira la cámara (tipo "billboard")
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
    }
}