using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Damage : MonoBehaviour
{
	enum damagetype { moving, stationary, DOT, homing, contact, AOE, bounce }
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
	[SerializeField] int maxBounce;
	[SerializeField] int bounceRange;
	[SerializeField] GameObject explosionArea;

    bool isDamaging;
	bool canKnockBack = true;
	bool isExploded = false;

    int bounceCount;
	HashSet<Transform> bounceEnemies = new HashSet<Transform>();
	Transform curEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		if (type == damagetype.moving || type == damagetype.homing || type == damagetype.AOE || type == damagetype.bounce)
		{
			Destroy(gameObject, destroyTime);
			if (type == damagetype.moving || type == damagetype.AOE || type == damagetype.bounce)
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
		if (type == damagetype.bounce && curEnemy != null)
		{
			Vector3 direct = (curEnemy.position - transform.position).normalized;
			rb.linearVelocity = direct * speed;
			if (Vector3.Distance(transform.position, curEnemy.position) < 0.5f)
			{
				HitTargetCount(curEnemy);
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
		if(type == damagetype.AOE)
		{
            Debug.Log("Hit: " + other.name);
            if (isExploded)
            {
                return;
            }
            Explode();
        }
        if (type == damagetype.bounce && dmg != null && !bounceEnemies.Contains(other.transform))
        {
            HitTargetCount(other.transform);
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
	void HitTargetCount(Transform target)
	{
		IDamage dmg = target.GetComponent<IDamage>();
		if (dmg != null)
		{
			dmg.TakeDMG(damageAmount);
		}
        bounceEnemies.Add(target);
		bounceCount++;
		if (bounceCount < maxBounce)
		{
			FindTarget();
		}
		else
		{
			Destroy(gameObject, 0.5f);
		}
	}

	void FindTarget()
	{
		Collider[] hits = Physics.OverlapSphere(transform.position, bounceRange);
		Transform nearbyEnemy = null;
		float minRange = Mathf.Infinity;
		foreach (Collider box in hits)
		{
			if (box.CompareTag("Enemy") && !bounceEnemies.Contains(box.transform))
			{
				float range = Vector3.Distance(transform.position,box.transform.position);
				if (range < minRange)
				{
					minRange = range;
					nearbyEnemy = box.transform;
				}
			}
		}
		if (nearbyEnemy != null)
		{
			curEnemy = nearbyEnemy;
		}
		else
		{
			Destroy(gameObject, 0.5f);
		}
	}
	void Explode()
	{
		Debug.Log("Explosion Trigger");
		isExploded = true;
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

