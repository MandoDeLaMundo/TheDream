using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class gameManager : MonoBehaviour
{
	public static gameManager instance;

	[SerializeField] GameObject menuActive;
	[SerializeField] GameObject menuPause;
	[SerializeField] GameObject menuWin;
	[SerializeField] GameObject menuLose;
	[SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] TMP_Text playerHPCountText;
    [SerializeField] TMP_Text playerMPCountText;
	[SerializeField] TMP_Text playerHPMaxText;
	[SerializeField] TMP_Text playerMPMaxText;
	[SerializeField] TMP_Text textOBJ1;
	[SerializeField] TMP_Text textOBJ2;
    [SerializeField] TMP_Text textOBJ3;
    [SerializeField] TMP_Text textOBJ4;
    [SerializeField] TMP_Text potionText;

    public GameObject playerDamageScreen;
	public Image playerHPBar;
	public GameObject player;
	public playerController playerScript;
    public Image playerManaBar;
	public GameObject textBox;
	public TMP_Text textDescription;

    public bool isPaused;

	float timeScaleOrig;
	int gameGoalCount;
	int playerHPCountOrig;
	int playerMPCountOrig;
	int playerHPMaxOrig;
	int playerMPMaxOrig;
	int potionCountOrig;
	int object4CountOrig;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		instance = this;
		player = GameObject.FindWithTag("Player");
		playerScript = player.GetComponent<playerController>();
		timeScaleOrig = Time.timeScale;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
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
				menuActive.SetActive(isPaused);
			}

			else if (menuActive == menuPause)
				StateUnpause();
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

	public void UpdateGameGoal(int amount)
	{
		gameGoalCount += amount;
		gameGoalCountText.text = gameGoalCount.ToString("F0");

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

	public void UpdatePlayerMaxHPMPCount(int hpAmount, int mpAmount)
	{
		UpdatePlayerHPCount(hpAmount);
		UpdatePlayerMPCount(mpAmount);

		playerMPMaxOrig += mpAmount;
		playerMPMaxText.text = playerMPMaxOrig.ToString("F0");

		playerHPMaxOrig += hpAmount;
		playerHPMaxText.text = playerHPMaxOrig.ToString("F0");
	}

	public void UpdatePotionCount(int amount)
	{
		potionCountOrig += amount;
		potionText.text = potionCountOrig.ToString("F0");
	}

	public void UpdateObject4Count(int amount)
    {
        object4CountOrig += amount;

        textOBJ4.text = object4CountOrig.ToString("F0");
    }
}
