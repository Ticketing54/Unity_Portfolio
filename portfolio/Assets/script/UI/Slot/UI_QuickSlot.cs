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
    public override void UpdateAllSlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {

            Item quickItem = character.quickSlot.GetItem(i);
            if(quickItem == null)
            {
                itemSlots[i].Clear();
            }
            else
            {
                itemSlots[i].Add(quickItem.itemSpriteName, quickItem.ItemCount, quickItem.index);
            }

        }
    }
}
