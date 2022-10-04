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
    public bool isUsingSkill;
    public CharacterSkill(Character _character,NavMeshAgent _nav, Animator _anim)
    {
        character = _character;
        nav = _nav;
        anim = _anim;
        isUsingSkill = false;

        haveSkills       = new HashSet<int>();
        coolTimeSkill     = new HashSet<int>();
        skillQuick       = new Skill[4, 4];
        character.AddKeyBoardSortCut(KeyCode.K, TryOpenEquipment);
        character.AddKeyBoardSortCut(KeyCode.Q, SkillSlot_Q);
        character.AddKeyBoardSortCut(KeyCode.W, SkillSlot_W);
        character.AddKeyBoardSortCut(KeyCode.E, SkillSlot_E);
        character.AddKeyBoardSortCut(KeyCode.R, SkillSlot_R);
    }
    #region KeyboardShorcut
    bool skillActive = false;
    void TryOpenEquipment()
    {
        skillActive = !skillActive;
        if (skillActive)
        {
            UIManager.uimanager.AOpenSKill();
        }
        else
        {
            UIManager.uimanager.ACloseSKill();
        }
    }

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
        nav.ResetPath();

        if(isUsingSkill == true && character.isPossableMove == false)
        {

            // "이 행동중엔 사용하실수 없습니다." 메세지 만들것
            return;
        }

        Skill skill = GetQuickSkill(_slotNum);

        if(skill == null)
        {
            return;
        }

        if(!haveSkills.Contains(skill.index) || coolTimeSkill.Contains(skill.index))
        {
            return;
        }

        if (skill.mana > character.stat.Mp)               // 마나 소모
        {
            Debug.Log("마나가 부족합니다.");
            return;
        }
        else
        {
            character.stat.Mp -= skill.mana;
        }


        if (skill.skillType == SKILLTYPE.BUFF)           // buff is same animation
        {   
            anim.SetInteger("SkillNum", 0);
            anim.SetBool("Skill", true);
            BuffSkill(skill.index);
        }
        else
        {   
            anim.SetInteger("SkillNum", skill.index);
            anim.SetBool("Skill", true);
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

    
    public void BuffSkill(int _skillIndex)                                     // buffSkill
    {   
        if (_skillIndex == 3)
        {
            character.StartCoroutine(CoFireField());            
        }
        else
        {
            character.stat.ApplyBuffSkill(_skillIndex);
        }
    }
    
    IEnumerator CoBuffSkill(int _skillindex)
    {
        yield return null;
        Skill skill = new Skill(_skillindex);
        
        
        character.stat.ApplyBuffSkill(skill.index);
        GameObject effect = EffectManager.effectManager.GetBuffEffect(skill.effectName);
        effect.transform.SetParent(character.transform);
        effect.transform.localPosition = Vector3.zero;

        string abilityList = skill.ability;
        string[] ability = abilityList.Split("#");
        for (int i = 0; i < ability.Length; i++)
        {
            string[] abilityData = ability[i].Split('/');
            
        }
        yield return new WaitForSeconds(skill.holdTime);
        
        EffectManager.effectManager.PushBuffEffect(skill.effectName, effect);
    }
    IEnumerator CoFireField()
    {
        HashSet<Monster> hitmob = new HashSet<Monster>();
        GameObject skillEffect = EffectManager.effectManager.GetBuffEffect("skill2");
        skillEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        

        float timer = 0f;
        while (timer <= 5)
        {
            yield return null;
            timer += Time.deltaTime;
            skillEffect.transform.position = character.transform.position;
            Collider[] hitMonsters = Physics.OverlapSphere(character.transform.position, 1f);
            for (int i = 0; i < hitMonsters.Length; i++)
            {
                if(hitMonsters[i].tag == "Monster")
                {
                    Monster mob = hitMonsters[i].GetComponent<Monster>();
                    if (!hitmob.Contains(mob))
                    {
                        mob.Damaged(false, 20);
                        hitmob.Add(mob);
                        character.StartCoroutine(ExitHit(hitmob, mob));
                    }
                }
            }

        }
        EffectManager.effectManager.PushBuffEffect("skill2", skillEffect);
    }

    IEnumerator ExitHit(HashSet<Monster> _hitmonster,Monster _mob)
    {
        yield return new WaitForSeconds(1f);
        _hitmonster.Remove(_mob);
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
