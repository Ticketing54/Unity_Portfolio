using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : UI_ItemSlots
{
    public override void OnEnable()
    {
        UIManager.uimanager.itemoveEnd += this.LeftClickUp;
        UIManager.uimanager.updateUiSlot += this.UpdateSlot;
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
