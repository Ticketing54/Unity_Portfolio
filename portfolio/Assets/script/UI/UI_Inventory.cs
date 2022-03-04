using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UI_Inventory : UI_ItemSlots
{
    private void Awake()
    {
        itemListType = ITEMLISTTYPE.INVEN;
    }



}