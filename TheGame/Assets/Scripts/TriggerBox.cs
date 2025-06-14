using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TriggerBox : MonoBehaviour
{
    enum triggertype {root, silent, debuff}
    [SerializeField] triggertype type;

    [SerializeField] GameObject objectModel;

    [SerializeField] ParticleSystem particleVFX;

    [SerializeField] float rootDuration;
    [SerializeField] float silentDuration;

    bool proc;
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
        if (!proc && type == triggertype.debuff) 
        {
            if (other.CompareTag("Player"))
            {
                proc = true;
                StartCoroutine(RootPlayer());
                StartCoroutine(SilentPlayer());
            }
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
    IEnumerator RootPlayer()
    {
        yield return new WaitForSeconds(5);
        playerController.instance.controller.enabled = false;
        yield return new WaitForSeconds(rootDuration);
        playerController.instance.controller.enabled = true;
        objectModel.SetActive(false);
    }
    IEnumerator SilentPlayer()
    {
        yield return new WaitForSeconds(5);
        playerController.instance.canShoot = false;
        yield return new WaitForSeconds(silentDuration);
        playerController.instance.canShoot = true;
    }
}
