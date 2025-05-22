using System.Data.SqlTypes;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class playerController : MonoBehaviour, IDamage
{
	[SerializeField] CharacterController controller;
	[SerializeField] LayerMask ignoreLayer;

	[SerializeField] int HP;
	int HPOrig;

	[SerializeField] int Mana;
	int ManaOrig;

	[SerializeField] int speed;
	[SerializeField] int sprintMod;

	[SerializeField] bool isShooting;
	[SerializeField] int shootDamage;
	[SerializeField] float shootRate;
	[SerializeField] int shootDist;
	float shootTimer;

	[SerializeField] bool isTeleportingRaycast;
	[SerializeField] float teleportRate;
	[SerializeField] int teleportDist;
	[SerializeField] GameObject teleportProj;
	[SerializeField] bool isTeleportingProj;

	GameObject currentTeleProj;

	[SerializeField] int jumpMax;
	[SerializeField] int jumpForce;
	[SerializeField] int Gravity;
	int jumpCount;
	Vector3 playerVel;

	[SerializeField] Transform shootPos;
	[SerializeField] bool isFireball;
	[SerializeField] GameObject fireBall;
	[SerializeField] bool isIce;
	[SerializeField] GameObject Ice;
	[SerializeField] bool isLightning;
	[SerializeField] GameObject Lightning;
	

	Vector3 moveDir;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		HPOrig = HP;
		ManaOrig = Mana;
		updatePlayerUI();
    }

	// Update is called once per frame
	void Update()
	{
		if (isShooting)
		{
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
		}
		if (isTeleportingRaycast)
		{
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * teleportDist, Color.blue);
		}

		if (controller.transform.position.y < 0)
			TakeDMG(100);

		Movement();
		sprint();
	}

	void Movement()
	{
		shootTimer += Time.deltaTime;

		if (controller.isGrounded)
		{
			jumpCount = 0;
			playerVel = Vector3.zero;
		}

		moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

		controller.Move(moveDir * speed * Time.deltaTime);

		jump();

		controller.Move(playerVel * Time.deltaTime);
		playerVel.y -= Gravity * Time.deltaTime;
		if (Input.GetButton("Fire1") && shootTimer >= shootRate)
		{
			if (isShooting)
				shoot();
			if (isFireball)
				shootFireball();
			//if(isIce)
			//shootIce();
			//if(isLightning)
			//shootLightning();
		}
		if (Input.GetButton("Fire2") && shootTimer >= teleportRate)
		{
			if(isTeleportingRaycast)
			teleportbyclick();
			if (isTeleportingProj)
				teleportproj();
		}
	}

	void jump()
	{
		if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
		{
			jumpCount++;
			playerVel.y = jumpForce;
		}
	}

	void sprint()
	{
		if (Input.GetButtonDown("Sprint"))
		{
			speed += sprintMod;
		}
		if (Input.GetButtonUp("Sprint"))
		{
			speed -= sprintMod;
		}
	}

	void shoot()
	{
		shootTimer = 0;

		RaycastHit hit;

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
		{
			Debug.Log(hit.collider.name);
			IDamage dmg = hit.collider.GetComponent<IDamage>();

			if (dmg != null)
			{
				dmg.TakeDMG(shootDamage);
			}
		}
	}

	void teleportbyclick()
	{
		if (currentTeleProj != null) return;
		shootTimer = 0;

		GameObject teleProj = Instantiate(teleportProj, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
        Rigidbody rb = teleProj.GetComponent<Rigidbody>();

		if (rb != null)
		{
			rb.linearVelocity = Camera.main.transform.forward * 20f;
		}
		currentTeleProj = teleProj;
	
	}

    void teleportproj() { }

	void shootFireball()
	{
		shootTimer = 0;
		Instantiate(fireBall, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
	}
	//void shootIce()
	//{
	//	shootTimer = 0;
	//	Instantiate(Ice, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
	//   }
	//void shootLightning()
	//{
	//	shootTimer = 0;
	//	Instantiate(Lightning, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
	//   }

	public void TakeDMG(int amount)
	{
		HP -= amount;
		updatePlayerUI();
		StartCoroutine(flashDamageScreen());


        if (HP <= 0)
		{
			gameManager.instance.YouLose();
		}
	}

	void UseMana(int mana)
	{

	}

	public void updatePlayerUI()
	{
		gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
		//gameManager.instance
	}

	IEnumerator flashDamageScreen()
	{
		gameManager.instance.playerDamageScreen.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		gameManager.instance.playerDamageScreen.SetActive(false);
	}
}
