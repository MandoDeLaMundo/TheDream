using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 movementOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float cycleDuration = 4f;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private Rigidbody rb;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        startPoint = transform.position;
        endPoint = startPoint + movementOffset;
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        float phase = Mathf.PingPong(timer / cycleDuration, 1f);
        Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, phase);
        rb.MovePosition(newPosition);
    }
}