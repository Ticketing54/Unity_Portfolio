using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Status
{

    Character character;            // 설정 시 지정


    public Status(Character _character)
    {
        LevelSetting(1);
        character = _character;      
    }

    // 캐릭터 레벨
    int level;

    // 캐릭터 능력치
    float hp, mp, atk;
    int need_Exp;

    // 장비 능력치
    float equip_Hp, equip_Mp, equip_Atk;

    // 현재 능력치
    float cur_Hp, cur_Mp = 0;
    int cur_Exp = 0;

    // 스킬 포인트
    int skillPoint = 0;
    public string NAME { get; set; }
    public int Level
    { 
        get 
        { 
            return level;
        }
        set
        {
            level = value;
            UIManager.uimanager.AUpdateLevel();
        }
    }
    public float MaxHp { get => hp + equip_Hp; }
    public float MaxMp { get => mp + equip_Hp; }    
    
    public int MaxExp
    {
        get => need_Exp;
        set
        {
            need_Exp = value;
            UIManager.uimanager.AUpdateExp();
        }
    }
    public float Hp
    {
        get => cur_Hp;
        set
        {
            if (value <= 0)
            {
                cur_Hp = 0;
            }
            else if (value >= MaxHp)
            {
                cur_Hp = MaxHp;
            }
            else
            {
                cur_Hp = value;
            }
            UIManager.uimanager.AUpdateHp();
        }
    }
    public float Mp
    {
        get => cur_Mp;
        set
        {
            if (cur_Mp <= 0)
            {
                cur_Mp = 0;
            }
            else if (cur_Mp >= MaxMp)
            {
                cur_Mp = MaxMp;
            }
            else
            {
                cur_Mp = value;
            }

            UIManager.uimanager.AUpdateMp();
        }
    }    
    public float Atk
    {
        get => atk;
        set
        {
            atk = value;
            UIManager.uimanager.AUpdateAtk();
        }
    }
    public int Exp
    {
        get
        {
            return cur_Exp;
        }
        set
        {
            cur_Exp = value;
            UIManager.uimanager.AUpdateExp();
        }
    }
    public float E_Hp
    {
        get => equip_Hp;
        set
        {
            equip_Hp = value;
            UIManager.uimanager.AUpdateHp();
        }
    }
    public float E_Mp
    {
        get => equip_Mp;
        set
        {
            equip_Mp = value;
            UIManager.uimanager.AUpdateMp();
        }
    }
    public float E_Atk
    {
        get => equip_Atk;
        set
        {
            equip_Atk = value;
            UIManager.uimanager.AUpdateAtk();
        }
    }
    public int SkillPoint
    {
        get
        {
            return skillPoint;
        }
        set
        {
            skillPoint = value;
            UIManager.uimanager.AUpdateSkillPoint();
        }
    }

    public int CRI { get; set; }
    public float ATK_RANGE { get { return attack_Range; } set { attack_Range = value; } }
    public float AttckDamage
    {
        get
        {
            float attackdamage = atk + equip_Atk;
            return (int)UnityEngine.Random.Range((float)(attackdamage * 0.8), (float)(attackdamage * 1.2));
        }        
    }
    public DAMAGE DamageType()
    {
        int count = CRI;

        int probabillity = UnityEngine.Random.Range(count, 101);

        if (probabillity <= count)
        {
            return DAMAGE.CRITICAL;
        }
        else
        {
            return DAMAGE.NOMAL;
        }
    }
    public void Damaged(DAMAGE _type, float _dmg)
    {
        switch (_type)
        {
            case DAMAGE.NOMAL:
                {
                    Hp -= _dmg;
                }
                break;
            case DAMAGE.CRITICAL:
                {
                    Hp -= _dmg * 2;
                }
                break;
        }        
        UIManager.uimanager.uiEffectManager.LoadDamageEffect(_dmg, character.gameObject, _type);        
    }
    public void GetExp(int _exp)
    {
        Exp += _exp;
        UIManager.uimanager.AGetExpUpdateUi(_exp);
        if(Exp >= need_Exp)
        {
            LevelUp();
            return;
        }
    }
    public void LevelUp()
    {
        Exp -= need_Exp;        
        Level++;
        SkillPoint++;
        LevelSetting(level);        
        UIManager.uimanager.uiEffectManager.LevelUpEffect(character.gameObject);
    }
    
    public void LevelSetting(int _Level)
    {
        List<string> Table = ResourceManager.resource.GetTable_Index("LevelTable", _Level);            
        
        level = int.Parse(Table[0]);        
        hp = float.Parse(Table[1]);
        mp = float.Parse(Table[2]);
        need_Exp = int.Parse(Table[3]);
        // 임시
        //
        cur_Hp = hp;
        cur_Mp = mp;
        // 
    }  
    public void EquipStatus(Item _item)
    {

        if (_item == null)
            return;


        string[] tmp = _item.itemProperty.Split('/');
        switch (tmp[0])
        {
            case "Defend":
                E_Hp += float.Parse(tmp[1]);
                break;
            case "Atk":
                E_Atk += float.Parse(tmp[1]);
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
        // Ui 세팅
    }


    // 공격 범위
    float attack_Range = 1.5f;

    // 수정할 것
    public bool isburn = false;
    public bool isIce = false;
    
    
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
            Hp += _rehp;
            timer += Time.deltaTime;

            if (Hp >= MaxHp)
            {
                Hp = MaxHp;
            }            
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
            Mp += _remp;
            timer += Time.deltaTime;

            if (Mp >= MaxMp)
            {
                Mp = MaxMp;
            }            
            yield return null;
        }
        usingPotion_Mp = false;
        EffectManager.effectManager.PushBuffEffect("Mp",recoveryMpEffect);

    }







}
