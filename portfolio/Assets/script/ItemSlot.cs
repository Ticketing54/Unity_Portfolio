using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : Slot
{
    [SerializeField]
    int SlotNum;
    [SerializeField]
    Item item = null;
    [SerializeField]
    Text ItemCount;
    public void Add(ItemListType _Type)
    {
        switch (_Type)
        {
            case ItemListType.INVEN:
                item = Character.Player.Inven.GetItem(SlotNum);
                break;
            case ItemListType.QUICK:
                item = Character.Player.Quick.GetItem(SlotNum);
                break;
            case ItemListType.EQUIP:
                item = Character.Player.Equip.GetItem(SlotNum);
                break;
            default:
                break;
        }
        
        if (item == null)
        {
            Clear();
            return;
        }            
        Icon.gameObject.SetActive(true);
        Icon.sprite = GameManager.gameManager.GetSprite(item.itemSpriteName);

        if (item.ItemCount > 1)
        {
            ItemCount.gameObject.SetActive(true);
            ItemCount.text = item.ItemCount.ToString();
        }
        else
        {
            if (ItemCount.gameObject.activeSelf == true)
                ItemCount.gameObject.SetActive(false);
        }
    }

    public void Clear()
    {
        item = null;
        Icon.sprite = null;
        Icon.gameObject.SetActive(false);

        if(ItemCount.gameObject.activeSelf==true)
            ItemCount.gameObject.SetActive(false);
    }

}
