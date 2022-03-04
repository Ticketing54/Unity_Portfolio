using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : UI_ItemSlots
{
    private void Awake()
    {
        itemListType = ITEMLISTTYPE.QUICK;
    }  

}
