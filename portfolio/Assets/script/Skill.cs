using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class Skill
{   
    public int Index;
    
    public SkillType skillType;
    public string skillName;
    public string Mana;
    public string skillExplain;        
    public string skillSpriteName;
    public int needLevel;
    public int cooltime;
    public int HoldTime;
    public string skill_ability;


    




    public Skill(int _Index, string _SkillType, string _SkillName, string _SkillSpriteName,string _mana ,string _SkillExplain,string _needLevel,string _cooltime,string _holdTime, string _ablity)
    {
        Index = _Index;
        skillType = (SkillType)Enum.Parse(typeof(SkillType), _SkillType);
        skillName = _SkillName;
        skillSpriteName = _SkillSpriteName;
        Mana = _mana;
        skillExplain = _SkillExplain;
        needLevel = int.Parse(_needLevel);
        cooltime = int.Parse(_cooltime);
        HoldTime = int.Parse(_holdTime);
        skill_ability = _ablity;        
    }

    public enum SkillType
    {
        Passive,
        Active,
        Buff,
        
    }













}
