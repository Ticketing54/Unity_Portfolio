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
    public override void Add()
    {
        item = Character.Player.Inven.GetItem(SlotNum);
        if (item == null || item.itemType == Item.ItemType.None)
            return;
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

    public override void Clear()
    {
        item = null;
        Icon.sprite = null;
        Icon.gameObject.SetActive(false);

        if(ItemCount.gameObject.activeSelf==true)
            ItemCount.gameObject.SetActive(false);
    }

}
