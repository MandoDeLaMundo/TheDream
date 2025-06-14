using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TriggerBox : MonoBehaviour
{
    enum triggertype {root, silent, debuff, geyser}
    [SerializeField] triggertype type;

    [SerializeField] GameObject objectModel;

    [SerializeField] ParticleSystem particleVFX;

    [SerializeField] float oxygenRegen;
    [SerializeField] float rootDuration;
    [SerializeField] float silentDuration;
    [SerializeField] float geyserStrength;

    float oxygenTimer;

    bool geyserPush;
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
                StartCoroutine(SilentPlayer());
                StartCoroutine(RootPlayer());
            }
        }
        if (!proc && type == triggertype.root)
        {
            if (other.CompareTag("Player"))
            {
                proc = true;
                StartCoroutine(RootPlayer());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (type == triggertype.geyser)
        {
            if (particleVFX != null)
            {
                particleVFX.Play();
            }
            if (playerController.instance.Oxygen != playerController.instance.OxygenOrig)
            {
                OxygenRegen();
            }
            if (other.CompareTag("Player"))
            {
                StartCoroutine(PlayerKnockBack(other.transform));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (type == triggertype.debuff || type == triggertype.root || type == triggertype.silent)
        {
            objectModel.SetActive(false);
        }
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
    IEnumerator PlayerKnockBack(Transform playerPosition)
    {
        geyserPush = false;
        Vector3 direction = Vector3.up;
        float move = 0f;
        while (move < geyserStrength)
        {
            float range = (geyserStrength * 3) * Time.deltaTime;
            playerPosition.Translate(direction * range, Space.World);
            move += range;
            yield return null;
        }
    }
    public void OxygenRegen()
    {
        oxygenTimer += Time.deltaTime;
        if (oxygenTimer >= oxygenRegen)
        {
            playerController.instance.Oxygen += 1;
            gameManager.instance.UpdatePlayerOXCount(1);
            playerController.instance.updatePlayerUI();
            oxygenTimer = 0;
        }
    }
}
