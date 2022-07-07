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
        StateControlDic = new Dictionary<string, Coroutine>();
    }

    Dictionary<string, Coroutine> StateControlDic;
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
    // 공격 범위
    float attack_Range = 1.5f;
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


    public void RecoveryHp_Potion(float _totalAmount, float _duration)
    {
        if (StateControlDic.ContainsKey("Hp"))
        {
            character.StopCoroutine(StateControlDic["Hp"]);
            
        }
        
        StateControlDic.Add("Hp", character.StartCoroutine(CoRecoveryHp(_totalAmount, _duration)));
    }
    public void RecoveryMp_Potion(float _totalAmount, float _duration)
    {
        if (StateControlDic.ContainsKey("Mp"))
        {
            character.StopCoroutine(StateControlDic["Mp"]);

        }

        StateControlDic.Add("Mp", character.StartCoroutine(CoRecoveryMp(_totalAmount, _duration)));
    }


    IEnumerator CoRecoveryHp(float _totalAmount, float _duration)
    {
        float timer = 0;
        float count = 0;
        float recoveryHp = _totalAmount / _duration;
        GameObject recoveryHpEffect = EffectManager.effectManager.GetBuffEffect("Hp");
        recoveryHpEffect.transform.SetParent(character.transform);
        recoveryHpEffect.transform.localPosition = Vector3.zero;
        while (timer <= _duration)
        {
            timer += Time.deltaTime;

            if (timer >= count)
            {
                count++;
                character.stat.Hp += recoveryHp;
            }

            UIManager.uimanager.AUpdateBuf("Hp", timer, _duration);
            yield return null;
        }
        UIManager.uimanager.ARemoveBuf("Hp");        
        EffectManager.effectManager.PushBuffEffect("Hp", recoveryHpEffect);
    }
    IEnumerator CoRecoveryMp(float _totalAmount, float _duration)
    {
        float timer = 0;
        float count = 0;
        float recoveryMp = _totalAmount / _duration;
        GameObject recoveryMpEffect = EffectManager.effectManager.GetBuffEffect("Mp");
        recoveryMpEffect.transform.SetParent(character.transform);
        recoveryMpEffect.transform.localPosition = Vector3.zero;
        while (timer <= _duration)
        {
            timer += Time.deltaTime;

            if (timer >= count)
            {
                count++;
                character.stat.Mp += recoveryMp;
            }

            UIManager.uimanager.AUpdateBuf("Mp", timer, _duration);
            yield return null;
        }
        UIManager.uimanager.ARemoveBuf("Mp");        
        EffectManager.effectManager.PushBuffEffect("Mp", recoveryMpEffect);
    }

}
