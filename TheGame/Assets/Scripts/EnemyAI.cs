using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour, IDamage
{
	[SerializeField] Renderer model;
	[SerializeField] NavMeshAgent agent;

	[SerializeField] int HP;
	[SerializeField] int faceTargetSpeed;

	[SerializeField] Transform shootPos;
	[SerializeField] GameObject projectile;
	[SerializeField] float shootRate;
	[SerializeField] int rangeDmgAmount;

	[SerializeField] float meleeRate;
	[SerializeField] float meleeDistance;
	[SerializeField] int meleeDmgAmount;

	Color colorOrig;

	Vector3 playerDir;

	float shootTimer;
	float meleeTimer;

	bool playerInRange;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		colorOrig = model.material.color;
		gameManager.instance.UpdateGameGoal(1);
	}

	// Update is called once per frame
	void Update()
	{
		if (playerInRange)
		{
			meleeTimer += Time.deltaTime;
			shootTimer += Time.deltaTime;

			playerDir = (gameManager.instance.player.transform.position - transform.position);

			agent.SetDestination(gameManager.instance.player.transform.position);


			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				faceTarget();
			}

			if (playerDir.magnitude <= meleeDistance && meleeTimer >= meleeRate)
			{
				attackPlayer();
			}

			if (playerDir.magnitude >  meleeDistance && shootTimer >= shootRate)
			{
				shootPlayer();
			}
		}


	}
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collision detected with: " + collision.collider.name);
		if (collision.collider.CompareTag("Player"))
		{
			IDamage dmg = collision.collider.GetComponent<IDamage>();
			if (dmg != null)
			{
				dmg.TakeDMG(rangeDmgAmount);
			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = false;
		}
	}

	public void TakeDMG(int amount)
	{
		HP -= amount;

		agent.SetDestination(gameManager.instance.player.transform.position);

		if (HP <= 0)
		{
			gameManager.instance.UpdateGameGoal(-1);
			Destroy(gameObject);
		}
		else
		{
			StartCoroutine(flashRed());
		}
	}

	IEnumerator flashRed()
	{
		model.material.color = Color.red;
		yield return new WaitForSeconds(0.05f);
		model.material.color = colorOrig;
	}

	void faceTarget()
	{
		Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
		transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
	}

	private void attackPlayer()
	{
		meleeTimer = 0;
		gameManager.instance.player.GetComponent<playerController>().TakeDMG(meleeDmgAmount);
	}

	private void shootPlayer()
	{
		shootTimer = 0;
		Instantiate(projectile, shootPos.position, transform.rotation);
	}
}
