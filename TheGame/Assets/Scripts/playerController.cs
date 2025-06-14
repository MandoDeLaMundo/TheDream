using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour, IDamage, IPickup, IInteraction
{
    public static playerController instance;

    public CharacterController controller;
    [SerializeField] Camera mainCam;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int animTransSpeed;

    [SerializeField] int HP;
    int HPOrig;
    [SerializeField] float healingCooldown;
    public int healingnum;
    int healingnumOrig;
    public int numofhealpotions;
    float healTimer;

    [SerializeField] int Mana;
    int ManaOrig;
    [SerializeField] int manaCost;
    [SerializeField] float manaCoolDownRate;
    float manaCooldownTimer;
    [SerializeField] float manaRegenRate;
    float manaRegenTimer;
    public int numofmanapotions;

    [SerializeField] int Oxygen;
    int OxygenOrig;
    [SerializeField] Transform WaterPos;
    [SerializeField] LayerMask waterLayer;

    [SerializeField] float speed;
    float origSpeed;
    [SerializeField] int sprintMod;
    bool inMud = false;
    bool canSprint = true;

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
    public bool canShoot = true;

    [SerializeField] bool isTeleportingRaycast;
    [SerializeField] float teleportRate;
    [SerializeField] int teleportDist;

    [SerializeField] int jumpMax;
    [SerializeField] int jumpForce;
    [SerializeField] int Gravity;
    int jumpCount;
    int origJump;
    Vector3 playerVel;

    int baconcount;
    int beewaxcount;
    int mushroomscount;

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
        instance = this;
        gameManager.instance.DisplayDescription(startupDialogue);
        HPOrig = HP;
        ManaOrig = Mana;
        OxygenOrig = Oxygen;
        origSpeed = speed;
        origJump = jumpForce;


        healingnumOrig = healingnum;
        gameManager.instance.UpdatePlayerMaxHPMPOXCount(HP, Mana, Oxygen);
        gameManager.instance.UpdatePotionCount(numofhealpotions, numofmanapotions);
        updatePlayerUI();
        if (spellList.Count > 0)
            changeSpell();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        if (isTeleportingRaycast)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * teleportDist, Color.blue);
        }

        Movement();
        sprint();
    }

    void Movement()
    {
        //setAnimPara();
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

        if (controller.enabled == true)
            controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        if (controller.enabled == true)
            controller.Move(playerVel * Time.deltaTime);

        playerVel.y -= Gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            if (choice == shootchoice.shootraycast)
                shoot();
            if (choice == shootchoice.teleportraycast)
                teleportbyclick();
            if (choice == shootchoice.spellList && spellList.Count > 0 && Mana >= manaCost)
                shootSpell(canShoot);
        }
        if (Input.GetKey("f"))
        {
            PotionUsed();
        }
        if (Input.GetKey("c"))
        {
            CraftPotion();
        }
        if (manaCooldownTimer >= manaCoolDownRate && Mana < ManaOrig)
        {
            ManaRegen();
        }


        //if (Input.GetKey("b"))
        //{
        //    shield.SetActive(true);
        //Debug.Log(manaCost);
        //    Mana -= manaCost;
        //}
        //else
        //{
        //    shield.SetActive(false);
        //}

        selectSpell();

        gameManager.instance.UpdateIngredientCount(baconcount, beewaxcount, mushroomscount);
    }

    void setAnimPara()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
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
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDMG(shootDamage);
            }
        }
    }

    void shootSpell(bool _canShoot)
    {
        if (_canShoot)
        {
            shootTimer = 0;
            manaCooldownTimer = 0;

            Mana -= manaCost;
            gameManager.instance.UpdatePlayerMPCount(-manaCost);
            updatePlayerUI();
            if (spellList[spellListPos].name != "Teleport Spell" && spellList[spellListPos].name != "Super_FireBall")
            {
                Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    Vector3 targetPoint = hit.point;
                    Vector3 shootDirection = (targetPoint - shootPos.position).normalized;

                    Instantiate(spell, shootPos.position, Quaternion.LookRotation(shootDirection));
                    if (spellList[spellListPos].hitEffect != null)
                        Instantiate(spellList[spellListPos].hitEffect, shootPos.position, Quaternion.LookRotation(shootDirection));
                }
            }
            else if (spellList[spellListPos].name == "Super_FireBall")
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
    }

    void PotionUsed()
    {
        if (craftingSystem.instance.IsHPPotion() && HP < HPOrig && healTimer > healingCooldown)
        {
            Heal();
        }
        else if (craftingSystem.instance.IsMPPotion())
        {
            ManaPotion();
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
            gameManager.instance.UpdatePotionCount(-1, 0);
        }
    }

    void ManaPotion()
    {
        if (numofmanapotions > 0)
        {
            Mana += healingnum;
            if (Mana > ManaOrig)
            {
                healingnum = healingnum + (ManaOrig - Mana);
                gameManager.instance.UpdatePlayerMPCount(healingnum);
                Mana = ManaOrig;
                healingnum = healingnumOrig;
            }
            else
            {
                gameManager.instance.UpdatePlayerMPCount(healingnum);
            }
            healTimer = 0;

            updatePlayerUI();
            numofmanapotions--;
            gameManager.instance.UpdatePotionCount(0, -1);
        }
    }

    void CraftPotion()
    {
        if (craftingSystem.instance.IsHPPotion() && beewaxcount > 0 && mushroomscount > 0 && healTimer > healingCooldown)
        {
            numofhealpotions++;
            gameManager.instance.UpdatePotionCount(1, 0);
            beewaxcount--;
            mushroomscount--;

            healTimer = 0;
        }
        else if (craftingSystem.instance.IsMPPotion() && baconcount > 0 && mushroomscount > 0)
        {
            numofmanapotions++;
            gameManager.instance.UpdatePotionCount(0, 1);
            baconcount--;
            mushroomscount--;
        }
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
        if (!Cheatmanager.instance.IsInvulnerable())
        {
            aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
            HP -= amount;
            gameManager.instance.UpdatePlayerHPCount(-amount);
            updatePlayerUI();
        }
        StartCoroutine(flashDamageScreen());


        if (HP <= 0)
        {
            anim.SetTrigger("HP");
            gameManager.instance.YouLose();
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerManaBar.fillAmount = (float)Mana / ManaOrig;
        gameManager.instance.playerOxygenBarFiller.fillAmount = (float)Oxygen / OxygenOrig;
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

    bool SpellInventoryCheck()
    {
        return true;
    }

    public void GetItemStats(itemStats item)
    {
        if (item.itemName == "Boar Meat")
        {
            if (item.firstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                item.firstTime = false;
                baconcount += 1;
            }
            else
                baconcount += 1;
        }
        else if (item.itemName == "Bee Wax")
        {
            if (item.firstTime)
            {

                gameManager.instance.DisplayDescription(item.itemDescription);
                item.firstTime = false;
                beewaxcount += 1;
            }
            else
                beewaxcount += 1;
        }
        else if (item.itemName == "Mushroom")
        {
            if (item.firstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                item.firstTime = false;
                mushroomscount += 1;
            }
            else
                mushroomscount += 1;
        }
        else if (item.itemName == "Health Potion")
        {
            if (item.firstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                item.firstTime = false;
                numofhealpotions += 1;
                gameManager.instance.UpdatePotionCount(1, 0);
            }
            else
            {
                numofhealpotions += 1;
                gameManager.instance.UpdatePotionCount(1, 0);
            }
        }
        else if (item.itemName == "Mana Potion")
        {
            if (item.firstTime)
            {
                gameManager.instance.DisplayDescription(item.itemDescription);
                item.firstTime = false;
                numofmanapotions += 1;
                gameManager.instance.UpdatePotionCount(0, 1);
            }
            else
            {
                numofhealpotions += 1;
                gameManager.instance.UpdatePotionCount(0, 1);
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