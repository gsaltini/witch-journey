using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class  UISkillTree: MonoBehaviour
{
    public GameObject player;
    public static UISkillTree Instance;
    public GameObject fireSkillLevelBtn;
    public GameObject fireSkillLevelText;

    public GameObject fireSkillCDBtn;
    public GameObject fireSkillCDText;

    public GameObject fireUltimateLevelBtn;
    public GameObject fireUltimateLevelText;

    public GameObject fireUltimateCDBtn;
    public GameObject fireUltimateCDText;

    public GameObject waterSkillLevelBtn;
    public GameObject waterSkillLevelText;


    public GameObject waterSkillCDBtn;
    public GameObject waterSkillCDText;

    public GameObject waterUltimateLevelBtn;
    public GameObject waterUltimateLevelText;

    public GameObject waterUltimateCDBtn;
    public GameObject waterUltimateCDText;


    public GameObject airSkillLevelBtn;
    public GameObject airSkillLevelText;

    public GameObject airSkillCDBtn;
    public GameObject airSkillCDText;

    public GameObject airUltimateLevelBtn;
    public GameObject airUltimateLevelText;

    public GameObject airUltimateCDBtn;
    public GameObject airUltimateCDText;


    public GameObject earthSkillLevelBtn;
    public GameObject earthSkillLevelText;

    public GameObject earthSkillCDBtn;
    public GameObject earthSkillCDText;

    public GameObject earthUltimateLevelBtn;
    public GameObject earthUltimateLevelText;

    public GameObject earthUltimateCDBtn;
    public GameObject earthUltimateCDText;
    
    public GameObject currentButtonGO;
    public GameObject SkillUpgradeButtonsParent;

    //public Button yesUpgrade;
    //public Button noUpgrade;
    
    public GameObject upgradePanel;
    public GameObject upgradeMessage;
    public TMP_Text numCoinsText;
    
    //private List<>
    UpgradeButton fireSkillLevel = new UpgradeButton("Fire", "Skill", "Level", 1);
    UpgradeButton fireSkillCD = new UpgradeButton("Fire", "Skill", "Cooldown", 7);
    //UpgradeButton fireSkillCD = new UpgradeButton("Fire", "Skill", "Cooldown", 2);

    UpgradeButton waterSkillLevel = new UpgradeButton("Water", "Skill", "Level", 1);
    UpgradeButton waterSkillCD = new UpgradeButton("Water", "Skill", "Cooldown", 7);
    //UpgradeButton waterSkillCD = new UpgradeButton("Water", "Skill", "Cooldown", 3);

    UpgradeButton airSkillLevel = new UpgradeButton("Air", "Skill", "Level", 1);
    UpgradeButton airSkillCD = new UpgradeButton("Air", "Skill", "Cooldown", 7);
    //UpgradeButton airSkillCD = new UpgradeButton("Air", "Skill", "Cooldown", 1);

    UpgradeButton earthSkillLevel = new UpgradeButton("Earth", "Skill", "Level", 1);
    UpgradeButton earthSkillCD = new UpgradeButton("Earth", "Skill", "Cooldown", 7);
    //UpgradeButton earthSkillCD = new UpgradeButton("Earth", "Skill", "Cooldown", 5);



    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //UpgradeButton fireSkillLevel = new UpgradeButton("Fire", "Skill", "Level", 0);
        UIUpgradePanel.setPlayer(player);
       
    }


    void OnEnable()
    {
        fireSkillLevelBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(fireSkillLevelBtn));
        fireSkillCDBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(fireSkillCDBtn));
        waterSkillLevelBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(waterSkillLevelBtn));
        waterSkillCDBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(waterSkillCDBtn));
        airSkillLevelBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(airSkillLevelBtn));
        airSkillCDBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(airSkillCDBtn));
        earthSkillLevelBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(earthSkillLevelBtn));
        earthSkillCDBtn.GetComponent<Button>().onClick.AddListener(() => Upgrade(earthSkillCDBtn));


    }

    private void Upgrade(GameObject buttonPressed) 
    {
        //UpgradePanel upgradePanelObj = new UpgradePanel();

        if (buttonPressed == fireSkillLevelBtn)
        {
            UpdateUpgradeMessage(fireSkillLevel);
            UIUpgradePanel.setUpgradeButton(fireSkillLevel);
            UIUpgradePanel.setUBText(fireSkillLevelText);
            currentButtonGO = fireSkillLevelBtn;
        }

        if (buttonPressed == fireSkillCDBtn)
        {
            UpdateUpgradeMessage(fireSkillCD);
            UIUpgradePanel.setUpgradeButton(fireSkillCD);
            UIUpgradePanel.setUBText(fireSkillCDText);
            currentButtonGO = fireSkillCDBtn;

        }

        if (buttonPressed == waterSkillLevelBtn)
        {
            UpdateUpgradeMessage(waterSkillLevel);
            UIUpgradePanel.setUpgradeButton(waterSkillLevel);
            UIUpgradePanel.setUBText(waterSkillLevelText);
            currentButtonGO = waterSkillLevelBtn;

        }

        if (buttonPressed == waterSkillCDBtn)
        {
            UpdateUpgradeMessage(waterSkillCD);
            UIUpgradePanel.setUpgradeButton(waterSkillCD);
            UIUpgradePanel.setUBText(waterSkillCDText);
            currentButtonGO = waterSkillCDBtn;

        }

        if (buttonPressed == airSkillLevelBtn)
        {
            UpdateUpgradeMessage(airSkillLevel);
            UIUpgradePanel.setUpgradeButton(airSkillLevel);
            UIUpgradePanel.setUBText(airSkillLevelText);
            currentButtonGO = airSkillLevelBtn;

        }

        if (buttonPressed == airSkillCDBtn)
        {
            UpdateUpgradeMessage(airSkillCD);
            UIUpgradePanel.setUpgradeButton(airSkillCD);
            UIUpgradePanel.setUBText(airSkillCDText);
            currentButtonGO = airSkillCDBtn;

        }

        if (buttonPressed == earthSkillLevelBtn)
        {
            UpdateUpgradeMessage(earthSkillLevel);
            UIUpgradePanel.setUpgradeButton(earthSkillLevel);
            UIUpgradePanel.setUBText(earthSkillLevelText);
            currentButtonGO = earthSkillLevelBtn;

        }

        if (buttonPressed == earthSkillCDBtn)
        {
            UpdateUpgradeMessage(earthSkillCD);
            UIUpgradePanel.setUpgradeButton(earthSkillCD);
            UIUpgradePanel.setUBText(earthSkillCDText);
            currentButtonGO = earthSkillCDBtn;

        }

        //OpenUpgradePanel();
    }

    public void UpdateUpgradeMessage(UpgradeButton ub)
    {
        string tempMessage = "NO MESSAGE";
        if (ub.bottomText == "Level")
        {
            tempMessage = "Would you like to upgrade " + ub.element + " " + ub.attackType + " to " +
               ub.bottomText + " " + (ub.number + 1) + "?";
        }
        else if (ub.bottomText == "Cooldown")
        {
            tempMessage = "Would you like to reduce " + ub.element + " " + ub.attackType + " cooldown to " +
                +(ub.number - 1) + " seconds ?";
        }

        upgradeMessage.GetComponent<TMP_Text>().text = tempMessage;


    }
    public void OpenUpgradePanel()
    {
        if (upgradePanel != null)
        {
            bool isActive = upgradePanel.activeSelf;
            upgradePanel.SetActive(!isActive);
        }
    }

    public void UpdateNumCoinsText()
    {
        numCoinsText.text = "x" + InventoryManager.Instance.numUpgradeCoins; 
    }

    public void MaxUpgrade()
    {
        /*var tempArray = SkillUpgradeButtonsParent.GetComponentInChildren<Button>().gameObject.name;
        foreach (string name in tempArray)
        {

        }*/
        currentButtonGO.GetComponent<Button>().interactable = false;
    }

    /*
    public static void UpdateUI(GameObject buttonText, UpgradeButton button)
    {
        buttonText.GetComponent<TMP_Text>().text = button.number.ToString();

    }*/

    void OnDisable()
    {
        //Un-Register Button Events
        fireSkillLevelBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        fireSkillCDBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        //yesUpgrade.onClick.RemoveAllListeners();
    }

}
