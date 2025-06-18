using UnityEngine;

public class BoulderPush : MonoBehaviour
{
    public float pushStrength = 5f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player"))
        {
            CharacterController controller = hit.collider.GetComponent<CharacterController>();
            if (controller != null)
            {
                // Calculate push direction away from boulder
                Vector3 pushDir = hit.collider.transform.position - transform.position;
                pushDir.y = 0; 
                pushDir.Normalize();

                controller.Move(pushDir * pushStrength * Time.deltaTime);
            }
        }
    }
}