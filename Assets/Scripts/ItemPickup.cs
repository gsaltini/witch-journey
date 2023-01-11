using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Code heavily inspired by this tutorial: https://www.youtube.com/watch?v=AoD_F1fSFFg

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    public void PickUp()
    {
        //Debug.Log("Picking up " + Item.name);
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    
    /*private void OnMouseDown()
    {
        PickUp();
    }
    */
}
