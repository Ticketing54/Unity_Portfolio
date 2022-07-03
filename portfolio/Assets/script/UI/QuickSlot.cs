using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : ItemSlot
{
    [SerializeField]
    Image CoolTime;


    public void SetCoolTime(float _percent)
    {
        CoolTime.fillAmount = 1-_percent;
        
    }
    public override void Clear()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        CoolTime.fillAmount = 0;

        if (ItemCount.gameObject.activeSelf == true)
            ItemCount.gameObject.SetActive(false);
    }
}
