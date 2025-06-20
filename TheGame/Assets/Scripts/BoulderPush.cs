using UnityEngine;

public class BoulderPush : MonoBehaviour
{
    public float pushStrength = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 pushDir = other.transform.position - transform.position;
                pushDir.y = 0;
                pushDir.Normalize();

                controller.Move(pushDir * pushStrength * Time.deltaTime);
            }
        }
    }
}