using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour, IDamage, IPickup, IInteraction
{
    [SerializeField] CharacterController controller;
    //[SerializeField] Animator anim;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    int HPOrig;

    [SerializeField] int Mana;
    int ManaOrig;

    [SerializeField] float speed;
    float origSpeed;


    [SerializeField] int sprintMod;
    enum shootchoice { shootraycast, spellList, teleportraycast }
    [SerializeField] shootchoice choice;
    [SerializeField] List<spellStats> spellList = new List<spellStats>();
    [SerializeField] GameObject spellModel;
    [SerializeField] GameObject spell;
    [SerializeField] Transform shootPos;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    float shootTimer;
    int spellListPos;

    [SerializeField] int manaCost;
    [SerializeField] float manaCoolDownRate;
    [SerializeField] float manaRegenRate;
    float manaRegenTimer;
    float manaCooldownTimer;

    [SerializeField] bool isTeleportingRaycast;
    [SerializeField] float teleportRate;
    [SerializeField] int teleportDist;

    [SerializeField] int jumpMax;
    [SerializeField] int jumpForce;
    [SerializeField] int Gravity;
    int jumpCount;
    int origJump;
    Vector3 playerVel;

    [SerializeField] float healingCooldown;
    public int healingnum;
    int healingnumOrig;
    public int numofhealpotions;
    float healTimer;

    int baconcount;
    int beewaxcount;
    int mushroomscount;
    int baconMax;
    int beewaxMax;
    int mushroomsMax;
    bool baconFirstTime;
    bool beewaxFirstTime;
    bool mushroomsFirstTime;
    bool healpotionFirstTime;
    bool inMud = false;
    bool canSprint = true;


    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audStep;
    [Range(0, 1)][SerializeField] float audStepVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;

    [SerializeField] GameObject shield;

    public string startupDialogue;

    bool isSprinting;
    bool isPlayingStep;
    Coroutine co;

    Vector3 moveDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.DisplayDescription(startupDialogue);
        HPOrig = HP;
        ManaOrig = Mana;
        origSpeed = speed;
        origJump = jumpForce;


        healingnumOrig = healingnum;
        gameManager.instance.UpdatePlayerMaxHPMPCount(HP, Mana);
        gameManager.instance.UpdatePotionCount(numofhealpotions);
        updatePlayerUI();
        FirstTime();
        if (spellList.Count > 0)
            changeSpell();
    }

    // Update is called once per frame
    void Update()
    {
        //if (choice == shootchoice.shootraycast)
        //{
        //Debug.Log(transform.position);
        Debug.DrawRay(shootPos.position, Camera.main.transform.forward * shootDist, Color.red);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        //}
        if (isTeleportingRaycast)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * teleportDist, Color.blue);
        }

        //   if (controller.transform.position.y < 0)
        //      TakeDMG(100);

        Movement();
        sprint();
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;
        healTimer += Time.deltaTime;

        if (Mana != ManaOrig)
            manaCooldownTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            if (moveDir.normalized.magnitude > 0.3f && !isPlayingStep)
            {
                StartCoroutine(playStep());
            }
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
            if (choice == shootchoice.teleportraycast)
                teleportbyclick();
            if (choice == shootchoice.spellList && spellList.Count > 0 && Mana >= manaCost)
                shootSpell();
        }

        if (Input.GetKey("r") && HP < HPOrig && healTimer > healingCooldown)
        {
            Heal();
        }
        if (Input.GetKey("c") && beewaxcount > 0 && mushroomscount > 0 && healTimer > healingCooldown)
        {
            CraftPotion();
        }
        if (manaCooldownTimer >= manaCoolDownRate)
        {
            ManaRegen();
        }

        if (Input.GetKey("b"))
        {
            if (Input.GetKeyDown("b"))
            {
                shield.SetActive(true);
                Mana -= manaCost;
            }
            else
            {
                shield.SetActive(false);
            }
        }

        selectSpell();

        gameManager.instance.UpdateIngredientCount(baconcount, beewaxcount, mushroomscount);
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpForce;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }
    }

    void sprint()
    {
        if (!canSprint)
        {
            if (isSprinting)
            {
                speed = origSpeed;
                isSprinting = false;
            }
            return;
        }
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            //speed += sprintMod;
            speed = origSpeed * sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            //speed -= sprintMod;
            speed = origSpeed;
            isSprinting = false;
        }



    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {

            //Debug.Log(hit.collider.name);
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
        manaCooldownTimer = 0;

        Mana -= manaCost;
        gameManager.instance.UpdatePlayerMPCount(-manaCost);
        updatePlayerUI();
        if (spellList[spellListPos].name != "Teleport Spell")
        {
            Instantiate(spell, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
            if (spellList[spellListPos].hitEffect != null)
                Instantiate(spellList[spellListPos].hitEffect, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
        }
        else
        {
            Teleport();
        }
    }

    void Heal()
    {
        if (numofhealpotions > 0)
        {
            HP += healingnum;
            if (HP > HPOrig)
            {
                healingnum = healingnum + (HPOrig - HP);
                gameManager.instance.UpdatePlayerHPCount(healingnum);
                HP = HPOrig;
                healingnum = healingnumOrig;
            }
            else
            {
                gameManager.instance.UpdatePlayerHPCount(healingnum);
            }
            healTimer = 0;

            updatePlayerUI();
            numofhealpotions--;
            gameManager.instance.UpdatePotionCount(-1);
        }
    }

    void CraftPotion()
    {
        numofhealpotions++;
        gameManager.instance.UpdatePotionCount(1);
        beewaxcount--;
        mushroomscount--;

        healTimer = 0;
    }

    void ManaRegen()
    {
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= manaRegenRate)
        {
            Mana += 1;
            gameManager.instance.UpdatePlayerMPCount(1);
            updatePlayerUI();
            manaRegenTimer = 0;
        }
        if (Mana == ManaOrig || Input.GetButton("Fire1"))
        {
            manaCooldownTimer = 0;
        }
    }

    void teleportbyclick()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            //Debug.Log(hit.collider.name);
            Vector3 teleportPosition = hit.point;
            if (Vector3.Distance(transform.position, teleportPosition) <= teleportDist)
            {
                controller.enabled = false;
                teleportPosition.y = 1.0f;
                transform.position = teleportPosition;
                controller.enabled = true;
            }
        }
    }

    public void TakeDMG(int amount)
    {
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        HP -= amount;
        gameManager.instance.UpdatePlayerHPCount(-amount);
        updatePlayerUI();
        StartCoroutine(flashDamageScreen());


        if (HP <= 0)
        {
            //anim.SetTrigger("HP");
            gameManager.instance.YouLose();
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerManaBar.fillAmount = (float)Mana / ManaOrig;
    }

    void selectSpell()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && spellListPos < spellList.Count - 1)
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
        if (spell.spellCheck)
        {
            spellList.Add(spell);
            spellListPos = spellList.Count - 1;

            changeSpell();
            gameManager.instance.DisplayDescription(spell.spellManual);
        }
    }

    public void GetItemStats(itemStats item)
    {
        if (item.itemName == "Boar Meat")
        {
            if (baconFirstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                baconFirstTime = false;
                baconcount += 1;
            }
            else
                baconcount += 1;
        }
        else if (item.itemName == "Bee Wax")
        {
            if (beewaxFirstTime)
            {

                gameManager.instance.DisplayDescription(item.itemDescription);
                beewaxFirstTime = false;
                beewaxcount += 1;
            }
            else
                beewaxcount += 1;
        }
        else if (item.itemName == "Mushroom")
        {
            if (mushroomsFirstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                mushroomsFirstTime = false;
                mushroomscount += 1;
            }
            else
                mushroomscount += 1;
        }
        else if (item.itemName == "Health Potion")
        {
            if (healpotionFirstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                healpotionFirstTime = false;
                numofhealpotions += 1;
                gameManager.instance.UpdatePotionCount(1);
            }
            else
            {
                numofhealpotions += 1;
                gameManager.instance.UpdatePotionCount(1);
            }
        }
        else if (item.itemName == "Boss Egg")
        {
            gameManager.instance.UpdateMonsterEgg(true);
            gameManager.instance.GameGoalMonsterEgg();
        }
    }

    void Teleport()
    {
        GameObject teleproj = Instantiate(spell, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
        teleproj.GetComponent<Teleport>().player = gameObject;
        teleproj.GetComponent<Teleport>().playercon = controller;
    }

    IEnumerator flashDamageScreen()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }

    IEnumerator playStep()
    {
        isPlayingStep = true;
        aud.PlayOneShot(audStep[Random.Range(0, audStep.Length)], audStepVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingStep = false;
    }

    void FirstTime()
    {
        baconFirstTime = true;
        beewaxFirstTime = true;
        mushroomsFirstTime = true;
        healpotionFirstTime = true;
    }

    public void EnterMud()
    {
        if (inMud) return;

        if (isSprinting)
        {
            speed = origSpeed;
            isSprinting = false;
        }


        origSpeed = Mathf.Max(1, speed / 2);
        speed = origSpeed;
        jumpForce = Mathf.Max(1, jumpForce / 2);

        canSprint = false;
        inMud = true;
    }

    public void ExitMud()
    {
        if (!inMud) return;

        origSpeed *= 2;
        speed = origSpeed;
        jumpForce = origJump;

        canSprint = true;
        inMud = false;
    }
}
