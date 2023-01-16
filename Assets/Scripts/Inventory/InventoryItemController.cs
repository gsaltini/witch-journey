using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Code heavily inspired by this tutorial: https://www.youtube.com/watch?v=AoD_F1fSFFg
//this is for removing items? like the close button

public class InventoryItemController : MonoBehaviour
{
    Item item;
    //public Button RemoveButton;
    GameObject Player;
    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        
        if (InventoryManager.Instance.ItemAmounts.TryGetValue(item, out int amount) == false)
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void setPlayer(GameObject _player)
    {
        Player = _player;
    }
    
    public Item getItem()
    {
        return item;
    }

    public void UseItem()
    {
        Debug.Log("im in UseItem()");

        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                Player.GetComponent<PlayerMovement>().addHealth(item.value);
                //Player.GEtPlayerMovement.addHealth(item.value);
                InventoryManager.Instance.HealPowerBar(item.value);
                Debug.Log("using heatlth potion");

                break;
            case Item.ItemType.Key:
                //Player.Instance.IncreaseHealth(item.value);
                Debug.Log("using key");
                break;
        }
        RemoveItem();
    }
}
