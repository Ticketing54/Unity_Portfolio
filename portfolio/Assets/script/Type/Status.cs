using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    
    Character character;            // 설정 시 지정
    
    public Status(Character _character)
    {
        LevelSetting(1);
        character = _character;        
    }

    public string NAME { get; set; }
    public int LEVEL { get { return level; } }    
    public int CRI { get; set; }
    public float ATK_RANGE { get { return attack_Range; } set{ attack_Range = value; } }
    public float HP { get { return cur_Hp; } set { cur_Hp = value; } }
    public float MP { get { return cur_Mp; } set { cur_Mp = value; } }
    public float MAXHP
    {
        get
        {
            return hp + equip_Hp;
        }
        set
        {
            equip_Hp = value;
        }
    }
    public float MAXMP
    {
        get
        {
            return mp + equip_Mp;
        }
        set
        {
            equip_Mp = value;
        }
    }
    public float AttackDamage
    {
        get
        {
            return atk + equip_Atk;
        }
        set
        {
            equip_Atk = value;
        }
    }
    public int MAXEXP { get { return Need_Exp; } }
    public int EXP { get { return cur_Exp; } set { cur_Exp = value; } }                 // LevelUp 구현\
   
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
        cur_Exp += Exp;
        if (cur_Exp >= Need_Exp)
            LevelUp();
    }
    public void LevelUp()
    {
        cur_Exp -= Need_Exp;
        level++;
        SkillPoint++;
        LevelSetting(level);
        // 레벨업 모션 
    }
    public void LevelSetting(int _Level)
    {        
        List<string> Table = ResourceManager.resource.GetTable_Index("LevelTable", _Level);            
        
        level = int.Parse(Table[0]);
        hp = float.Parse(Table[1]);
        mp = float.Parse(Table[2]);
        Need_Exp = int.Parse(Table[3]);        
        cur_Hp = hp;
        cur_Mp = mp;
    }  
    public void EquipStatus(Item _item)
    {

        if (_item == null)
            return;


        string[] tmp = _item.itemProperty.Split('/');
        switch (tmp[0])
        {
            case "Defend":
                equip_Hp += float.Parse(tmp[1]);
                break;
            case "Atk":
                equip_Atk += float.Parse(tmp[1]);
                break;
            default:
                break;
        }
    }
    public void TakeOffStatus(Item _item)
    {
        if (_item == null)
            return;


        string[] tmp = _item.itemProperty.Split('/');
        switch (tmp[0])
        {
            case "Defend":
                equip_Hp -= float.Parse(tmp[1]);
                break;
            case "Atk":
                equip_Atk -= float.Parse(tmp[1]);
                break;
            default:
                break;
        }
    }


    // 캐릭터 레벨
    int level;
    // 캐릭터 능력치
    float hp, mp, atk;
    int Need_Exp;
    // 장비 능력치
    float equip_Hp, equip_Mp, equip_Atk=0;
    // 현재 능력치
    float cur_Hp, cur_Mp=0;
    int cur_Exp = 0;        
    // 스킬 포인트
    
    // 공격 범위
    float attack_Range = 1.5f;

    // 수정할 것
    public bool isburn = false;
    public bool isIce = false;
    public int SkillPoint = 0;

    bool usingPotion_Hp = false;  
    bool usingPotion_Mp = false;
    public bool UsingPotion_Hp { get => usingPotion_Hp; }
    public bool UsingPotion_Mp { get => usingPotion_Mp; }
    public void BuffSkill(Skill _skill)
    {
        character.StartCoroutine(CoBuffSkill(_skill));        
    }

    IEnumerator CoBuffSkill(Skill _skill)
    {
        string abilityList = _skill.ability;
        GameObject effect = EffectManager.effectManager.GetBuffEffect(_skill.effectName);
        effect.transform.SetParent(character.transform);
        effect.transform.localPosition = Vector3.zero;

        string[] ability = abilityList.Split("#");
        for (int i = 0; i < ability.Length; i++)
        {
            string[] abilityData = ability[i].Split('/');
            ApplybufSkill(abilityData[0], float.Parse(abilityData[1]));
        }

        yield return new WaitForSeconds(_skill.holdTime);

        for (int i = 0; i < ability.Length; i++)
        {
            string[] abilityData = ability[i].Split('/');
            ApplybufSkill(abilityData[0], -float.Parse(abilityData[1]));
        }
        EffectManager.effectManager.PushBuffEffect(_skill.effectName, effect);
    }
    void ApplybufSkill(string _stat, float _bufIncrease)
    {
        switch (_stat)
        {
            case "Spd":
                {
                    
                }
                break;
            case "Atk":
                {
                    atk += _bufIncrease;
                }
                break;
            case "Cri":
                break;
            case "Def":
                break;

        }
    }
    public void RecoveryHp(float _rehp, float _duration)
    {
        usingPotion_Hp = true;        
        character.StartCoroutine(CoRecoveryHp(_rehp, _duration));
    }
    public void RecoveryMp(float _reMp, float _duration)
    {
        usingPotion_Mp = true;
        character.StartCoroutine(CoRecoveryMp(_reMp, _duration));
    }   
    IEnumerator CoRecoveryHp(float _rehp, float _duration)
    {
        float timer = 0f;
        GameObject recoveryHpEffect = EffectManager.effectManager.GetBuffEffect("Hp");
        recoveryHpEffect.transform.SetParent(character.transform);
        recoveryHpEffect.transform.localPosition = Vector3.zero;
        while (timer <= _duration)
        {
            HP += _rehp;
            timer += Time.deltaTime;

            if (HP >= MAXHP)
            {
                HP = MAXHP;
            }

            //update Ui Hp;
            yield return null;
        }
        
        usingPotion_Hp = false;
        EffectManager.effectManager.PushBuffEffect("Hp", recoveryHpEffect);
    }
    IEnumerator CoRecoveryMp(float _remp, float _duration)
    {
        float timer = 0f;
        GameObject recoveryMpEffect = EffectManager.effectManager.GetBuffEffect("Mp");
        recoveryMpEffect.transform.SetParent(character.transform);
        recoveryMpEffect.transform.localPosition = Vector3.zero;
        while (timer <= _duration)
        {
            MP += _remp;
            timer += Time.deltaTime;

            if (MP >= MAXMP)
            {
                MP = MAXMP;
            }

            //update Ui Hp;
            yield return null;
        }
        usingPotion_Mp = false;
        EffectManager.effectManager.PushBuffEffect("Mp",recoveryMpEffect);

    }







}
