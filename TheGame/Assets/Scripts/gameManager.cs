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
}
