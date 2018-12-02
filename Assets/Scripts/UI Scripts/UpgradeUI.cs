using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeUI : MonoBehaviour {

    [SerializeField]
    GameObject upgradePanel;
    [SerializeField]
    GameObject successPanel;

    [SerializeField]
    GameObject successPanelYes;


    //health upgrade panel
    [SerializeField]
    GameObject shipHealthPanel;
    [SerializeField]
    GameObject shipHealthButton1;
    [SerializeField]
    Button ammoButton1;
    [SerializeField]
    Button shipMovementButton1;
    [SerializeField]
    GameObject health1Upgrade;
    [SerializeField]
    Text health1Price;
    [SerializeField]
    GameObject health2Upgrade;
    [SerializeField]
    Text health2Price;
    [SerializeField]
    GameObject health3Upgrade;
    [SerializeField]
    Text health3Price;



    //ammo panel
    [SerializeField]
    GameObject ammoPanel;
    [SerializeField]
    Button shipHealthButton2;
    [SerializeField]
    GameObject ammoButton2;
    [SerializeField]
    Button shipMovementButton2;
    [SerializeField]
    GameObject ammo1Upgrade;
    [SerializeField]
    Text ammo1Price;
    [SerializeField]
    GameObject ammo2Upgrade;
    [SerializeField]
    Text ammo2Price;



    //shipmovement panel
    [SerializeField]
    GameObject shipMovementPanel;
    [SerializeField]
    Button shipHealthButton3;
    [SerializeField]
    Button ammoButton3;
    [SerializeField]
    GameObject shipMovementButton3;
    [SerializeField]
    GameObject movement1Upgrade;
    [SerializeField]
    Text movement1Price;
    [SerializeField]
    GameObject movement2Upgrade;
    [SerializeField]
    Text movement2Price;






    [SerializeField]
    GameObject confirmationPanel;

    [SerializeField]
    GameObject yesButton;

    [SerializeField]
    Button noButton;

    [SerializeField]
    Text moneyText;


    public float upgradeWindowDelay;
    public float successWindowDelay;


    public static bool startNextLevel;
    private GameManager gameManager;
    private HealthSlider hp;
    private StationNav stationNav;
    private float upgardeWindowTimer = 0;
    private float successWindowTimer = 0;
    private bool canDisplayUpgrade=true;

    // Use this for initialization
    void Start () {
        startNextLevel = false;
        shipHealthPanel.SetActive(true);
        ammoPanel.SetActive(false);
        shipMovementPanel.SetActive(false);
        shipHealthButton1.GetComponent<Button>().interactable = false;
        confirmationPanel.SetActive(false);
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        if (gameManagerObject)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
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

        health1Price.text = movement1Price.text = ammo1Price.text = GameManager.priceList[0].ToString();
        health2Price.text = movement2Price.text = ammo2Price.text = GameManager.priceList[1].ToString();
        health3Price.text = GameManager.priceList[2].ToString();
        SetButtonInteactive();
    }

    // Update is called once per frame
    void Update () {
        moneyText.text = GameManager.money.ToString();
        if (!(gameManager.HelloGrandma()) && hp.LevelEnded && !upgradePanel.activeSelf&&canDisplayUpgrade)
        {
            if(upgardeWindowTimer>=upgradeWindowDelay)
            {
                activateUpgradePanel();
                upgardeWindowTimer = 0;
            }
            else
            {
                upgardeWindowTimer += Time.deltaTime;
            }
        }
        else if((gameManager.HelloGrandma()) && hp.BossEnded && !successPanel.activeSelf)
        {
            if (successWindowTimer >= successWindowDelay)
            {
                activateSuccessPanel();
                successWindowTimer = 0;
            }
            else
            {
                successWindowTimer += Time.deltaTime;
            }
        }
    }

    private void activateUpgradePanel()
    {
        upgradePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(shipHealthButton1, null);

    }

    private void activateSuccessPanel()
    {
        successPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(successPanelYes, null);
    }

    public void shipHealthPanelActive()
    {
        if(!shipHealthPanel.activeSelf)
        {
            shipHealthPanel.SetActive(true);
            ammoPanel.SetActive(false);
            shipMovementPanel.SetActive(false);
            shipHealthButton1.GetComponent<Button>().interactable = false;
            EventSystem.current.SetSelectedGameObject(shipHealthButton1, null);
            ammoButton1.interactable = true;
            shipMovementButton1.interactable = true;
        }
    }
    public void ammoPanelActive()
    {
        if (!ammoPanel.activeSelf)
        {
            ammoPanel.SetActive(true);
            shipHealthPanel.SetActive(false);
            shipMovementPanel.SetActive(false);
            ammoButton2.GetComponent<Button>().interactable = false;
            EventSystem.current.SetSelectedGameObject(ammoButton2, null);
            shipHealthButton2.interactable = true;
            shipMovementButton2.interactable = true;
        }
    }


    public void shipMovementPanelActive()
    {
        if (!shipMovementPanel.activeSelf)
        {
            shipMovementPanel.SetActive(true);
            ammoPanel.SetActive(false);
            shipHealthPanel.SetActive(false);
            shipMovementButton3.GetComponent<Button>().interactable = false;
            EventSystem.current.SetSelectedGameObject(shipMovementButton3, null);
            shipHealthButton3.interactable = true;
            ammoButton3.interactable = true;
        }
    }

    
    public void closeUpgradePanel()
    {
        upgradePanel.SetActive(false);
        confirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(yesButton, null);
        canDisplayUpgrade = false;
    }

    public void GoToMainMenu()
    {
		GameObject menuManager = GameObject.FindGameObjectWithTag ("MenuManager");
		if (menuManager != null) 
		{
			MenuManager menuManagerScript = menuManager.GetComponent<MenuManager> ();
			menuManagerScript.PlayMenuSel ();
			menuManagerScript.StartMenuMusic ();	
		}
        SceneManager.LoadScene("MainMenu");
    }

    public void YesButtonClicked()
    {
        startNextLevel = true;
        confirmationPanel.SetActive(false);
        //upgradePanel.SetActive(false);
    }

    public void NoButtonClicked()
    {
        confirmationPanel.SetActive(false);
        upgradePanel.SetActive(true);
        shipHealthPanel.SetActive(true);
        ammoPanel.SetActive(false);
        shipMovementPanel.SetActive(false);
        canDisplayUpgrade = true;
        EventSystem.current.SetSelectedGameObject(shipHealthButton1, null);

    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void healthUpdate1()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[0])
        {
            health1Upgrade.GetComponent<Button>().navigation = nonNav;
            health1Upgrade.GetComponent<Button>().interactable = false;
            health2Upgrade.GetComponent<Button>().interactable = true;
            EventSystem.current.SetSelectedGameObject(health2Upgrade, null);
            hp.UpgradeMaxHealth(GameManager.healthUpgradeValue[0]);
            gameManager.spendCoin(GameManager.priceList[0]);
			PlayCoinSound ();
            GameManager.healthUpgradeAvailability[0] = false;
            GameManager.healthUpgradeAvailability[1] = true;

        }
    }

    public void healthUpdate2()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[1])
        {
            health2Upgrade.GetComponent<Button>().navigation = nonNav;
            health2Upgrade.GetComponent<Button>().interactable = false;
            health3Upgrade.GetComponent<Button>().interactable = true;
            EventSystem.current.SetSelectedGameObject(health3Upgrade, null);
            hp.UpgradeMaxHealth(GameManager.healthUpgradeValue[1]);
            gameManager.spendCoin(GameManager.priceList[1]);
			PlayCoinSound ();
            GameManager.healthUpgradeAvailability[1] = false;
            GameManager.healthUpgradeAvailability[2] = true;
        }
    }


    public void healthUpdate3()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[2])
        {
            health3Upgrade.GetComponent<Button>().navigation = nonNav;
            health3Upgrade.GetComponent<Button>().interactable = false;
            EventSystem.current.SetSelectedGameObject(shipHealthButton1, null);
            hp.UpgradeMaxHealth(GameManager.healthUpgradeValue[2]);
            gameManager.spendCoin(GameManager.priceList[2]);
			PlayCoinSound ();
            GameManager.healthUpgradeAvailability[2] = false;
        }
    }

    public void ammoUpgrade1()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[0])
        {
            ammo1Upgrade.GetComponent<Button>().navigation = nonNav;
            ammo1Upgrade.GetComponent<Button>().interactable = false;
            ammo2Upgrade.GetComponent<Button>().interactable = true;
            EventSystem.current.SetSelectedGameObject(ammo2Upgrade, null);
            GameManager.ammoTypeUnlocked[0] = true;
            gameManager.spendCoin(GameManager.priceList[0]);
			PlayCoinSound ();
            GameManager.ammoUpgradeAvailability[0] = false;
            GameManager.ammoUpgradeAvailability[1] = true;
        }
    }

    public void ammoUpgrade2()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[1])
        {
            ammo2Upgrade.GetComponent<Button>().navigation = nonNav;
            ammo2Upgrade.GetComponent<Button>().interactable = false;
            EventSystem.current.SetSelectedGameObject(ammoButton2, null);
            GameManager.ammoTypeUnlocked[1] = true;
            gameManager.spendCoin(GameManager.priceList[1]);
			PlayCoinSound ();
            GameManager.ammoUpgradeAvailability[1] = false;
        }
    }


    public void movementUpdate1()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[0])
        {
            movement1Upgrade.GetComponent<Button>().navigation = nonNav;
            movement1Upgrade.GetComponent<Button>().interactable = false;
            movement2Upgrade.GetComponent<Button>().interactable = true;
            EventSystem.current.SetSelectedGameObject(movement2Upgrade, null);
            stationNav.UpgradeTorque(GameManager.torqueIncrease);
            gameManager.spendCoin(GameManager.priceList[0]);
			PlayCoinSound ();
            GameManager.movementUpgradeAvailability[0] = false;
            GameManager.movementUpgradeAvailability[1] = true;
        }
    }



    public void movementUpgrade2()
    {
        Navigation nonNav = new Navigation();
        nonNav.mode = Navigation.Mode.None;
        if (GameManager.money >= GameManager.priceList[1])
        {
            movement2Upgrade.GetComponent<Button>().navigation = nonNav;
            movement2Upgrade.GetComponent<Button>().interactable = false;
            EventSystem.current.SetSelectedGameObject(shipMovementButton3, null);
            hp.UpGradeInvulnerability(GameManager.invulnerabilityTimerIncrease);
            gameManager.spendCoin(GameManager.priceList[1]);
			PlayCoinSound ();
            GameManager.movementUpgradeAvailability[1] = false;
        }
    }


    private void SetButtonInteactive()
    {
        health1Upgrade.GetComponent<Button>().interactable = GameManager.healthUpgradeAvailability[0];
        health2Upgrade.GetComponent<Button>().interactable = GameManager.healthUpgradeAvailability[1];
        health3Upgrade.GetComponent<Button>().interactable = GameManager.healthUpgradeAvailability[2];
        ammo1Upgrade.GetComponent<Button>().interactable = GameManager.ammoUpgradeAvailability[0];
        ammo2Upgrade.GetComponent<Button>().interactable = GameManager.ammoUpgradeAvailability[1];
        movement1Upgrade.GetComponent<Button>().interactable = GameManager.movementUpgradeAvailability[0];
        movement2Upgrade.GetComponent<Button>().interactable = GameManager.movementUpgradeAvailability[1];
    }

	public void PlayCoinSound()
	{
		GameObject ExtraSFX = GameObject.FindGameObjectWithTag ("ExtraSFX");
		AudioSource[] audioSources = ExtraSFX.GetComponents<AudioSource> ();
		AudioSource coinSound = audioSources [2];
		coinSound.Play ();
	}


}
