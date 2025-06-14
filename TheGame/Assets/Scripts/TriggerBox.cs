using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [SerializeField] GameObject objectModel;
    [SerializeField] ParticleSystem particleVFX;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        objectModel.SetActive(true);
        if (particleVFX != null) 
        {
            particleVFX.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        objectModel.SetActive(false);
        if (particleVFX != null)
        {
            particleVFX.Stop();
        }
    }
}
