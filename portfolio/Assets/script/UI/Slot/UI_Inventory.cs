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
        UIManager.uimanager.AOpenInventoryUi += () => gameObject.SetActive(true);
        UIManager.uimanager.ACloseInventoryUi += () => gameObject.SetActive(false);                
        gameObject.SetActive(false);
    }
    public override void UpdateAllSlot()
    {
        Item getitem;
        for (int itemSlotNum = 0; itemSlotNum < itemSlots.Length; itemSlotNum++)
        {
            getitem = GameManager.gameManager.character.ItemList_GetItem(itemListType, itemSlotNum);
            if (getitem == null)
            {
                itemSlots[itemSlotNum].Clear();
            }
            else
            {
                itemSlots[itemSlotNum].Add(getitem.itemSpriteName, getitem.ItemCount);
            }

            getitem = null;
        }
    }



}