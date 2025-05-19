using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
	enum damagetype { moving, stationary, DOT, homing, contact }
	[SerializeField] damagetype type;
	[SerializeField] Rigidbody rb;

	[SerializeField] int damageAmount;
	[SerializeField] int damageRate;
	[SerializeField] int speed;
	[SerializeField] int destroyTime;
	[SerializeField] int contactDMGAmount;
	[SerializeField] float knockBackDistance;
	[SerializeField] float knockBackSpeed;
	[SerializeField] float knockbackDelay;

    bool isDamaging;
	bool canKnockBack = true;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (type == damagetype.moving || type == damagetype.homing)
		{
			Destroy(gameObject, destroyTime);
			if (type == damagetype.moving)
			{
				rb.linearVelocity = transform.forward * speed;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (type == damagetype.homing)
		{
			rb.linearVelocity = (gameManager.instance.transform.position - transform.position).normalized * speed * Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("Player Trigger");
		if (!canKnockBack)
		{
			return;
		}
        if (other.isTrigger)
		{
			return;
		}

		IDamage dmg = other.GetComponent<IDamage>();

		if (dmg != null && (type == damagetype.moving || type == damagetype.stationary || type == damagetype.homing))
		{
			dmg.TakeDMG(damageAmount);
		}

		if (type == damagetype.moving || type == damagetype.homing)
		{
			Destroy(gameObject);
		}
        if (other.CompareTag("Player") && type == damagetype.contact )
        {
            dmg.TakeDMG(contactDMGAmount);
            Debug.Log("Contact DMG");
            StartCoroutine(PlayerKnockBack(other.transform));
            StartCoroutine(Cooldown());
        }
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		IDamage dmg = other.GetComponent<IDamage>();
		if (dmg != null && type == damagetype.DOT)
		{
			if (!isDamaging)
			{
				StartCoroutine(damageOther(dmg));
			}

		}

	}
	IEnumerator damageOther(IDamage d)
	{
		isDamaging = true;
		d.TakeDMG(damageAmount);
		yield return new WaitForSeconds(damageRate);
		isDamaging = false;

	}
	IEnumerator PlayerKnockBack(Transform playerPosition)
	{
		canKnockBack = false;
		Vector3 direction = (playerPosition.position - transform.position).normalized;
		float move = 0f;
		while (move < knockBackDistance)
		{
			float range = knockBackSpeed * Time.deltaTime;
			playerPosition.Translate(direction * range, Space.World);
			move += range;
			yield return null;
		}
	}
	IEnumerator Cooldown()
	{
		canKnockBack = false;
		yield return new WaitForSeconds (knockbackDelay);
		canKnockBack = true;
	}
}

