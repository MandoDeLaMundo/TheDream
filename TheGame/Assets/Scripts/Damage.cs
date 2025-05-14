using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
	enum damagetype { moving, stationary, DOT, homing }
	[SerializeField] damagetype type;
	[SerializeField] Rigidbody rb;

	[SerializeField] int damageAmount;
	[SerializeField] int damageRate;
	[SerializeField] int speed;
	[SerializeField] int destroyTime;

	bool isDamaging;

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
		if (other.isTrigger)
		{
			return;
		}
		IDamage dmg = other.GetComponent<IDamage>();
		if (dmg != null && type == damagetype.moving || type == damagetype.stationary || type == damagetype.homing)
		{
			dmg.TakeDMG(damageAmount);

		}
		if (type == damagetype.moving || type == damagetype.homing)
		{
			Destroy(gameObject);
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
}

