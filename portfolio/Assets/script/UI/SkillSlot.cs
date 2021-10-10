using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SkillSlot : MonoBehaviour
{
    public Skill skill = null;   
    public Image skillImage;
    public Image CoolTime_Image;
    public TextMeshProUGUI CoolTime_Num;
    public Image Possible_Image; // 습득한거
    public Image PosLev_Image;      //습득가능한거
    
    public bool isactive = false;


    public RectTransform tr;
    Rect rc;
    GameObject Effect;
    public Rect RC
    {
        get
        {
            rc.x = tr.position.x - tr.rect.width * 0.5f;
            rc.y = tr.position.y + tr.rect.height * 0.5f;
            return rc;
        }
    }


    void Start()
    {
        rc.x = tr.transform.position.x - tr.rect.width / 2;
        rc.y = tr.transform.position.y + tr.rect.height / 2;
        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;       
    }







    public bool isInRect(Vector2 pos)
    {
        if (pos.x >= RC.x &&pos.x <= RC.x + RC.width &&pos.y >= RC.y - RC.height &&pos.y <= RC.y)
        {
            
            return true;
        }
        return false;
    }
    
    public void UseSkill()
    {



        if (Character.Player.Stat.MP < int.Parse(skill.Mana) || Character.Player.isMove == true)
            return;
      
        if (skill.skillType == Skill.SkillType.Active)
        {
            Monster mob = Character.Player.FindNearEnermy();
            if(mob.DiSTANCE >= 6f && mob.DiSTANCE <= 12f)
            {
                Character.Player.Stat.ATK_RANGE = 6f;
                Character.Player.Target = mob.gameObject;                
                return;

            }
            if(mob.DiSTANCE < 6f)
            {
                Character.Player.Stat.MP -= int.Parse(skill.Mana);
                Character.Player.Target = mob.gameObject;
                Character.Player.SetDestination(Character.Player.transform.position);
                Vector3 dir = mob.transform.position - Character.Player.transform.position;                
                Character.Player.gameObject.transform.rotation = Quaternion.LookRotation(dir.normalized);
                Character.Player.Stat.MP -= int.Parse(skill.Mana);
                Character.Player.anim.SetFloat("SkillNum", skill.Index);
                Character.Player.anim.SetTrigger("Skill");
                StartCoroutine(ActiveCoolTimel(skill));
                return;
                
            }

            Character.Player.Stat.MP -= int.Parse(skill.Mana);
            Character.Player.anim.SetFloat("SkillNum", skill.Index);
            Character.Player.anim.SetTrigger("Skill");            
            StartCoroutine(ActiveCoolTimel(skill));
        }
        else if(skill.skillType == Skill.SkillType.Buff)
        {
            Character.Player.Stat.MP -= int.Parse(skill.Mana);
            Character.Player.anim.SetFloat("SkillNum", skill.Index);
            Character.Player.anim.SetTrigger("Skill");
            StartCoroutine(UseBuffSkill(skill));
        }
        
    }
  
    

    
    public void Addskill(Skill _skill)
    {
        skill = _skill;
        skillImage.gameObject.SetActive(true);        
        skillImage.sprite = GameManager.gameManager.GetSprite(_skill.skillSpriteName);                    
        
    }
    public void SlotClear()
    {
        this.skill= null;
        skillImage.sprite = null;        
        skillImage.gameObject.SetActive(false);    
        
    }

   
    public void PossibleSkill()
    {
        Possible_Image.gameObject.SetActive(false);

    }
    public void statusbuf (string _info, bool _Done)
    {
        if(_Done == false)
        {
            string[] info = _info.Split('#');
            for (int i = 0; i < info.Length; i++)
            {
                string[] buf = info[i].Split('/');
                if (buf[0] == "Atk")
                {
                    Character.Player.Stat.ATK += int.Parse(buf[1]);

                }
                else if (buf[0] == "Spd")
                {
                    Character.Player.nav.speed += int.Parse(buf[1]);
                }
                else if (buf[0] == "Cri")
                {
                    Character.Player.Stat.CRI += int.Parse(buf[1]);

                }

            }

        }
        else
        {
            string[] info = _info.Split('#');
            for (int i = 0; i < info.Length; i++)
            {
                string[] buf = info[i].Split('/');
                if (buf[0] == "Atk")
                {
                    Character.Player.Stat.ATK -= int.Parse(buf[1]);

                }
                else if (buf[0] == "Spd")
                {

                    Character.Player.nav.speed -= int.Parse(buf[1]);
                }
                else if (buf[0] == "Cri")
                {
                    Character.Player.Stat.CRI -= int.Parse(buf[1]);

                }

            }

        }











    }
 
    // 스킬 쿨타임
    IEnumerator UseBuffSkill(Skill _skill) // 기본 패시브
    {
        bufimage buf_Image = null;
        float _coolTime = _skill.cooltime;
        Effect = ObjectPoolManager.objManager.EffectPooling(skill.skillSpriteName);
        buf_Image = ObjectPoolManager.objManager.PoolingbufControl();                               
        buf_Image.bufsprite.sprite = GameManager.gameManager.ImageManager[skill.skillSpriteName];   //체력바 위 버프 이미지
        buf_Image.cooltime_num.gameObject.SetActive(true);                                          //쿨타임 숫자
        float HoldTime = skill.HoldTime;
        float HoldTime_Max = HoldTime;
        CoolTime_Image.gameObject.SetActive(true);
        CoolTime_Num.gameObject.SetActive(true);
        float CoolTimeMax = _coolTime;

        statusbuf(skill.skill_ability, false);

        while (true)
        {            
            //스킬창 쿨타임
            _coolTime -= Time.deltaTime;
            CoolTime_Num.text = ((int)_coolTime).ToString();
            CoolTime_Image.fillAmount = (_coolTime/CoolTimeMax);
            if (buf_Image !=null&&Effect !=null)
            {
                Effect.transform.position = Character.Player.transform.position;
                //버프창 지속시간표시
                HoldTime -= Time.deltaTime;
                buf_Image.cooltime_num.text = ((int)HoldTime).ToString();
                buf_Image.cooltime_image.fillAmount = 1 - (HoldTime/HoldTime_Max);
                if (_skill.Index == 2)
                {
                    FileFieldRange();
                }
                if (HoldTime <= 0)
                {                    
                    statusbuf(skill.skill_ability, true);
                    Effect.SetActive(false);
                    Effect = null;
                    buf_Image.cooltime_num.gameObject.SetActive(false);
                    buf_Image.gameObject.SetActive(false);
                }

            }
                

            if (_coolTime <= 0)
            {
                
                CoolTime_Image.gameObject.SetActive(false);
                CoolTime_Num.gameObject.SetActive(false);               
                yield break;
            }

            yield return null;
        }


        
    }   
    public void FileFieldRange()
    {
        foreach(Monster one in Character.Player.MobList)
        {
            if(one.DiSTANCE <= 3f)
            {
                one.burnStateOn();
            }
        }
    }

    IEnumerator ActiveCoolTimel(Skill _skill)
    {
        
        float _coolTime = _skill.cooltime;       
        CoolTime_Image.gameObject.SetActive(true);
        CoolTime_Num.gameObject.SetActive(true);
        float CoolTimeMax = _coolTime;



        while (true)
        {
            //스킬창 쿨타임
            _coolTime -= Time.deltaTime;
            CoolTime_Num.text = ((int)_coolTime).ToString();
            CoolTime_Image.fillAmount = (_coolTime / CoolTimeMax);                       


            if (_coolTime <= 0)
            {

                CoolTime_Image.gameObject.SetActive(false);
                CoolTime_Num.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }


    }

}
