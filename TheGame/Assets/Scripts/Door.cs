using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject doorModel;
    //[SerializeField] GameObject button;
    [SerializeField] string text;
    //[SerializeField] bool destroyDoorOn;

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
        IInteraction openable = other.GetComponent<IInteraction>();
        if (openable != null)
        {
            //button.SetActive(true);          
            gameManager.instance.DialogueDescription.text = text;
            gameManager.instance.DialogueBox.SetActive(true);
        }
        //&& destroyDoorOn != false (add this below if needed)
        if (other.CompareTag("FireBall"))
        {
            doorModel.SetActive(false);
            //button.SetActive(false);
            gameManager.instance.DialogueBox.SetActive(false);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        IInteraction openable = other.GetComponent<IInteraction>();
        if (openable != null)
        {
            //button.SetActive(false);
            doorModel.SetActive(true);
            gameManager.instance.DialogueBox.SetActive(false);
        }

    }
}
