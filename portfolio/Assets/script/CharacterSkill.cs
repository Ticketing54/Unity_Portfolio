using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill
{
    Dictionary<string, Skill> SkillList;
    Skill skill;
    public CharacterSkill()
    {
        SkillList = new Dictionary<string, Skill>();
    }
    public void Add(string _SkillName,Skill _NewSkill)
    {
        SkillList.Add(_SkillName, _NewSkill);
    }
    public Skill FindSKill(string _SkillName)
    {
        if(SkillList.TryGetValue(_SkillName,out skill))
        {
            return skill;
        }
        else
        {
            Debug.Log("스킬이 없습니다.");
            return null;
        }
    }
}
