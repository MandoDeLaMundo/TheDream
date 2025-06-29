using UnityEngine;


public class BoulderPush : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushStrength = 5f;

    [Header("Movement Settings")]
    public Vector3 rollDirection = Vector3.forward;
    public float rollSpeed = 8f;

    private bool isActive = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void ActivateBoulder()
    {
        if (isActive) return;

        isActive = true;
        rb.isKinematic = false;
        rb. linearVelocity = rollDirection.normalized * rollSpeed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 pushDir = other.transform.position - transform.position;
                pushDir.y = 0;
                controller.Move(pushDir.normalized * pushStrength * Time.deltaTime);
            }
        }
    }
}
