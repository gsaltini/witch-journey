using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Code heavily inspired by this tutorial: https://www.youtube.com/watch?v=AoD_F1fSFFg

[CreateAssetMenu (fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public int value;
    public string itemName = "new item"; // using new keyword hides the object.name??
    //public TMP_Text itemNameTMP;
    public Sprite icon;
    public ItemType itemType;
    //public int number = 0;
   public enum ItemType
    {
        HealthPotion = 1,
        UpgradeCoin = 2,
        Key = 3
    }
}
