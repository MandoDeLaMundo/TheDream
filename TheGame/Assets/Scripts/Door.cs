using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject doormodel;
    [SerializeField] GameObject button;
    [SerializeField] string text;

    bool playerInTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                doormodel.SetActive(false);
                button.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteraction openable = other.GetComponent<IInteraction>();
        if (openable != null)
        {
            button.SetActive(true);
            playerInTrigger = true;
            //gameManager.instance.textPopUpDescription.text = text;
            //gameManager.instance.textPopUp.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        IInteraction openable = other.GetComponent<IInteraction>();
        if (openable != null)
        {
            button.SetActive(false);
            playerInTrigger = false;
            doormodel.SetActive(true);
            //gameManager.instance.textPopUp.SetActive(false);
        }

    }
}
