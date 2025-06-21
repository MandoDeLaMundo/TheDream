using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
    public float stretchDuration;
    public float swingDuration;
    public float shrinkDuration;
    public float maxLength;
    public float damageAmount;

    [SerializeField] BoxCollider whipCollider;
    [SerializeField] Transform whipTransform;

    Vector3 initialScale;
    Vector3 targetScale;


    void Start()
    {
        initialScale = whipTransform.localScale;
        targetScale = new Vector3(initialScale.x, initialScale.y, maxLength);

        StartCoroutine(WhipRoutine());
    }

    IEnumerator WhipRoutine()
    {
        yield return AnimateScale(initialScale, targetScale, stretchDuration);

        float swingTimer = 0f;
        float swingAngle = 30f;
        Quaternion startRot = whipTransform.rotation;
        Quaternion endRot = Quaternion.Euler(whipTransform.eulerAngles + new Vector3(0, swingAngle, 0));

        while (swingTimer < swingDuration)
        {
            swingTimer += Time.deltaTime;
            float swingProgress = swingTimer / swingDuration;
            whipTransform.rotation = Quaternion.Slerp(startRot, endRot, swingProgress);
            yield return null;
        }

        yield return AnimateScale(targetScale, initialScale, shrinkDuration);

        Destroy(gameObject);
    }

    IEnumerator AnimateScale(Vector3 from, Vector3 to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            whipTransform.localScale = Vector3.Lerp(from, to, timer / duration);

            float currentZ = whipTransform.localScale.z;
            whipTransform.localPosition = new Vector3(0, 0, currentZ / 2f);

            if (whipCollider)
            {
                whipCollider.size = new Vector3(1, 1, currentZ);
                whipCollider.center = new Vector3(0, 0, currentZ / 2f);
            }

            yield return null;
        }

        whipTransform.localScale = to;
        whipTransform.localPosition = new Vector3(0, 0, to.z / 2f);
        if (whipCollider)
        {
            whipCollider.size = new Vector3(1, 1, to.z);
            whipCollider.center = new Vector3(0, 0, to.z / 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player)
            {
                player.TakeDMG((int)damageAmount);
            }
        }
    }
}
