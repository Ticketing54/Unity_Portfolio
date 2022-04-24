using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : UI_ItemSlots
{
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }
    private void Awake()
    {
        itemListType = ITEMLISTTYPE.QUICK;
    }  

}
