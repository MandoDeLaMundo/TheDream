using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class playerController : MonoBehaviour, IDamage, IPickup, IInteraction
{
    public static playerController instance;

    [Header("Player")]
    public CharacterController controller;
    public Camera mainCam;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int animTransSpeed;

    [SerializeField] ItemCount ingredents;
    [SerializeField] ListsTracker listsTracker;

    [Header("Health")]
    public int HP;
    int HPOrig;
    [SerializeField] float healingCooldown;
    public int healingnum;
    public int numofhealpotions;
    float healTimer;
    public bool canTakeDam = true;

    [Header("Mana")]
    [SerializeField] int Mana;
    int ManaOrig;
    [SerializeField] int manaCost;
    [SerializeField] int shieldManaCost;
    [SerializeField] float manaCoolDownRate;
    float manaCooldownTimer;
    [SerializeField] float manaRegenRate;
    float manaRegenTimer;
    public int numofmanapotions;

    [Header("Oxygen")]
    public int Oxygen;
    public int OxygenOrig;
    [SerializeField] Transform WaterPos;
    [SerializeField] LayerMask waterLayer;

    [Header("Movement")]
    public float speed;
    public float origSpeed;
    [SerializeField] int sprintMod;
    bool inMud = false;
    bool canSprint = true;
    public bool canMove = true;
    public bool canStunned = true;
    [SerializeField] int superSpeed;

    enum shootchoice { shootraycast, spellList, teleportraycast }
    [Header("Shooting")]
    [SerializeField] shootchoice choice;
    public List<spellStats> spellList = new List<spellStats>();
    [SerializeField] GameObject spellModel;
    [SerializeField] GameObject spell;
    [SerializeField] Transform shootPos;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    float shootTimer;
    public int spellListPos;
    public bool canShoot = true;

    [Header("Shield")]
    [SerializeField] GameObject shield;
    [SerializeField] GameObject shieldBubble;
    [SerializeField] float shieldRate;
    float shieldTimer;

    [SerializeField] bool isTeleportingRaycast;
    [SerializeField] float teleportRate;
    float TeleportTimer;
    [SerializeField] int teleportDist;
    [SerializeField] GameObject TeleportModel;
    [SerializeField] GameObject spellTeleport;

    [Header("Jump")]
    [SerializeField] int jumpMax;
    [SerializeField] int jumpForce;
    [SerializeField] int Gravity;
    int jumpCount;
    int origJump;
    Vector3 playerVel;

    [Header("Audio")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audStep;
    [Range(0, 1)][SerializeField] float audStepVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;

    public string startupDialogue;

    int InventoryPos = 0;
    public bool IsInventory;
    public bool PauseGameInInventory;

    bool isSprinting;
    bool isPlayingStep;
    bool isShielding = false;
    Coroutine co;

    public float potionTimerUse;
    float potionTimer;
    int OverMax;

    [SerializeField] itemStats healthPotionStats;
    [SerializeField] itemStats manaPotionStats;
    [SerializeField] itemStats healthPotionPlusStats;
    [SerializeField] itemStats manaPotionPlusStats;

    bool test;

    Vector3 moveDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        HPOrig = HP;
        ManaOrig = Mana;
        OxygenOrig = Oxygen;
        origSpeed = speed;
        origJump = jumpForce;
        test = true;
        IsInventory = false;
        canTakeDam = true;
        OverMax = 0;

        if(listsTracker.spellList.Count != 0 && spellList.Count == 0)
        {
            for (int i = 0; i < listsTracker.spellList.Count; i++)
            {
                spellList.Add(listsTracker.spellList[i]);
                changeSpell();
            }
        }

        gameManager.instance.UpdatePlayerMaxHPMPOXCount(HP, Mana, Oxygen);
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

        if (Cheatmanager.instance.SpeedCheat)
        {
            speed = origSpeed * superSpeed;
        }
    }

    void Movement()
    {
        //setAnimPara();
        shootTimer += Time.deltaTime;
        healTimer += Time.deltaTime;
        TeleportTimer += Time.deltaTime;
        potionTimer += Time.deltaTime;

        if (Mana != ManaOrig)
            manaCooldownTimer += Time.deltaTime;

        if (test)
        {
            Oxygen -= 5;
            gameManager.instance.UpdatePlayerOXCount(-5);
            updatePlayerUI();
            test = false;
        }

        if (controller.isGrounded)
        {
            if (moveDir.normalized.magnitude > 0.3f && !isPlayingStep)
            {
                StartCoroutine(playStep());
            }
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        if (canMove)
        {
            moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

            if (controller.enabled == true)
                controller.Move(moveDir * speed * Time.deltaTime);

            jump();

            if (controller.enabled == true)
                controller.Move(playerVel * Time.deltaTime);

            playerVel.y -= Gravity * Time.deltaTime;
        }


        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            if (choice == shootchoice.shootraycast)
                shoot();
            if (choice == shootchoice.teleportraycast)
                teleportbyclick();
            if (choice == shootchoice.spellList && spellList.Count > 0 && Mana >= manaCost)
                shootSpell(canShoot);
        }
        if (Input.GetButton("Fire2") && TeleportTimer >= teleportRate && spellTeleport != null)
        {
            Teleport();
            TeleportTimer = 0;
        }
        if (Input.GetKey("f"))
        {
            PotionUsed();
        }
        if (manaCooldownTimer >= manaCoolDownRate && Mana < ManaOrig)
        {
            ManaRegen();
        }

        if (Input.GetButtonDown("Shield") && shield != null && gameManager.instance.Shield.sprite != null)
        {
            isShielding = !isShielding;
        }
        if (isShielding && Mana > 0)
        {
            shieldTimer += Time.deltaTime;
            Shield();
        }
        else
        {
            isShielding = false;
            shieldBubble.SetActive(isShielding);
            shieldTimer = 0;
        }

        if (Input.GetButtonDown("Inventory"))
        {
            IsInventory = !IsInventory;
            Inventory();
        }

        if (potionTimer > potionTimerUse)
        {
            if (Input.GetKeyDown("z"))
            {
                HealPotion();
            }

            if (Input.GetKeyDown("x"))
            {
                ManaPotion();
            }

            if (Input.GetKeyDown("c"))
            {
                HealPotionPlus();
            }

            if (Input.GetKeyDown("v"))
            {
                ManaPotionPlus();
            }
        }



        selectSpell();

        gameManager.instance.UpdateIngredientCount(ingredents.baconCount, ingredents.beewaxCount, ingredents.mushroomCount);
    }

    void setAnimPara()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    }

    void Shield()
    {
        shield.SetActive(isShielding);
        shieldBubble.SetActive(isShielding);
        if (shieldTimer >= shieldRate)
        {
            Mana -= shieldManaCost;
            gameManager.instance.UpdatePlayerMPCount(-shieldManaCost);
            updatePlayerUI();
            shieldTimer = 0;
        }
    }

    void Inventory()
    {
        if (IsInventory == false)
        {
            gameManager.instance.StateUnpause();
        }
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
            IDamage dmg = hit.collider.GetComponentInParent<IDamage>();

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
            if (spellList[spellListPos].name != "Spell7_Teleport Spell" && spellList[spellListPos].name != "Spell2_Super_FireBall")
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
            else if (spellList[spellListPos].name == "Spell2_Super_FireBall")
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
            HealPotion();
        }
        else if (craftingSystem.instance.IsMPPotion())
        {
            ManaPotion();
        }
    }

    void HealPotion()
    {
        if (ingredents.HealthPotion > 0)
        {
            OverMax = HP + healthPotionStats.healFactor;
            if (OverMax > HPOrig)
            {
                OverMax = HPOrig - HP;
                gameManager.instance.UpdatePlayerHPCount(OverMax);
                HP = HPOrig;
                OverMax = 0;
            }
            else
            {
                gameManager.instance.UpdatePlayerHPCount(healthPotionStats.healFactor);
                HP += healthPotionStats.healFactor;
            }
            healTimer = 0;

            updatePlayerUI();
            ingredents.HealthPotion--;
            gameManager.instance.UpdatePotionCount();
        }
    }

    void ManaPotion()
    {
        if (ingredents.ManaPotion > 0)
        {
            OverMax = Mana + manaPotionStats.ManaFactor;
            if (OverMax > ManaOrig)
            {
                OverMax = ManaOrig - Mana;
                gameManager.instance.UpdatePlayerMPCount(OverMax);
                Mana = ManaOrig;
                OverMax = 0;
            }
            else
            {
                gameManager.instance.UpdatePlayerMPCount(manaPotionStats.ManaFactor);
                Mana += manaPotionStats.ManaFactor;
            }
            healTimer = 0;

            updatePlayerUI();
            ingredents.ManaPotion--;
            gameManager.instance.UpdatePotionCount();
        }
    }

    void HealPotionPlus()
    {
        if (ingredents.HealPlusPotion > 0)
        {
            OverMax = HP + healthPotionPlusStats.healFactor;
            if (OverMax > HPOrig)
            {
                OverMax = HPOrig - HP;
                gameManager.instance.UpdatePlayerHPCount(OverMax);
                HP = HPOrig;
                OverMax = 0;
            }
            else
            {
                gameManager.instance.UpdatePlayerHPCount(healthPotionPlusStats.healFactor);
                HP += healthPotionPlusStats.healFactor;
            }
            healTimer = 0;

            updatePlayerUI();
            ingredents.HealPlusPotion--;
            gameManager.instance.UpdatePotionCount();
        }
    }

    void ManaPotionPlus()
    {
        if (ingredents.ManaPlusPotion > 0)
        {
            OverMax = Mana + manaPotionPlusStats.ManaFactor;
            if (OverMax > ManaOrig)
            {
                OverMax = ManaOrig - Mana;
                gameManager.instance.UpdatePlayerMPCount(OverMax);
                Mana = ManaOrig;
                OverMax = 0;
            }
            else
            {
                gameManager.instance.UpdatePlayerMPCount(manaPotionPlusStats.ManaFactor);
                Mana += manaPotionPlusStats.ManaFactor;
            }
            healTimer = 0;

            updatePlayerUI();
            ingredents.ManaPlusPotion--;
            gameManager.instance.UpdatePotionCount();
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
        if (Mana == ManaOrig || Input.GetButton("Fire1") || shieldTimer > 0)
        {
            manaCooldownTimer = 0;
        }
    }

    void teleportbyclick()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
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
        if (canTakeDam)
        {
            if (!Cheatmanager.instance.IsInvulnerable())
            {
                aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
                HP -= amount;
                gameManager.instance.UpdatePlayerHPCount(-amount);
                updatePlayerUI();
                if (canStunned)
                    StartCoroutine(Stunned(0.5f));
            }
        }
        else
        {

        }


        StartCoroutine(flashDamageScreen());
        //StartCoroutine(PostInvulnerable());


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
        if (Input.anyKeyDown)
        {
            for (int i = 0; i <= 6; i++)
            {
                KeyCode key = KeyCode.Alpha0 + i;

                if (Input.GetKeyDown(key) && spellListPos < spellList.Count && spellList.Count != 1)
                {
                    int spellpos = i - 1;
                    spellListPos = spellpos;
                    changeSpell();
                }
            }
        }
        //listsTracker.spellListPos = spellListPos;
    }

    void changeSpell()
    {
        shootDamage = spellList[spellListPos].shootDMG;

        shootDist = spellList[spellListPos].shootDist;
        shootRate = spellList[spellListPos].shootRate;
        manaCost = spellList[spellListPos].manaCost;

        spellModel.GetComponent<MeshFilter>().sharedMesh = spellList[spellListPos].model.GetComponent<MeshFilter>().sharedMesh;
        spellModel.GetComponent<MeshRenderer>().sharedMaterial = spellList[spellListPos].model.GetComponent<MeshRenderer>().sharedMaterial;

        //if (spellList[spellListPos] != null)
        //    listsTracker.spellList.Add(spellList[spellListPos]);

        if (DisplayHotBar.instance == null)
        {
            
        }
        else
        {
            DisplayHotBar.instance.HotBar(spellListPos);
        }

        spell = spellList[spellListPos].spellProjectile;
    }

    public void GetSpellStats(spellStats spell)
    {
        if (spell.spellCheck)
        {
            if (spell.name != "Spell8_Shield" && spell.name != "Spell7_Teleport Spell")
            {
                listsTracker.spellList.Add(spell);
                spellList.Add(spell);
                spellListPos = spellList.Count - 1;

                changeSpell();
                spell.spellCheck = false;
            }
            else if (spell.name == "Spell8_Shield") //shield values
            {
                shield = spell.model;
                shieldManaCost = spell.manaCost;
                shieldRate = spell.shootRate;
                spell.spellCheck = false;

                gameManager.instance.Shield.sprite = spell.sprite;
                gameManager.instance.ShieldObj.SetActive(true);

            } // who watching?
            else
            {
                gameManager.instance.TeleportSlot.sprite = spell.sprite;
                teleportRate = spell.shootRate;

                TeleportModel.GetComponent<MeshFilter>().sharedMesh = spell.model.GetComponent<MeshFilter>().sharedMesh;
                TeleportModel.GetComponent<MeshRenderer>().sharedMaterial = spell.model.GetComponent<MeshRenderer>().sharedMaterial;

                spellTeleport = spell.spellProjectile;
                gameManager.instance.TeleportObj.SetActive(true);
            }
            if (!Cheatmanager.instance.DescriptionCheat)
                gameManager.instance.DisplayDescription(spell.spellManual);
        }

        if (Cheatmanager.instance.spellCheat == true)
        {
            spellList.Add(spell);
            spellListPos = listsTracker.spellList.Count;

            changeSpell();
            spell.spellCheck = false;
        }
    }

    bool SpellInventoryCheck()
    {
        return true;
    }

    public void GetItemStats(itemStats item)
    {
        //switch(item.itemName)
        if (item.itemName == "Bee Wax")
        {
            ingredents.beewaxCount++;
        }
        else if (item.itemName == "Boar Meat")
        {
            ingredents.baconCount++;
        }
        else if (item.itemName == "Mushroom")
        {
            ingredents.mushroomCount++;
        }
        else if (item.itemName == "Venom Gland")
        {
            ingredents.venomGlandCount++;
        }
        else if (item.itemName == "Health Potion")
        {
            ingredents.HealthPotion++;
        }
        else if (item.itemName == "Mana Potion")
        {
            ingredents.ManaPotion++;
        }
        else if (item.itemName == "Potion+")
        {
            ingredents.HealPlusPotion++;
        }
        else if (item.itemName == "ManaPotion+")
        {
            ingredents.ManaPlusPotion++;
        }

        if (item.itemName == "Boss Egg")
        {
            item.bossCheck = false;
            gameManager.instance.GameGoalMonsterEgg();
        }
        else if(item.itemName == "Cinnamon")
        {
            item.bossCheck = false;
            gameManager.instance.YouWin();
        }

        if (item.firstTime && Cheatmanager.instance.DescriptionCheat == false && item.itemName != "Boss Egg" && item.itemName != "Cinnamon")
        {
            gameManager.instance.DisplayDescription(item.itemDescription);
            item.firstTime = false;
        }

        if (item.itemName != "Health Potion" && item.itemName != "Mana Potion" && item.itemName != "Potion+" && item.itemName != "ManaPotion+")
        {
            if (InventorySystem.instance.inventoryStats.Count <= gameManager.instance.items.Count && !InventorySystem.instance.inventoryStats.Contains(item))
            {
                InventorySystem.instance.inventoryStats.Add(item);
                item.Count++;
                InventorySystem.instance.StoredInventory(InventoryPos);
                InventoryPos++;
            }
            else
            {
                item.Count++;
            }
        }
        else
        {
            gameManager.instance.UpdatePotionCount();
        }

        InventorySystem.instance.VerifyCount();
    }

    void Teleport()
    {
        GameObject teleproj = Instantiate(spellTeleport, shootPos.position, Quaternion.LookRotation(Camera.main.transform.forward));
        teleproj.GetComponent<Teleport>().player = gameObject;
        teleproj.GetComponent<Teleport>().playercon = controller;
    }

    public void Stun(float duration)
    {
        StartCoroutine(Stunned(duration));
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

    IEnumerator Stunned(float duration)
    {
        canShoot = false;
        canMove = false;
        gameManager.instance.playerStunScreen.SetActive(true);
        yield return new WaitForSeconds(duration);
        gameManager.instance.playerStunScreen.SetActive(false);
        canMove = true;
        canShoot = true;
    }

    IEnumerator PostInvulnerable()
    {
        canTakeDam = false;
        yield return new WaitForSeconds(2f);
        canTakeDam = true;
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