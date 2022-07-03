using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[System.Serializable]
public class Skill
{
    public int index{ get; }    
    public SKILLTYPE skillType { get; }
    public string name { get; }
    public float mana { get; }
    public string explain { get; }      
    public string spriteName { get; }
    public int needLevel { get; }
    public int coolTime { get; }
    public float holdTime { get; }
    public string ability { get; }
    public string effectName { get; }


    public Skill(int _Index)
    {
        List<string> table = ResourceManager.resource.GetTable_Index("SkillTable",_Index);

        index = _Index;

        SKILLTYPE t_Type;
        if(Enum.TryParse(table[1],out t_Type))
        {
            skillType = t_Type;
        }
        else
        {
            Debug.LogError("Create Skill : SkillType Error");
        }

        name = table[2];

        spriteName = table[3];

        float t_Mana;
        if(float.TryParse(table[4],out t_Mana))
        {
            mana = t_Mana;
        }
        else
        {
            Debug.LogError("Create Skill : SkillMana Error");
        }

        explain = table[5];

        int t_NeedLevel;
        if(int.TryParse(table[6],out t_NeedLevel))
        {
            needLevel = t_NeedLevel;
        }
        else
        {
            Debug.LogError("Create Skill : SkillNeedLevel Error");
        }

        int t_CoolTime;
        if (int.TryParse(table[7], out t_CoolTime))
        {
            coolTime = t_CoolTime;
        }
        else
        {
            Debug.LogError("Create Skill : SkillCoolTime Error");
        }

        float t_HoldTime;
        if (float.TryParse(table[8], out t_HoldTime))
        {
            holdTime = t_HoldTime;
        }
        else
        {
            Debug.LogError("Create Skill : SkillHoldTime Error");
        }
        
        ability = table[9];
        effectName = spriteName;
        //if (!string.IsNullOrEmpty(table[10]))
        //{
        //    effectName = table[10]);
        //}
        //else
        //{
        //    effectName = string.Empty;
        //}
    }








}
