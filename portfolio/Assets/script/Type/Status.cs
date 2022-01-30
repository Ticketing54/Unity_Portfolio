using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    List<string> Table;
    
    public Status(string _Name, int _Gold, int _Level,float _Hp_c, float _Mp_C, int _Exp_C, int _SkillPoint)
    {
        NAME = _Name;
        GOLD = _Gold;
        Level = _Level;
        Cur_Hp = _Hp_c;
        Cur_Mp = _Mp_C;
        Cur_Exp = _Exp_C;
        SkillPoint = _SkillPoint;
    }

    public string NAME { get; set; }
    public int LEVEL { get { return Level; } }
    public int GOLD { get; set; }
    public int CRI { get; set; }
    public float ATK_RANGE { get { return Attack_Range; } set{ Attack_Range = value; } }
    public float HP { get { return Cur_Hp; } set { Cur_Hp = value; } }
    public float MP { get { return Cur_Mp; } set { Cur_Mp = value; } }
    public float MAXHP
    {
        get
        {
            return Hp + Equip_Hp;
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
            return Mp + Equip_Mp;
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
            return Atk + Equip_Atk;
        }
        set
        {
            Equip_Atk = value;
        }
    }
    public int MAXEXP { get { return Need_Exp; } }
    public int EXP { get { return Cur_Exp; } set { Cur_Exp = value; } }
    //public float returnAtk()
    //{
    //    float tmp = Atk + Equip_Atk;
    //    if (tmp == 1)
    //        return 1;

    //    float critical;
    //    critical = Random.Range(0, 101);
    //    if (critical <= CRI)
    //    {
    //        return (int)(Random.Range((float)(tmp * 0.8), (float)(tmp * 1.2)) * 2);
    //    }
    //    else
    //    {
    //        return (int)Random.Range((float)(tmp * 0.8), (float)(tmp * 1.2) * 2);
    //    }
    //}
    public void GetExp(int Exp)
    {
        Cur_Exp += Exp;
        if (Cur_Exp >= Need_Exp)
            LevelUp();
    }
    public void LevelUp()
    {
        Cur_Exp -= Need_Exp;
        Level++;
        SkillPoint++;
        LevelSetting(Level);
    }
    public void LevelSetting(int _Level)
    {        
        Table = GameManager.gameManager.resource.GetTable(TABLETYPE.LEVEL,_Level);            
        
        Level = int.Parse(Table[0]);
        Hp = float.Parse(Table[1]);
        Mp = float.Parse(Table[2]);
        Need_Exp = int.Parse(Table[3]);        
        Cur_Hp = Hp;
        Cur_Mp = Mp;
    }  
    public void EquipStatus(Item _item)
    {
        string[] tmp = _item.ItemProperty.Split('/');
        switch (tmp[0])
        {
            case "Defend":
                Equip_Hp += float.Parse(tmp[1]);
                break;
            case "Atk":
                Equip_Atk += float.Parse(tmp[1]);
                break;
            default:
                break;
        }
    }
    public void TakeOffStatus(Item _item)
    {
        string[] tmp = _item.ItemProperty.Split('/');
        switch (tmp[0])
        {
            case "Defend":
                Equip_Hp -= float.Parse(tmp[1]);
                break;
            case "Atk":
                Equip_Atk -= float.Parse(tmp[1]);
                break;
            default:
                break;
        }
    }
    // 캐릭터 레벨
    int Level;
    // 캐릭터 능력치
    float Hp, Mp, Atk;
    int Need_Exp;
    // 장비 능력치
    float Equip_Hp, Equip_Mp, Equip_Atk=0;
    // 현재 능력치
    float Cur_Hp, Cur_Mp=0;
    int Cur_Exp = 0;        
    // 스킬 포인트
    
    // 공격 범위
    float Attack_Range = 1.5f;

    // 수정할 것
    public bool isburn = false;
    public bool isIce = false;
    public int SkillPoint = 0; 



}
