using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : Slot
{
    [SerializeField]
    int SlotNum;    
    [SerializeField]
    Text ItemCount;
    public void Add(string _SpriteName, int? _ItemCount)
    {
        if (_SpriteName == string.Empty)
        {
            Clear();
            return;
        }
        Icon.gameObject.SetActive(true);
        Icon.sprite = GameManager.gameManager.resource.GetImage(_SpriteName);        

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
        Icon.sprite = null;
        Icon.gameObject.SetActive(false);

        if(ItemCount.gameObject.activeSelf==true)
            ItemCount.gameObject.SetActive(false);
    }

}
