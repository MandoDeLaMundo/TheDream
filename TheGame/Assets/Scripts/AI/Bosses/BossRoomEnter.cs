using UnityEngine;
//using UnityEngine.UI;

public class BossRoomEnter : MonoBehaviour
{
    [SerializeField] public GameObject bossDoor;
    [SerializeField] public GameObject bossHP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossDoor)
                bossDoor.SetActive(true);

            bossHP.SetActive(true);
        }
    }
}
