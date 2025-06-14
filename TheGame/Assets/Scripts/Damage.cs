using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Damage : MonoBehaviour
{
	enum damagetype { moving, stationary, DOT, homing, contact, AOE}
	[SerializeField] damagetype type;
	[SerializeField] Rigidbody rb;

	[SerializeField] int damageAmount;
	[SerializeField] int damageRate;
	[SerializeField] int speed;
	[SerializeField] float destroyTime;
	[SerializeField] int contactDMGAmount;
	[SerializeField] float knockBackDistance;
	[SerializeField] float knockBackSpeed;
	[SerializeField] float knockbackDelay;
	[SerializeField] GameObject explosionArea;

    bool isDamaging;
	bool canKnockBack = true;
	bool isExploded = false;

	Transform lockEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		if (type == damagetype.moving || type == damagetype.homing || type == damagetype.AOE)
		{
			Destroy(gameObject, destroyTime);
			if (type == damagetype.moving || type == damagetype.AOE || type == damagetype.homing)
			{
				rb.linearVelocity = transform.forward * speed;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
        if (type == damagetype.homing && lockEnemy != null)
        {
            Debug.Log("Come");
            Vector3 direction = (lockEnemy.position - transform.position).normalized;
            Vector3 rotate = Vector3.Cross(transform.forward, direction);
            rb.angularVelocity = rotate * speed * Time.deltaTime;
            rb.linearVelocity = transform.forward * speed;
        }
        else 
        {
			if (type == damagetype.homing)
			{
				Debug.Log("Detection fail");
				rb.linearVelocity = transform.forward * speed;
			}
        }
    }

	private void OnTriggerEnter(Collider other)
	{
		if (!canKnockBack)
		{
			return;
		}
        if (other.isTrigger)
		{
			return;
		}

		IDamage dmg = other.GetComponent<IDamage>();

		if (dmg != null && (type == damagetype.moving || type == damagetype.stationary))
		{
			dmg.TakeDMG(damageAmount);
		}

		if (type == damagetype.moving)
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
		if(type == damagetype.AOE)
		{
            Debug.Log("Hit: " + other.name);
            if (isExploded)
            {
                return;
            }
            Explode();
        }
		if(type == damagetype.homing && other.CompareTag("Enemy"))
		{
			Debug.Log("Homing trigger");
			if (lockEnemy == null)
			{
				lockEnemy = other.transform;
			}
			if (lockEnemy != null)
			{
				if (dmg != null)
				{
					dmg.TakeDMG(damageAmount);
				}
				Destroy(gameObject);
			}
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

    private void OnTriggerExit(Collider other)
	{
		isDamaging = false;
	}

    void Explode()
	{
		Debug.Log("Explosion Trigger");
		isExploded = true;
		speed = 0;
        rb.linearVelocity = transform.forward * speed;
		rb.useGravity = false;
        explosionArea.SetActive(true);
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		Destroy(gameObject, destroyTime);	
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

