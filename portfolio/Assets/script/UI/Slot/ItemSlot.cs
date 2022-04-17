using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : Slot
{
    [SerializeField]
    Text ItemCount;
    public void Add(string _SpriteName, int _ItemCount,int _itemIndex)
    {
        itemIndex = _itemIndex;
        if (_SpriteName == string.Empty)
        {
            Clear();
            return;
        }
        icon.gameObject.SetActive(true);
        icon.sprite = ResourceManager.resource.GetImage(_SpriteName);        

        if (_ItemCount > 1)
        {
            ItemCount.gameObject.SetActive(true);
            ItemCount.text = _ItemCount.ToString();
        }
        else
        {
            if (ItemCount.gameObject.activeSelf == true)
                ItemCount.gameObject.SetActive(false);
        }
    }
    
    public override void Clear()
    {        
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        itemIndex = -1;

        if(ItemCount.gameObject.activeSelf==true)
            ItemCount.gameObject.SetActive(false);
    }

}
