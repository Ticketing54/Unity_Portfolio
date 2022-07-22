using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SkillSlot : Slot
{   
    [SerializeField]
    Image posLev;
    [SerializeField]
    Image notLearn;


    void SetSkillSlot(Skill _skill)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = ResourceManager.resource.GetImage(_skill.spriteName);
    }
    public override void Add(string _SpriteName)
    {
        base.Add(_SpriteName);
    }

    public void SetHaveSkill()
    {
        posLev.gameObject.SetActive(false);
        SetImageAlpha(true);
    }
    public void Setlearn()
    {
        posLev.gameObject.SetActive(true);
        SetImageAlpha(false);
    }
    public void SetCantLearn()
    {
        posLev.gameObject.SetActive(false);
        SetImageAlpha(false);
    }
    void SetImageAlpha(bool _possable)
    {
        Color color = notLearn.color;
        if (_possable)
        {
            color.a = 0f;
        }
        else
        {
            color.a = 0.5f;
        }
        
        notLearn.color = color;
    }
    public override void Clear()
    {
        base.Clear();
        posLev.gameObject.SetActive(false);
        SetImageAlpha(false);        
    }


}
