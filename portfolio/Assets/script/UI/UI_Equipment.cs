using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UI_Equipment : UI_ItemSlots
{
    public override void SetItemMove()
    {
        itemMove = Character.Player.Equip;
    }

    
}
