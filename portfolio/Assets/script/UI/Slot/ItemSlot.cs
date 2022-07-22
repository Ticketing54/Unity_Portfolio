using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : Slot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Text ItemCount;
    public void Add(string _SpriteName, int _ItemCount)
    {   
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

        if(ItemCount.gameObject.activeSelf==true)
            ItemCount.gameObject.SetActive(false);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //UIManager.uimanager.OpenMiniInfo(itemIndex, eventData.position);
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //UIManager.uimanager.CloseMiniInfo();
        
    }

}
