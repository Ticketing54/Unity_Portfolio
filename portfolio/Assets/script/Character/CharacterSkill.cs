using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterSkill
{   
    HashSet<int> haveSkills;    

    Skill[,] skillQuick;
    int skillQuickNum;
    public int SkillSlotNum 
    { 
        get => skillQuickNum;
        set
        {
            if(value >= 4)
            {
                skillQuickNum = 0;
            }
            else
            {
                skillQuickNum = value;
            }
        } 
    }

    Character character;
    NavMeshAgent nav;
    Animator anim;


    HashSet<int> coolTimeSkill;

    public CharacterSkill(Character _character,NavMeshAgent _nav, Animator _anim)
    {
        character = _character;
        nav = _nav;
        anim = _anim;

        haveSkills       = new HashSet<int>();
        coolTimeSkill     = new HashSet<int>();
        skillQuick       = new Skill[4, 4];
        
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.Q, SkillSlot_Q);
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.W, SkillSlot_W);
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.E, SkillSlot_E);
        UIManager.uimanager.AddKeyBoardSortCut(KeyCode.R, SkillSlot_R);
    }
    #region KeyboardShorcut   
    public void SkillSlot_Q()
    {
        UseSkill(0);
    }

    public void SkillSlot_W()
    {
        UseSkill(1);
    }

    public void SkillSlot_E()
    {
        UseSkill(2);
    }

    public void SkillSlot_R()
    {
        UseSkill(3);
    }
    #endregion
    #region QuickSkill
    public void AddQuickSkill(int _slotNum, int _skillIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            if(skillQuick[skillQuickNum, i] != null && skillQuick[skillQuickNum, i].index == _skillIndex)
            {
                RemoveQuickSkill(i);
                UIManager.uimanager.AUpdateSkillSlot(i);
            }                
        }

        skillQuick[skillQuickNum, _slotNum] = new Skill(_skillIndex);
        UIManager.uimanager.AUpdateSkillSlot(_slotNum);
    }

    public void RemoveQuickSkill(int _slotNum)
    {
        skillQuick[skillQuickNum, _slotNum] = null;
    }

    public Skill GetQuickSkill(int _slotNum)
    {
        return skillQuick[skillQuickNum, _slotNum];
    }
    public void MoveQuickSkill(int _startIndex, int _endIndex)
    {
        if(_endIndex == -1)
        {
            skillQuick[skillQuickNum, _startIndex] = null;
            UIManager.uimanager.AUpdateSkillSlot(_startIndex);
            return;
        }
        Skill skill = skillQuick[skillQuickNum, _startIndex];
        skillQuick[skillQuickNum, _startIndex] = skillQuick[skillQuickNum, _endIndex];
        skillQuick[skillQuickNum, _endIndex] = skill;

        UIManager.uimanager.AUpdateSkillSlot(_startIndex);
        UIManager.uimanager.AUpdateSkillSlot(_endIndex);
    }
    #endregion

    void UseSkill(int _slotNum)
    {
        Skill skill = GetQuickSkill(_slotNum);

        if(skill == null)
        {
            return;
        }

        if(!haveSkills.Contains(skill.index) || coolTimeSkill.Contains(skill.index))
        {
            return;
        }
        //if (skill.mana > character.stat.Mp)               // 마나 소모
        //{
        //    Debug.Log("마나가 부족합니다.");
        //    return;
        //}
        //else
        //{
        //    character.stat.Mp -= skill.mana;
        //}


        switch (skill.index)
        {
            case 1:
                {
                    BuffSkill(skill.index);
                }
                break;
            case 2:
                {
                    RushSkill();
                }
                break;
            case 3:
                {
                    BuffSkill(skill.index);
                }
                break;
            case 4:
                {
                    BuffSkill(skill.index);
                }
                break;
            case 5:
                {
                    TakeDownSword();
                }
                break;
            default:
                return;
        }

        coolTimeSkill.Add(skill.index);
        character.StartCoroutine(CoCoolTimecheck_Skill(skill.index, skill.coolTime));

    }

    public void Add(int _skillIndex)
    {
        if (!haveSkills.Contains(_skillIndex))
        {
            if (character.stat.SkillPoint > 0)
            {
                character.stat.SkillPoint--;
                haveSkills.Add(_skillIndex);
                int slotIndex = _skillIndex - 1;
                UIManager.uimanager.AUpdateSkillMain(slotIndex);
            }
        }        
    }
   public Skill GetSkill(int _skillIndex)
    {
        if (haveSkills.Contains(_skillIndex))
        {
            return new Skill(_skillIndex);
        }
        else
        {
            return null;
        }
    }


    IEnumerator CoTakeDownSword()
    {
        yield return new WaitForSeconds(2f);
        character.isPossableMove = true;
    }

    IEnumerator CoRushSkill()           // 스킬 끝나는 시간 조절하기
    {
        Vector3 curPosition = character.gameObject.transform.position;
        Monster closestMob = character.ClosestMonster();
        Vector3 targetPos;
        if (closestMob == null)
        {
            nav.stoppingDistance = 0f;
            nav.acceleration = 100;
            targetPos = character.gameObject.transform.position + character.gameObject.transform.forward * 4f;
        }
        else
        {
            nav.stoppingDistance = 2f;            
            targetPos = closestMob.transform.position;
        }
        yield return new WaitForSeconds(0.6f);
        nav.speed = 20;
        

        NavMeshHit hitinfo;
        if(nav.Raycast(targetPos, out hitinfo))
        {
            nav.SetDestination(hitinfo.position);
        }
        else
        {
            nav.SetDestination(targetPos);
        }


        float timer = 0f;
        DamageList list = new DamageList();
        foreach (Monster mob in character.nearMonster)
        {
            list.Add(mob);
        }
        Vector3 des = nav.transform.forward;
        while (timer < 1.5f)
        {
            yield return null;

            timer += Time.deltaTime;
            list.DamedMonster(5f, STATUSEFFECT.STURN, 1f);            
            if(nav.remainingDistance <= 0.01 && nav.velocity.magnitude == 0f)
            {
                break;
            }
            
        }

        nav.speed = 7;
        character.isPossableMove = true;
    }


    
    public void TakeDownSword() // 애니메이션 이벤트                                     //skill 0번
    {
        character.isPossableMove = false;
        //이펙트 연출
        character.StartCoroutine(CoTakeDownSword());
        anim.SetFloat("SkillNum", 0);
        anim.SetTrigger("Skill");

    }
    public void RushSkill()                                                              //skill 1번
    {
        // 이펙트 추가 할 것
        anim.SetFloat("SkillNum", 1);
        anim.SetTrigger("Skill");
        Monster closestMob = character.ClosestMonster();
        if (character.nearMonster.Count != 0)
        {
            character.transform.LookAt(closestMob.transform);
        }
        character.StartCoroutine(CoRushSkill());
    }
    public void BuffSkill(int _skillIndex)                                     //skill 2번
    {
        anim.SetFloat("SkillNum", 2);
        anim.SetTrigger("Skill");
        character.stat.ApplyBuffSkill(_skillIndex);
        //character.StartCoroutine(CoBuffSkill(_skillIndex));
    }
    public void SkillEffectOn(string _effectName)
    {

    }

    IEnumerator CoBuffSkill(int _skillindex)
    {
        yield return null;
        //Skill skill = new Skill(_skillindex);
        
        //GameObject effect = EffectManager.effectManager.GetBuffEffect(skill.effectName);
        //effect.transform.SetParent(character.transform);
        //effect.transform.localPosition = Vector3.zero;

        //string abilityList = skill.ability;
        //string[] ability = abilityList.Split("#");
        //for (int i = 0; i < ability.Length; i++)
        //{
        //    string[] abilityData = ability[i].Split('/');
        //    ApplybufSkill(abilityData[0], float.Parse(abilityData[1]));
        //}

        //yield return new WaitForSeconds(skill.holdTime);

        //for (int i = 0; i < ability.Length; i++)
        //{
        //    string[] abilityData = ability[i].Split('/');
        //    ApplybufSkill(abilityData[0], -float.Parse(abilityData[1]));
        //}
        //EffectManager.effectManager.PushBuffEffect(skill.effectName, effect);
    }

    IEnumerator CoBuffUIControl(Skill _skill)
    {
        float timer = 0f;
        float holdTime = _skill.holdTime;
        float coolTime = _skill.coolTime;
        string spriteName = _skill.spriteName;
        while (timer < coolTime)
        {
            timer += Time.deltaTime;


            yield return null;
        }
    }
    
    IEnumerator CoCoolTimecheck_Skill(int _skillIndex, float _coolTime)
    {
        float timer = 0.0001f;

        while (timer <= _coolTime)
        {
            timer += Time.deltaTime;
            float maxCoolTime = _coolTime;
            maxCoolTime -= timer;
            float percent = timer / _coolTime;
            CoolTimeCheck_SKill(_skillIndex, percent, (int)maxCoolTime);
            yield return null;
        }
        coolTimeSkill.Remove(_skillIndex);
    }

    void CoolTimeCheck_SKill(int _skillIndex, float _percent, int _coolTime)
    {
        for (int i = 0; i < 4; i++)
        {
            if (skillQuick[skillQuickNum,i]!= null && skillQuick[skillQuickNum, i].index == _skillIndex)
            {
                UIManager.uimanager.AQuickSlotSkillCooltime(i, _percent, _coolTime);
            }
        }
    }


}
