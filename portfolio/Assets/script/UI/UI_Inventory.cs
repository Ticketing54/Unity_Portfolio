using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UI_Inventory : UI_ItemSlots
{
    public override void OnEnable()
    {
        base.OnEnable();
        UIManager.uimanager.updateInven+= UpdateItemSlots;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        UIManager.uimanager.updateInven -= UpdateItemSlots;
    }

    
    

    public override void SetItemMove()
    {
        itemMove = Character.Player.Inven;
    }

}