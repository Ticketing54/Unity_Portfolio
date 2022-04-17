using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UI_Inventory : UI_ItemSlots
{
    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }
    private void Awake()
    {
        itemListType = ITEMLISTTYPE.INVEN;
    }



}