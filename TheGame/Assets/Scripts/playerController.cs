using System.Data.SqlTypes;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class playerController : MonoBehaviour
{
	[SerializeField] CharacterController controller;
	[SerializeField] LayerMask ignoreLayer;

	[SerializeField] int HP;
	int HPOrig;

	[SerializeField] int speed;
	[SerializeField] int sprintMod;

	[SerializeField] bool isShooting;
	[SerializeField] int shootDamage;
	[SerializeField] float shootRate;
	[SerializeField] int shootDist;
	float shootTimer;

	[SerializeField] bool isTeleporting;
	[SerializeField] float teleportRate;
	[SerializeField] int teleportDist;

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
		updatePlayerUI();

    }

	// Update is called once per frame
	void Update()
	{
		if (isShooting)
		{
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
		}
		if (isTeleporting)
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
			if(isShooting)
			shoot();
			if(isFireball)
			shootFireball();
			if(isIce)
			shootIce();
			if(isLightning)
			shootLightning();
		}
		if (Input.GetButton("Fire2") && shootTimer >= teleportRate && isTeleporting)
		{
			teleportbyclick();
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
		shootTimer = 0;

		RaycastHit hit;

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
		{
			Debug.Log(hit.collider.name);
			Vector3 teleportpos = hit.point;
			if (Vector3.Distance(transform.position, teleportpos) <= teleportDist)
			{
				teleportpos.y = transform.position.y;
				transform.position = teleportpos;
			}
		}
	}

	void shootFireball()
	{
		shootTimer = 0;
		Instantiate(fireBall, shootPos.position, transform.rotation);
	}
	void shootIce()
	{
		shootTimer = 0;
		Instantiate(Ice, shootPos.position, transform.rotation);
	}
	void shootLightning()
	{
		shootTimer = 0;
		Instantiate(Lightning, shootPos.position, transform.rotation);
	}

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

	public void updatePlayerUI()
	{
		gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
	}

	IEnumerator flashDamageScreen()
	{
		gameManager.instance.playerDamageScreen.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		gameManager.instance.playerDamageScreen.SetActive(false);
	}
}
