using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillQuickSlot : Slot
{
    [SerializeField]
    TextMeshProUGUI count;
    [SerializeField]
    Image coolTime;
    public override void Add(string _SpriteName)
    {
        Clear();
        base.Add(_SpriteName);
    }
    public void SetCoolTime(float _percent, int _count)
    {
        coolTime.fillAmount = 1 - _percent;
        if(_count <= 0)
        {
            count.text = "";
        }
        else
        {
            count.text = _count.ToString() + "s"; 
        }        
    }
    public override void Clear()
    {
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        coolTime.fillAmount = 0;
        count.text = "";
    }
}
