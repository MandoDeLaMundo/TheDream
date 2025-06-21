using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("Menus")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    public GameObject Inventory;

    [Header("Texts")]
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] TMP_Text bossHPCountText;
    [SerializeField] TMP_Text bossHPMaxText;
    [SerializeField] TMP_Text baconCountText;
    [SerializeField] TMP_Text beesWaxCountText;
    [SerializeField] TMP_Text mushroomCountText;
    [SerializeField] TMP_Text baconGoalText;
    [SerializeField] TMP_Text beesWaxGoalText;
    [SerializeField] TMP_Text mushroomGoalText;

    [Header("Player Health")]
    [SerializeField] TMP_Text playerHPCountText;
    [SerializeField] TMP_Text playerHPMaxText;
    public Image playerHPBar;
    int playerHPCountOrig;
    int playerHPMaxOrig;

    [Header("Player Mana")]
    [SerializeField] TMP_Text playerMPCountText;
    [SerializeField] TMP_Text playerMPMaxText;
    public Image playerManaBar;
    int playerMPCountOrig;
    int playerMPMaxOrig;

    [Header("Player Oxgyen")]
    [SerializeField] TMP_Text playerOXCountText;
    [SerializeField] TMP_Text playerOXMaxText;
    public Image playerOxygenBarFiller;
    int playerOXCountOrig;
    int playerOXMaxOrig;

    [Header("Potions Text")]
    [SerializeField] TMP_Text healpotionText;
    int healpotionCountOrig;
    [SerializeField] TMP_Text manapotionText;
    int manapotionCountOrig;
    [SerializeField] TMP_Text healpotionplusText;
    int healpotionplusCountOrig;
    [SerializeField] TMP_Text manapotionplusText;
    int manapotionplusCountOrig;


    [Header("HotBar")]
    public Image MainSpell;
    public Image SpellOne;
    public Image SpellTwo;
    public Image SpellThree;
    public Image SpellFour;
    public Image SpellFive;
    [SerializeField] List<spellStats> Spell = new List<spellStats>();
    public List<itemStats> items = new List<itemStats>();
    [SerializeField] ItemCount ItemCount;
    [SerializeField] ListsTracker AllLists;

    public GameObject TeleportObj;
    public Image TeleportSlot;
    public GameObject ShieldObj;
    public Image Shield;

    [Header("Description")]
    public GameObject textBox;
    public TMP_Text textDescription;

    [Header("Dialogue")]
    public GameObject DialogueBox;
    public TMP_Text DialogueDescription;

    [Header("Screens")]
    public GameObject playerDamageScreen;
    public GameObject playerStunScreen;

    public GameObject GodMode;
    public GameObject NormalMode;

    public GameObject player;
    public playerController playerScript;
    public Image bossHPBar;
    public int baconGoalPI;
    public int beesWaxGoalPI;
    public int mushroomGoalPI;

    public bool isPaused;

    float timeScaleOrig;
    int gameGoalCount;
    int bossHPCountOrig;
    int bossHPMaxOrig;
    int baconCount;
    int beesWaxCount;
    int mushroomCount;
    int baconGoal;
    int beesWaxGoal;
    int mushroomGoal;

    bool hasMonsterEgg = false;
    bool hasEnoughBacon = false;
    bool hasEnoughBeesWax = false;
    bool hasEnoughMushroom = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<playerController>();
        }
        timeScaleOrig = Time.timeScale;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AllReset();
        UpdatePotionCount();
        UpdateIngredientGoal(baconGoalPI, beesWaxGoalPI, mushroomGoalPI);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                StatePause();
                menuActive = menuPause;

                if (menuActive != null)
                {
                    menuActive.SetActive(true);
                }

            }
            else if (menuActive == menuPause)
                StateUnpause();
        }

        if (playerController.instance != null && playerController.instance.IsInventory)
        {
            if (playerController.instance.IsInventory)
            {
                if (menuActive != menuPause)
                {
                    if (playerController.instance.PauseGameInInventory)
                        StatePause();

                    menuActive = Inventory;
                    menuActive.SetActive(true);
                }
                else
                {
                    playerController.instance.IsInventory = false;
                }
            }
        }
       

        if (Input.GetKey("q"))
        {
            HideDescription();
        }
    }

    public void StatePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void YouLose()
    {
        StatePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void DisplayDescription(string description)
    {
        textBox.SetActive(true);
        textDescription.text = description;

        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideDescription()
    {
        textBox.SetActive(false);
        textDescription.text = "";

        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisplayDialogue(string Dialogue)
    {
        DialogueBox.SetActive(true);
        DialogueDescription.text = Dialogue;

        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideDialogue()
    {
        DialogueBox.SetActive(false);
        DialogueDescription.text = "";

        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateGameGoal(int amount)
    {
        gameGoalCount += amount;
        //gameGoalCountText.text = gameGoalCount.ToString("F0");

        if (gameGoalCount <= 0)
        {
            StatePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void UpdatePlayerHPCount(int amount)
    {
        playerHPCountOrig += amount;
        playerHPCountText.text = playerHPCountOrig.ToString("F0");
    }

    public void UpdatePlayerMPCount(int amount)
    {
        playerMPCountOrig += amount;
        playerMPCountText.text = playerMPCountOrig.ToString("F0");
    }

    public void UpdatePlayerOXCount(int amount)
    {
        playerOXCountOrig += amount;
        playerOXCountText.text = playerOXCountOrig.ToString("F0");
    }

    public void UpdatePlayerMaxHPMPOXCount(int hpAmount, int mpAmount, int oxAmount)
    {
        UpdatePlayerHPCount(hpAmount);
        UpdatePlayerMPCount(mpAmount);
        UpdatePlayerOXCount(oxAmount);

        playerHPMaxOrig += hpAmount;
        playerHPMaxText.text = playerHPMaxOrig.ToString("F0");

        playerMPMaxOrig += mpAmount;
        playerMPMaxText.text = playerMPMaxOrig.ToString("F0");

        playerOXMaxOrig += oxAmount;
        playerOXMaxText.text = playerOXMaxOrig.ToString("F0");
    }

    public void UpdateBossHPCount(int amount)
    {
        bossHPCountOrig += amount;
        bossHPCountText.text = bossHPCountOrig.ToString("F0");
    }

    public void UpdatePotionCount()
    {
        if(healpotionText != null)
        {
            healpotionCountOrig = ItemCount.HealthPotion;
            healpotionText.text = healpotionCountOrig.ToString("F0");
        }
      
        if (manapotionText != null)
        {
            manapotionCountOrig = ItemCount.ManaPotion;
            manapotionText.text = manapotionCountOrig.ToString("F0");
        }
       
        if (healpotionplusText != null)
        {
            healpotionCountOrig = ItemCount.HealPlusPotion;
            healpotionplusText.text = healpotionplusCountOrig.ToString("F0");
        }
        
        if (manapotionplusText != null)
        {
            manapotionCountOrig = ItemCount.ManaPlusPotion;
            manapotionplusText.text = manapotionplusCountOrig.ToString("F0");
        }
    }

    public void UpdateIngredientCount(int baconAmount, int beesWaxAmount, int mushroomAmount)
    {
        baconCount = baconAmount;
        beesWaxCount = beesWaxAmount;
        mushroomCount = mushroomAmount;

        baconCountText.text = baconCount.ToString("F0");
        beesWaxCountText.text = beesWaxCount.ToString("F0");
        mushroomCountText.text = mushroomCount.ToString("F0");

        CheckIngredientGoals();
    }

    public void UpdateIngredientGoal(int baconAmount, int beesWaxAmount, int mushroomAmount)
    {
        baconGoal = baconAmount;
        beesWaxGoal = beesWaxAmount;
        mushroomGoal = mushroomAmount;

        if (baconGoalText != null)
        {
            baconGoalText.text = baconGoal.ToString("F0");
        }
        if (beesWaxGoalText != null)
        {
            beesWaxGoalText.text = beesWaxGoal.ToString("F0");
        }

        if (mushroomGoalText != null)
        {
            mushroomGoalText.text = mushroomGoal.ToString("F0");
        }
    }

    private void CheckIngredientGoals()
    {
        hasEnoughBacon = baconCount >= baconGoal;
        hasEnoughBeesWax = beesWaxCount >= beesWaxGoal;
        hasEnoughMushroom = mushroomCount >= mushroomGoal;
    }

    public void UpdateMonsterEgg(bool hasEgg)
    {
        hasMonsterEgg = hasEgg;
    }

    public void GameGoalMonsterEgg()
    {
        if (hasMonsterEgg)
        {
            StatePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    void PickUpCheck()
    {
        for (int i = 0; i < Spell.Count; i++)
            Spell[i].spellCheck = true;
        for (int i = 0; i < items.Count; i++)
            items[i].firstTime = true;
    }

    void InventoryReset()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].Count = 0;
        }
    }

    void AllReset()
    {
        PickUpCheck();
        if(ItemCount != null)
        {
            ItemCount.beewaxCount = 0;
            ItemCount.baconCount = 0;
            ItemCount.mushroomCount = 0;
            ItemCount.HealthPotion = 0;
            ItemCount.ManaPotion = 0;
            ItemCount.HealPlusPotion = 0;
            ItemCount.ManaPlusPotion = 0;
        }
        InventoryReset();
        //if (AllLists.spellList.Count > 0)
        //{
        //    AllLists.spellList = null;
        //    AllLists.spellListPos = 0;
        //    AllLists.ItemList = null;
        //}
    }
}
