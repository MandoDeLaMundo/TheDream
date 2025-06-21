using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TriggerBox : MonoBehaviour
{
    enum triggertype { none, root, silent, debuff, geyser, elementShield }

    [SerializeField] GameObject objectModel;

    [SerializeField] ParticleSystem particleVFX;

    [Header("Types")]
    [SerializeField] triggertype type;

    [Header("Debuff")]
    [SerializeField] float rootDuration;
    [SerializeField] float silentDuration;

    [Header("Geyser")]
    [SerializeField] float geyserStrength;
    [SerializeField] float oxygenRegen;

    [Header("Element Shield")]
    [SerializeField] private Material spellProjectileMaterial;

    [Header("")]
    [SerializeField] float timer;

    float oxygenTimer;

    bool playerInside = false;
    bool proc;
    void Start()
    {
        if (type == triggertype.debuff || type == triggertype.root || type == triggertype.silent)
        {
            StartCoroutine(SelfDestroy());
        }
    }

    void Update()
    {
        if (timer != 0)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInside = true;
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
        if (!proc && type == triggertype.root)
        {
            if (other.CompareTag("Player"))
            {
                proc = true;
                StartCoroutine(RootPlayer());
            }
        }
        if (!proc && type == triggertype.silent)
        {
            if (other.CompareTag("Player"))
            {
                proc = true;
                StartCoroutine(SilentPlayer());
            }
        }
        if (other.CompareTag("Fire") && type == triggertype.elementShield)
        {
            MeshRenderer renderer = other.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material projectileMaterial = renderer.sharedMaterial;
                if (projectileMaterial == spellProjectileMaterial)
                {
                    Destroy(gameObject);
                }
            }
        }
        if (other.CompareTag("Ice") && type == triggertype.elementShield)
        {
            MeshRenderer renderer = other.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material projectileMaterial = renderer.sharedMaterial;
                if (projectileMaterial == spellProjectileMaterial)
                {
                    Destroy(gameObject);
                }
            }
        }
        if (other.CompareTag("Lightning") && type == triggertype.elementShield)
        {
            MeshRenderer renderer = other.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material projectileMaterial = renderer.sharedMaterial;
                if (projectileMaterial == spellProjectileMaterial)
                {
                    Destroy(gameObject);
                }
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
            if (playerController.instance.Oxygen < playerController.instance.OxygenOrig)
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
        playerInside = false;
        if (type == triggertype.debuff || type == triggertype.root || type == triggertype.silent || type== triggertype.none)
        {
            Destroy(gameObject);
        }
        if (particleVFX != null)
        {
            particleVFX.Stop();
        }
    }
    IEnumerator RootPlayer()
    {
        yield return new WaitForSeconds(3);
        playerController.instance.controller.enabled = false;
        yield return new WaitForSeconds(rootDuration);
        playerController.instance.controller.enabled = true;
        yield return null;
        Destroy(gameObject);
    }
    IEnumerator SilentPlayer()
    {
        yield return new WaitForSeconds(3);
        playerController.instance.canShoot = false;
        yield return new WaitForSeconds(silentDuration);
        playerController.instance.canShoot = true;
        if (type == triggertype.silent)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator PlayerKnockBack(Transform playerPosition)
    {
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

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(timer);
        if(timer <= 0 && !playerInside)
        {
        Destroy(gameObject);
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
