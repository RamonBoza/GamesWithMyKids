using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float bounds = 9f; // límites del suelo (Ground ~20m -> +-9 deja margen)

	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public KeyCode actionKey = KeyCode.Space;

    Rigidbody rb;
    float yFixed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yFixed = transform.position.y;
    }

    void Update()
    {
        float h = Input.GetAxisRaw(horizontalAxis); // A/D o flechas
        float v = Input.GetAxisRaw(verticalAxis);   // W/S o flechas
        Vector3 input = new Vector3(h, 0f, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // Mover (kinematic -> Translate está bien)
        Vector3 delta = input * speed * Time.deltaTime;
        transform.Translate(delta, Space.World);

        // Girar hacia la dirección de movimiento
        if (input.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(input, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.2f);
        }

        // Mantener altura y límites del área
        Vector3 p = transform.position;
        p.y = yFixed;
        p.x = Mathf.Clamp(p.x, -bounds, bounds);
        p.z = Mathf.Clamp(p.z, -bounds, bounds);
        transform.position = p;
    }
}