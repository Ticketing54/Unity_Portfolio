using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    List<string> Table;
    public Status(string _Name, int _Gold, int _Level,float _Hp_c, float _Mp_C, float _Exp_C, int _SkillPoint)
    {
        Character_Name = _Name;
        Gold = _Gold;
        Player_Level = _Level;
        Hp_C = _Hp_c;
        Mp_C = _Mp_C;
        Exp_C = _Exp_C;
        SkillPoint = _SkillPoint;
    }
    public string Character_Name { get; set; }
    public int Gold { get; set; }
    public int Cri { get; set; }
    public float returnHp()
    {
        return Hp_E + Hp;
    }
    public float returnMp()
    {
        return Mp_E + Mp;
    }
    public float returnAtk()
    {
        float tmp = Atk + Atk_E;
        float critical;
        critical = Random.Range(0, 101);
        if (critical <= Cri)
        {
            return (int)(Random.Range((float)(tmp * 0.8), (float)(tmp * 1.2)) * 2);
        }
        else
        {
            return (int)Random.Range((float)(tmp * 0.8), (float)(tmp * 1.2) * 2);
        }
    }
    public void LevelUp()
    {
        Exp_C -= Exp;
        Player_Level++;
        SkillPoint++;
        LevelSetting(Player_Level);
    }
    public void LevelSetting(int _Level)
    {
        
        Table = LevelTableManager.instance.Level_Table.GetData(_Level);        
        Player_Level = int.Parse(Table[0]);
        Hp = float.Parse(Table[1]);
        Mp = float.Parse(Table[2]);
        Exp = float.Parse(Table[3]);        
        Hp_C = Hp;
        Mp_C = Mp;
    }
    public void EquipStatus(List<Item> Equip)
    {
        // 장비에 따른 스탯 증감
    }
    int Player_Level;
    float Hp, Mp, Atk, Exp;

    float Hp_E, Mp_E, Atk_E; 
    float Hp_C, Mp_C, Exp_C = -1; 
    
    
    float Atk_Range = 1.5f;
    int SkillPoint = 0;

    bool isburn = false;
    bool isIce = false;

}
