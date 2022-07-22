using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : ItemSlot
{
    [SerializeField]
    Image coolTime;
    public override void Add(string _SpriteName)
    {
        Clear();
        base.Add(_SpriteName);        
    }

    public void SetCoolTime(float _percent)
    {
        coolTime.fillAmount = 1-_percent;
        
    }
    public override void Clear()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        coolTime.fillAmount = 0;

        if (ItemCount.gameObject.activeSelf == true)
            ItemCount.gameObject.SetActive(false);
    }
}
