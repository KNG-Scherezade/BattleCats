using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static int money = 1000;

    public static int coinValue = 20;

    public static int[] priceList = { 100, 200, 300 };

    public static bool[] healthUpgradeAvailability = { true, false, false };
    public static int[] healthUpgradeValue = { 100, 200, 300 };

    public static bool[] ammoUpgradeAvailability = { true, false };
    public static bool[] ammoTypeUnlocked = { false, false };

    //1.invulnerabilityTimerIncrease 2. torque
    public static bool[] movementUpgradeAvailability = { true, false };
    public static float invulnerabilityTimerIncrease = 0.5f;
    public static float torqueIncrease = 1.0f;
    
    [SerializeField]
    GameObject pausePanel;

    private HealthSlider hp;
    private StationNav stationNav;
    private bool isPaused = false;

    //DEBUGUI
    [SerializeField]
    Text coinText;

    [SerializeField]
    private float m_timeBeforeRestart = 4.0f;

    private Dictionary<string, string> nextLevels = new Dictionary<string, string>();

    public enum CauseOfDeath
    {
        Fire,
        Enemy,
        Water
    }
    public GameObject m_restartPanel;
    private Dictionary<CauseOfDeath, List<string>> m_randomTexts = new Dictionary<CauseOfDeath, List<string>>();


    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this);

        nextLevels.Add("Level0", "Level1");
        nextLevels.Add("Level1", "Level2a");
        nextLevels.Add("Level2b", "Level3");
    }

    private void Start()
    {
        GameObject hpObject = GameObject.FindGameObjectWithTag("YarnPhysics");
        if (hpObject)
        {
            hp = hpObject.GetComponent<HealthSlider>();
        }
        GameObject navGameObject = GameObject.FindGameObjectWithTag("NavigationStation");
        if (navGameObject)
        {
            stationNav = navGameObject.GetComponent<StationNav>();
        }
        else
        {
            Debug.LogError("Cannot find Navigation Station!");
        }

        m_randomTexts.Add(CauseOfDeath.Fire, new List<string> {
            "Toasty in here, isn't it?",
            "Did someone leave the oven on?"});

        m_randomTexts.Add(CauseOfDeath.Enemy, new List<string> {
            "Hey, now isn't the time to take a nap!",
            "You knead to focus!"});

        m_randomTexts.Add(CauseOfDeath.Water, new List<string> {
            "Was that water?! Aghhh!",
            "Did you know cats liked baths?\n\nSunbathing, at least."});

        // more generic messages
        foreach (KeyValuePair<CauseOfDeath, List<string>> kv in m_randomTexts)
        {
            kv.Value.AddRange(new List<string> {
                "Oh no, you passed away!\n\nGood thing cats have nine lives...",
                "That's not what the \"No littering\" sign meant!" });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pausePanel)
        {
            pausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }

        startNextLevel();
        //coinText.text = "Coin: " + money;
    }

    public void ResetGameState()
    {
        money = 1000;

        coinValue = 20;

        healthUpgradeAvailability[0] = true; // { true, false, false }
        healthUpgradeAvailability[1] = false;
        healthUpgradeAvailability[2] = false;

        ammoUpgradeAvailability[0] = true; // { true, false }
        ammoUpgradeAvailability[1] = false;
        ammoTypeUnlocked[0] = false;  // { false, false }
        ammoTypeUnlocked[1] = false;

        movementUpgradeAvailability[0] = true; //{ true, false }
        movementUpgradeAvailability[0] = false;

        hp.DealMaxHeal(); // reset yarn health
    }

    public IEnumerator RestartLevel(CauseOfDeath cause)
    {
        int scene = SceneManager.GetActiveScene().buildIndex;

        if (m_restartPanel == null)
        {
            m_restartPanel = GameObject.Find("Canvas").transform.Find("RestartLevelPanel").gameObject;
        }

        if (!m_restartPanel.activeInHierarchy)
        {
            int randomChoice = Random.Range(0, m_randomTexts[cause].Count);

            m_restartPanel.SetActive(true);
            m_restartPanel.GetComponent<RestartPanel>().DisplayWithText(m_randomTexts[cause][randomChoice]);

            yield return new WaitForSeconds(m_timeBeforeRestart);

            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// #Issue 41:
    /// Return the value indicates whether this is the end of the game
    /// But assume not extra scene after the last level scene
    /// </summary>
    /// <returns></returns>
    public bool HelloGrandma()
    {
        return SceneManager.GetActiveScene().name == "boss";
    }

    public void startNextLevel()
    {
        if (UpgradeUI.startNextLevel)
        {
            isPaused = false;

            string nextLevel = nextLevels[SceneManager.GetActiveScene().name];
            Debug.Log("start next level " + nextLevel);
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void getCoin(int v)
    {
        money += v;
    }

    public void spendCoin(int v)
    {
        money -= v;
    }

    public void UpgradeAmmo()
    {
        int index = 0;
        for (int i = 0; i < ammoUpgradeAvailability.Length; i++)
        {
            if (ammoUpgradeAvailability[i])
            {
                index = i;
                break;
            }
        }
        if (money >= priceList[index])
        {
            ammoTypeUnlocked[index] = true;
            spendCoin(priceList[index]);
			PlayCoinSound ();
            ammoUpgradeAvailability[index] = false;
            if (index + 1 < ammoUpgradeAvailability.Length)
            {
                ammoUpgradeAvailability[index + 1] = true;
            }
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY!");
        }
    }

    public void UpgradeMovement1()
    {
        int index = 0;
        for (int i = 0; i < movementUpgradeAvailability.Length; i++)
        {
            if (movementUpgradeAvailability[i])
            {
                index = i;
                break;
            }
        }
        if (money >= priceList[index])
        {
            spendCoin(priceList[index]);
			PlayCoinSound ();
            movementUpgradeAvailability[index] = false;
            stationNav.UpgradeTorque(torqueIncrease);
            if (index + 1 < movementUpgradeAvailability.Length)
            {
                movementUpgradeAvailability[index + 1] = true;
            }
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY!");
        }
    }

    public void UpgradeMovement2()
    {
        int index = 0;
        for (int i = 0; i < movementUpgradeAvailability.Length; i++)
        {
            if (movementUpgradeAvailability[i])
            {
                index = i;
                break;
            }
        }
        if (money >= priceList[index])
        {
            spendCoin(priceList[index]);
			PlayCoinSound ();
            //call upgrade invu.. function
            hp.UpGradeInvulnerability(invulnerabilityTimerIncrease);
            movementUpgradeAvailability[index] = false;
            if (index + 1 < movementUpgradeAvailability.Length)
            {
                movementUpgradeAvailability[index + 1] = true;
            }     
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY!");
        }
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

    public void closePauseWindow()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
		GameObject menuManager = GameObject.FindGameObjectWithTag ("MenuManager");
		if (menuManager != null) 
		{
			MenuManager menuManagerScript = menuManager.GetComponent<MenuManager> ();
			menuManagerScript.StartMenuMusic ();	
		}

        ResetGameState();
        SceneManager.LoadScene("MainMenu");
    }

	public void PlayCoinSound()
	{
		GameObject ExtraSFX = GameObject.FindGameObjectWithTag ("ExtraSFX");
		AudioSource[] audioSources = ExtraSFX.GetComponents<AudioSource> ();
		AudioSource coinSound = audioSources [2];
		coinSound.Play ();
	}

}
