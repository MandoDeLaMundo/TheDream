using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour, IDamage
{
	[SerializeField] Renderer model;
	[SerializeField] NavMeshAgent agent;
	[SerializeField] Animator anim;
	[SerializeField] Transform headPos;
	[SerializeField] Collider weaponCol;

	[SerializeField] int HP;
	[SerializeField] int faceTargetSpeed;
	[SerializeField] int FOV;
	[SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animTranSpeed;

    [SerializeField] Transform shootPos;
	[SerializeField] GameObject projectile;
	[SerializeField] float shootRate;
	[SerializeField] int rangeDmgAmount;

	[SerializeField] float meleeRate;
	[SerializeField] float meleeDistance;
	[SerializeField] int meleeDmgAmount;

	[SerializeField] bool isSentry;
	Color colorOrig;

	Vector3 playerDir;
    Vector3 startingPos;

    float shootTimer;
	float meleeTimer;
    float angleToPlayer;
    float roamTimer;
    float stoppingDistOrig;

    bool playerInRange;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		colorOrig = model.material.color;
		gameManager.instance.UpdateGameGoal(1);
		startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
		if(isSentry)
		{
			agent.enabled = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		meleeTimer += Time.deltaTime;
		shootTimer += Time.deltaTime;

		if (agent.remainingDistance < 0.01f)
		{
			roamTimer += Time.deltaTime;
		}
		if (playerInRange)
		{
			playerDir = gameManager.instance.player.transform.position - transform.position;

			if (!isSentry)
			{
				agent.SetDestination(gameManager.instance.player.transform.position);

				if (agent.remainingDistance <= agent.stoppingDistance)
				{
					faceTarget();
				}
			}
			else
			{
				faceTarget3D();
			}



			if (playerDir.magnitude <= meleeDistance && meleeTimer >= meleeRate)
			{
				attackPlayer();
			}

			if (playerDir.magnitude > meleeDistance && shootTimer >= shootRate && CanSeePlayer())
			{
				shootPlayer();
			}
		}
		else if (!isSentry)
		{
			
				checkRoam();
		}
	
    }

    void faceTarget3D()
    {
        Vector3 direction = (gameManager.instance.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * faceTargetSpeed);
    }

    void checkRoam()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist + startingPos;

        if (NavMesh.SamplePosition(ranPos, out NavMeshHit hit, roamDist, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

	bool CanSeePlayer()
	{
		Vector3 directionToPlayer = gameManager.instance.player.transform.position - headPos.position;
		Vector3 horizontalDir = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
		angleToPlayer = Vector3.Angle(horizontalDir, transform.forward);

		Debug.DrawRay(headPos.position, horizontalDir, Color.yellow);

		if (angleToPlayer <= FOV)
		{
			if (Physics.Raycast(headPos.position, directionToPlayer, out RaycastHit hit))
			{
				if (hit.collider.CompareTag("Player"))
				{
					agent.stoppingDistance = stoppingDistOrig;
					return true;
				}
			}
		}
        agent.stoppingDistance = 0;
        return false;
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
            agent.stoppingDistance = 0;
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
        if (projectile != null)
		{
            Vector3 dir = (gameManager.instance.player.transform.position - shootPos.position).normalized;
            GameObject proj = Instantiate(projectile, shootPos.position, Quaternion.LookRotation(dir));
        }
	}
}
