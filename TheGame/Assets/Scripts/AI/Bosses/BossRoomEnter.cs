using UnityEngine;
//using UnityEngine.UI;

public class BossRoomEnter : MonoBehaviour
{
    [SerializeField] public GameObject bossDoor;
    [SerializeField] public GameObject bossHP;
    [SerializeField] public GameObject boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossDoor)
                bossDoor.SetActive(true);

            bossHP.SetActive(true);
            boss.SetActive(true);
        }
    }
}
