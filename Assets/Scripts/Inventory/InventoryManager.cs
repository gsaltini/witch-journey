using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//Code heavily inspired by this tutorial: https://www.youtube.com/watch?v=AoD_F1fSFFg


public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static InventoryManager Instance;
    public int numUpgradeCoins;
    public List<Item> Items = new List<Item>();
    public Dictionary<Item, int> ItemAmounts = new Dictionary<Item, int>();
    public GameObject Player;
    public Transform ItemContent;
    public GameObject InventoryItem;
    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;
    public UIUpgradePanel UIUpgradePanel;
    public Item upgradeCoinItem;
    public UISkillTree UISkillTree;
    public PowerBar PowerBar;

    private void Awake()
    {
        Instance = this;
        numUpgradeCoins = 0;
        UIUpgradePanel.updateCurrentUpgradeCoins(numUpgradeCoins);
    }

    public void Add(Item item)
    {
        if (item.itemType.Equals(Item.ItemType.UpgradeCoin))
        {
            numUpgradeCoins++;
            UIUpgradePanel.updateCurrentUpgradeCoins(numUpgradeCoins);
            Debug.Log("its is upgrade coin");
            upgradeCoinItem = item;
            UIUpgradePanel.yesUpgrade.GetComponent<Button>().interactable = true;
            //UIUpgradePanel.insufficientUCsText.SetActive(false);
            UISkillTree.UpdateNumCoinsText();

        }
        if (ItemAmounts.TryGetValue(item, out int amount))
        {
          
            ItemAmounts[item]++;

        }
        else
        {
            Items.Add(item);
            ItemAmounts.Add(item, 1);

        }

        PrintInventory();
    }

    //returns index if item already exists, returns -1 if not
    public int Contains(Item item)
    {
        int index = 0;
        foreach (Item i in Items)
        {
            if (i == item)
            {
                return index;
            }
            index++;

        }

        return -1;
    }

    public void UpdateItemAmount(Item item)
    {
        foreach (Transform i in ItemContent)
        {
            var iicItem = i.GetComponent<InventoryItemController>().getItem();
            if (iicItem.itemName == item.itemName)
            {
                var tempItemNumber = i.Find("ItemNumber").GetComponent<Text>();
                tempItemNumber.text = "x" + ItemAmounts[item].ToString();
            }

            //Destroy(item.gameObject);
        }
    }

    public void Remove(Item item)
    {
        ItemAmounts[item] = ItemAmounts[item] - 1;

        if (ItemAmounts[item] > 0)
        {
            //ListItem();
            UpdateItemAmount(item);
            //decrement number
        }
        else
        {
            Items.Remove(item);
            ItemAmounts.Remove(item);

        }
    }


    
    public void cleanInventory()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
    }

    public void ListItem()
    {
        cleanInventory();
        
        foreach (var item in Items)
        {
            if (item.itemName != "Upgrade Coin")
            {
                //TMP_Text tmpugui; 
                GameObject obj = Instantiate(InventoryItem, ItemContent);

                //Debug.Log("Picking up " + Item.name);

                var itemName = obj.transform.Find("ItemName").GetComponent<Text>();

                // = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
                //Debug.Log("Printing item name" + tmpugui);


                var itemNumber = obj.transform.Find("ItemNumber").GetComponent<Text>();

                var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
                //var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();
                //Debug.Log(itemName);
                //Debug.Log(itemIcon);
                itemName.text = item.itemName;
                obj.name = itemName.text;
                //Debug.Log("itemName.text" + tmpugui.text);

                itemIcon.sprite = item.icon;
                itemNumber.text = "x" + ItemAmounts[item].ToString();


                if (itemName.text == "Upgrade Coin")
                {
                    obj.GetComponent<Button>().interactable = false;
                }

                obj.GetComponent<InventoryItemController>().AddItem(item);
                obj.GetComponent<InventoryItemController>().setPlayer(Player);


                /*if (EnableRemove.isOn)
                {
                    removeButton.gameObject.SetActive(true);
                }*/
            }
        }

      
        //SetInventoryItems();
        
    }

    public void EnableItemsRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach(Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }

        }
        else
        {
            foreach(Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
            InventoryItems[i].setPlayer(Player);

        }

    }

    public void getNumCoins()
    {

    }
    public void useUpgradeCoin()
    {
        var temp = ItemAmounts[upgradeCoinItem] = ItemAmounts[upgradeCoinItem] - 1;
        numUpgradeCoins = temp;
        UIUpgradePanel.updateCurrentUpgradeCoins(temp);
        if (numUpgradeCoins <= 0)
        {
            UIUpgradePanel.yesUpgrade.GetComponent<Button>().interactable = false;
        }
        UISkillTree.UpdateNumCoinsText();
    }

    public void PrintInventory()
    {
        foreach (Item i in Items)
        {
            Debug.Log(i.itemName + " has " + ItemAmounts[i]);

        }
    }

    public void HealPowerBar(int amount)
    {
        PowerBar.Heal(amount);
    }
}

