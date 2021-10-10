using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    List<string> Table;
    public Status(string _Name, int _Gold, int _Level,float _Hp_c, float _Mp_C, float _Exp_C, int _SkillPoint)
    {
        NAME = _Name;
        GOLD = _Gold;
        Player_Level = _Level;
        Cur_Hp = _Hp_c;
        Cur_Mp = _Mp_C;
        Cur_Exp = _Exp_C;
        SkillPoint = _SkillPoint;
    }

    public string NAME { get; set; }
    public int LEVEL { get { return Player_Level; } }
    public int GOLD { get; set; }
    public int CRI { get; set; }
    public float ATK_RANGE { get { return Attack_Range; } set{ Attack_Range = value; } }
    public float HP { get { return Cur_Hp; } set { Cur_Hp = value; } }
    public float MP { get { return Cur_Mp; } set { Cur_Mp = value; } }
    public float MAXHP
    {
        get
        {
            return Character_Hp + Equip_Hp;
        }
        set
        {
            Equip_Hp = value;
        }
    }
    public float MAXMP
    {
        get
        {
            return Character_Mp + Equip_Mp;
        }
        set
        {
            Equip_Mp = value;
        }
    }
    public float ATK
    {
        get
        {
            return Character_Atk + Equip_Atk;
        }
        set
        {
            Equip_Atk = value;
        }
    }
    public float MAXEXP { get { return Need_Exp; } }
    public float EXP { get { return Cur_Exp; } set { Cur_Exp = value; } }
    //public float returnAtk()
    //{
    //    float tmp = Character_Atk + Equip_Atk;
    //    if (tmp == 1)
    //        return 1;

    //    float critical;
    //    critical = Random.Range(0, 101);
    //    if (critical <= Cri)
    //    {
    //        return (int)(Random.Range((float)(tmp * 0.8), (float)(tmp * 1.2)) * 2);
    //    }
    //    else
    //    {
    //        return (int)Random.Range((float)(tmp * 0.8), (float)(tmp * 1.2) * 2);
    //    }
    //}
    public void LevelUp()
    {
        Cur_Exp -= Need_Exp;
        Player_Level++;
        SkillPoint++;
        LevelSetting(Player_Level);
    }
    public void LevelSetting(int _Level)
    {
        
        Table = LevelTableManager.instance.Level_Table.GetData(_Level);        
        Player_Level = int.Parse(Table[0]);
        Character_Hp = float.Parse(Table[1]);
        Character_Mp = float.Parse(Table[2]);
        Need_Exp = float.Parse(Table[3]);        
        Cur_Hp = Character_Hp;
        Cur_Mp = Character_Mp;
    }  
    // 캐릭터 레벨
    int Player_Level;
    // 캐릭터 능력치
    float Character_Hp, Character_Mp, Character_Atk, Need_Exp;
    // 장비 능력치
    float Equip_Hp, Equip_Mp, Equip_Atk=0; 
    // 현재 능력치
    float Cur_Hp, Cur_Mp, Cur_Exp = -1;        
    // 스킬 포인트
    
    // 공격 범위
    float Attack_Range = 1.5f;

    // 수정할 것
    public bool isburn = false;
    public bool isIce = false;
    public int SkillPoint = 0; 



}
