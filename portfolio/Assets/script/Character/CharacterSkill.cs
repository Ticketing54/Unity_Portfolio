using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterSkill
{
    HashSet<int> haveSkills;
    HashSet<int> coolTimeSkills;
    
    Character character;
    NavMeshAgent nav;
    Animator anim;
    
    public CharacterSkill(Character _character,NavMeshAgent _nav, Animator _anim)
    {
        haveSkills = new HashSet<int>();
        coolTimeSkills = new HashSet<int>();        
        character = _character;
        nav = _nav;
        anim = _anim;
    }

    public void UseSkill(int _skillIndex)
    {   
        if(!haveSkills.Contains(_skillIndex) || coolTimeSkills.Contains(_skillIndex))
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


        switch (_skillIndex)
        {
            case 1:
                {
                    BuffSkill(_skillIndex);
                }
                break;
            case 2:
                {
                    RushSkill();
                }
                break;
            case 3:
                {
                    BuffSkill(_skillIndex);
                }
                break;
            case 4:
                {
                    BuffSkill(_skillIndex);
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
        haveSkills.Add(_skillIndex);
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
    public void BuffSkill(int _skillIndex)                                     //skill 2번
    {
        anim.SetFloat("SkillNum", 2);
        character.StartCoroutine(CoBuffSkill(_skillIndex));
    }
    public void SkillEffectOn(string _effectName)
    {

    }

    IEnumerator CoBuffSkill(int _skillindex)
    {
        Skill skill = new Skill(_skillindex);
        
        GameObject effect = EffectManager.effectManager.GetBuffEffect(skill.effectName);
        effect.transform.SetParent(character.transform);
        effect.transform.localPosition = Vector3.zero;

        string abilityList = skill.ability;
        string[] ability = abilityList.Split("#");
        for (int i = 0; i < ability.Length; i++)
        {
            string[] abilityData = ability[i].Split('/');
            ApplybufSkill(abilityData[0], float.Parse(abilityData[1]));
        }

        yield return new WaitForSeconds(skill.holdTime);

        for (int i = 0; i < ability.Length; i++)
        {
            string[] abilityData = ability[i].Split('/');
            ApplybufSkill(abilityData[0], -float.Parse(abilityData[1]));
        }
        EffectManager.effectManager.PushBuffEffect(skill.effectName, effect);
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
                    character.stat.Atk += _bufIncrease;
                }
                break;
            case "Cri":
                break;
            case "Def":
                break;

        }
    }
}
