using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterSkill
{
    Dictionary<int, Skill> skillDic;
    Skill skill;
    Character character;
    NavMeshAgent nav;
    Animator anim;
    
    public CharacterSkill(Character _character,NavMeshAgent _nav, Animator _anim)
    {
        skillDic = new Dictionary<int, Skill>();
        
        character = _character;
        nav = _nav;
        anim = _anim;
    }

    public void UseSkill(int _skillIndex)
    {
        Skill skill;
        if (!skillDic.TryGetValue(_skillIndex,out skill))
        {
            return;
        }        
        
        //if (skill.mana > character.stat.MP)               // 마나 소모
        //{
        //    Debug.Log("마나가 부족합니다.");
        //    return;
        //}
        //else
        //{
        //    character.stat.MP -= skill.mana;
        //}


        switch (skill.index)
        {
            case 1:
                {
                    BuffSkill(skill);
                }
                break;
            case 2:
                {
                    RushSkill();
                }
                break;
            case 3:
                {
                    BuffSkill(skill);
                }
                break;
            case 4:
                {
                    BuffSkill(skill);
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
    }

    public void Add(int _skillIndex)
    {
        Skill newSkill = new Skill(_skillIndex);
        skillDic.Add(_skillIndex, newSkill);
    }
   


    IEnumerator CoTakeDownSword()
    {
        yield return new WaitForSeconds(2f);
        character.isCantMove = false;
    }

    IEnumerator CoRushSkill()
    {
        character.isCantMove = true;
        nav.ResetPath();
        float timer = 0f;
        DamageList list = new DamageList();
        foreach (Monster mob in character.nearMonster)
        {
            list.Add(mob);
        }
        Vector3 des = nav.transform.forward;
        while (timer <= 1.2)
        {
            timer += Time.deltaTime;
            nav.velocity = des * 2;
            list.DamedMonster(5f, STATUSEFFECT.STURN, 1f);
            yield return null;
        }
        character.isCantMove = false;
    }


    
    public void TakeDownSword() // 애니메이션 이벤트                                     //skill 0번
    {
        character.isCantMove = true;
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
    public void BuffSkill(Skill _skillIndex)                                     //skill 2번
    {
        anim.SetFloat("SkillNum", 2);        
        character.stat.BuffSkill(_skillIndex);
        
    }
    public void SkillEffectOn(string _effectName)
    {

    }
}
