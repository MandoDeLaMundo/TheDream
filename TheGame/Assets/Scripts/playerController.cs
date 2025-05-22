using System.Data.SqlTypes;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class playerController : MonoBehaviour, IDamage, IPickup
{
	[SerializeField] CharacterController controller;
	[SerializeField] LayerMask ignoreLayer;

	[SerializeField] int HP;
	int HPOrig;

	[SerializeField] int Mana;
	int ManaOrig;

	[SerializeField] int speed;
	[SerializeField] int sprintMod;

	enum shootchoice { shootraycast, spellList }
	[SerializeField] shootchoice choice;
	[SerializeField] List<spellStats> spellList = new List<spellStats>();
	[SerializeField] GameObject spellModel;
	[SerializeField] GameObject spell;
	[SerializeField] Transform shootPos;
	[SerializeField] int shootDamage;
	[SerializeField] float shootRate;
	[SerializeField] int shootDist;
	[SerializeField] int manaCost;
	float shootTimer;
	int spellListPos;

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

	//[SerializeField] bool isFireball;
	//[SerializeField] GameObject fireBall;
	//[SerializeField] bool isIce;
	//[SerializeField] GameObject Ice;
	//[SerializeField] bool isLightning;
	//[SerializeField] GameObject Lightning;
	

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
		//if (isShooting)
		//{
		//	Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
		//}
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
			if (choice == shootchoice.shootraycast)
				shoot();
			if (choice == shootchoice.spellList && spell != null)
				shootSpell();
		}
		if (Input.GetButton("Fire2") && shootTimer >= teleportRate)
		{
			if(isTeleportingRaycast)
			teleportbyclick();
			if (isTeleportingProj)
				teleportproj();
		}

		selectSpell();
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
	void shootSpell()
	{
		shootTimer = 0;
		Instantiate(spell, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
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

	void selectSpell()
	{
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && spellListPos < spellList.Count - 1)
		{
			spellListPos++;
			changeSpell();
		}
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && spellListPos > 0)
        {
            spellListPos--;
            changeSpell();
        }
    }

	void changeSpell()
	{
		shootDamage = spellList[spellListPos].shootDMG;
		shootDist = spellList[spellListPos].shootDist;
		shootRate = spellList[spellListPos].shootRate;
		manaCost = spellList[spellListPos].manaCost;

		spellModel.GetComponent<MeshFilter>().sharedMesh = spellList[spellListPos].model.GetComponent<MeshFilter>().sharedMesh;
		spellModel.GetComponent<MeshRenderer>().sharedMaterial = spellList[spellListPos].model.GetComponent<MeshRenderer>().sharedMaterial;

		spell = spellList[spellListPos].spellProjectile;
    }

	public void GetSpellStats(spellStats spell)
	{
		spellList.Add(spell);
		spellListPos = spellList.Count - 1;

		changeSpell();
	}
}
