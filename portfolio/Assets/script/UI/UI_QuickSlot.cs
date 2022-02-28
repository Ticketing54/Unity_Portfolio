using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_QuickSlot : UI_ItemSlots
{
    public override void UpdateItemSlots()
    {
        StartCoroutine(UpdateQuickSlot());
    }
    public override void SetItemMove()
    {
        itemMove = Character.Player.QuickSlot;
    }

    IEnumerator UpdateQuickSlot()
    {
        yield return new WaitForEndOfFrame();


        if(itemMove == null)
        {
            SetItemMove();
        }

        for (int itemIndex = 0; itemIndex < Count - 1; itemIndex++)
        {
            ItemSlots[itemIndex].Add(itemMove.GetImage(itemIndex), itemMove.GetItemCount(itemIndex));
        }
    }
}
