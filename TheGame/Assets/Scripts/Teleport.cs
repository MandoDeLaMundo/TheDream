using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int speed;
    public GameObject player;
    public CharacterController playercon;
    public float destroyDelay = 0.1f;

    void Start()
    {
            rb.linearVelocity = transform.forward * speed;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleportable"))
        {
            playercon.enabled = false;
            Vector3 teleportPosition = transform.position;
            teleportPosition.y += 1;
            player.transform.position = teleportPosition;

            Destroy(gameObject, destroyDelay);
            playercon.enabled = true;
        }
        if(other.CompareTag("NONTeleportable"))
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
