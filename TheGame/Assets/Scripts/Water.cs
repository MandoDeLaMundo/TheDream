using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteraction interactable = other.GetComponent<IInteraction>();
        Debug.Log("enterTrigger" + interactable);
        if (interactable != null)
        {
            gameManager.instance.playerOxygenBar.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        IInteraction interactable = other.GetComponent<IInteraction>();
        if (interactable != null)
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteraction interactable = other.GetComponent<IInteraction>();
        Debug.Log("exitTrigger" + interactable);
        if (interactable != null)
        {
            gameManager.instance.playerOxygenBar.SetActive(true);
        }

    }
}
