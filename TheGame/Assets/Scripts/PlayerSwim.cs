using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerSwim : MonoBehaviour
{
    [SerializeField] LayerMask waterLayer;
    [SerializeField] float swimSpeed;
    [SerializeField] float verticalSwimSpeed;
    [SerializeField] float oxygenDrainRate;

    bool isInWater = false;
    playerController player;

    float oxygenDrainTimer = 0f;

    void Start()
    {
        player = GetComponent<playerController>();
    }

    void Update()
    {
        if (isInWater)
        {
            Swim();
            DrainOxygen();
        }
    }

    void Swim()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Vertical movement (Space and Ctrl)
        float y = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            y += 1f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            y -= 1f;
        }

        Vector3 swimVelocity = moveDir * swimSpeed + Vector3.up * y * verticalSwimSpeed;

        player.controller.Move(swimVelocity * Time.deltaTime);
    }

    void DrainOxygen()
    {
        oxygenDrainTimer += Time.deltaTime;
        if (oxygenDrainTimer >= 1f)
        {
            player.Oxygen -= (int)oxygenDrainRate;
            gameManager.instance.UpdatePlayerOXCount(-(int)oxygenDrainRate);
            player.updatePlayerUI();

            oxygenDrainTimer = 0f;

            if (player.Oxygen <= 0)
            {
                player.TakeDMG(5); // Optional drowning damage
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0)
        {
            isInWater = true;
            Debug.Log("Entered Water");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & waterLayer) != 0)
        {
            isInWater = false;
            Debug.Log("Exited Water");
        }
    }
}
