using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public abstract class Monster : BattleUnit
{   
    NavMeshAgent nav;
    public Collider hitBox;
    bool isstrun = false;
    protected int index = -1;
    public override void Start()
    {
        base.Start();
        nav = GetComponent<NavMeshAgent>();
        hitBox = GetComponent<Collider>();        
    }

    public void SetMonster(int _index,string _mobName, int _lev, float _hpMax,float _atk, int _gold, int _exp, float _nickYPos,string _sound,List<int[]>_items = null)
    {
        index = _index;
        unitName = _mobName;
        hp_Max = _hpMax;
        hp_Cur = _hpMax;
        atk = _atk;
        lev = _lev;
        gold = _gold;
        exp = _exp;
        nick_YPos = _nickYPos;
        sound = _sound;
        items = _items;
    }   
    public virtual void Damaged(DAMAGE _type, float _dmg)
    {
        float finalyDmg = 0;
        switch (_type)
        {
            case DAMAGE.NOMAL:
                {
                    finalyDmg = _dmg;
                    this.hp_Cur -= finalyDmg;
                }                
                break;
            case DAMAGE.CRITICAL:
                {
                    finalyDmg = _dmg * 2;
                    this.hp_Cur -= finalyDmg;
                }
                break;
        }
        UIManager.uimanager.uiEffectManager.LoadDamageEffect(finalyDmg, this.gameObject, _type);
    }
    public void StatusEffect(STATUSEFFECT _state, float _duration)
    {
        switch (_state)
        {
            case STATUSEFFECT.KNOCKBACK:
                {
                    KnockBack(_duration);
                }
                break;
            case STATUSEFFECT.STURN:
                {
                    Sturn(_duration);
                }
                break;
            default:
                break;                
        }
    }

    void Sturn(float _duration)
    {
        // 스턴 애니메이션
        StartCoroutine(CoStrun(_duration));
    }
    IEnumerator CoStrun(float _duration)
    {
        isstrun = true;
        Debug.Log("스턴");
        yield return new WaitForSeconds(_duration);
        Debug.Log("스턴해제");
        isstrun = false;
    }
    void KnockBack(float _duration)
    {
        // 넉백 애니메이션
        StartCoroutine(CoKnockBack(_duration));
    }
    IEnumerator CoKnockBack(float _duration)
    {
        float timer = 0f;
        nav.ResetPath();
        while (timer <= _duration)
        {
            timer += Time.deltaTime;
            nav.velocity = -transform.forward * 8;
            yield return null;
        }


    }
   
   
    //public NavMeshAgent nav;
    //public Animator anim;
    
    //public GameObject HitBox;
    //public SkinnedMeshRenderer MobBounds;
    //public SkinnedMeshRenderer hitboxBounds;
    //float Dis;
    //public bool isDamage = false;
    //public bool isTarget = false;


    //public TextMeshProUGUI NickName;
    //public Image Hp_Bar;
    //public Image MiniMap_Dot;

    //public bool isburn = false;
    //public bool isIce = false;


    //public float Attack_Dmg()
    //{
    //    return (int)(Random.Range((float)(Atk * 0.8), (float)(Atk * 1.2)));
    //}

  
    //public void ItemDropState()
    //{
    //    if(MiniMap_Dot != null)
    //    {
    //        MiniMap_Dot.gameObject.SetActive(false);
    //        MiniMap_Dot = null;
    //    }
    //    if(NickName != null)
    //    {
    //        NickName.gameObject.SetActive(false);
    //        NickName = null;
    //    }
    //    if(Hp_Bar != null)
    //    {
    //        Hp_Bar.gameObject.SetActive(false);
    //        Hp_Bar = null;
    //    }
    //}

    //IEnumerator Mini_Dot()
    //{


    //    while (true)
    //    {
    //        if (UIManager.uimanager.minimap.Minimap_n.gameObject.activeSelf == true && Character.Player !=null)
    //        {
    //            if (DiSTANCE <= 15f && MiniMap_Dot == null && Hp > 0 && gameObject.tag == "Monster")
    //            {

    //                MiniMap_Dot = ObjectPoolManager.objManager.PoolingMiniDot_n();
    //                MiniMap_Dot.sprite = ResourceManager.resource.GetImage("Dot_E");

    //            }
    //            else if (DiSTANCE <= 15f && MiniMap_Dot != null)
    //            {

    //                MiniMap_Dot.rectTransform.anchoredPosition = UIManager.uimanager.minimap.MoveDotPosition(transform.position);

    //            }
    //            else if ((DiSTANCE > 15f && MiniMap_Dot != null )||(Hp<=0&&MiniMap_Dot !=null))
    //            {
    //                MiniMap_Dot.gameObject.SetActive(false);
    //                MiniMap_Dot = null;
    //            }




    //        }

    //        yield return null;
    //    }

    //}
    //IEnumerator NickUpdate()
    //{
    //    if (Nick_y == 0)
    //        yield break;


    //    while (true)
    //    {
    //        if(Character.Player != null)
    //        {
    //            if(Lev == 0)
    //            {
    //                if (DiSTANCE < 4f && NickName == null)
    //                {
    //                    NickName = ObjectPoolManager.objManager.PoolingNickName();
    //                    NickName.text = MobName;
    //                }
    //                else if (DiSTANCE < 4f && NickName != null || Character.Player.Target == this.gameObject)
    //                {
    //                    if(NickName == null)
    //                    {
    //                        NickName = ObjectPoolManager.objManager.PoolingNickName();
    //                        NickName.text = MobName;
    //                    }

    //                    NickName.color = Color.green;
    //                    NickName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, Nick_y, 0f));
    //                }
    //                else
    //                {
    //                    if(NickName != null)
    //                    {
    //                        NickName.gameObject.SetActive(false);
    //                        NickName = null;
    //                    }

    //                }

    //            }
    //            else
    //            {
    //                if (DiSTANCE < 5f && NickName == null && Hp > 0 && gameObject.tag == "Monster")
    //                {

    //                    //닉네임
    //                    NickName = ObjectPoolManager.objManager.PoolingNickName();
    //                    NickName.text = MobName;
    //                    //hp바
    //                    Hp_Bar = ObjectPoolManager.objManager.PoolingHpBar();


    //                }
    //                else if (DiSTANCE < 5f && NickName != null && gameObject.tag == "Monster" && Hp > 0)
    //                {

    //                    if (Character.Player.Stat.LEVEL < Lev) // 몬스터 레벨이 높을 경우
    //                        NickName.color = Color.red;
    //                    else if (Character.Player.Stat.LEVEL >= Lev)   //몬스터 레벨이 낮을경우
    //                        NickName.color = Color.white;

    //                    NickName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, Nick_y, 0f));
    //                    Hp_Bar.transform.position = NickName.transform.position - new Vector3(0f, 22f, 0f);
    //                    Hp_Bar.fillAmount = Hp / Hp_max;



    //                }
    //                else if(Character.Player.Target == this.gameObject && gameObject.tag == "Monster" && Hp > 0)
    //                {
    //                    if(NickName == null)
    //                    {
    //                        NickName = ObjectPoolManager.objManager.PoolingNickName();
    //                        NickName.text = MobName;
    //                    }
    //                    if (Hp_Bar == null)
    //                    {
    //                        Hp_Bar = ObjectPoolManager.objManager.PoolingHpBar();
    //                    }




    //                    if (Character.Player.Stat.LEVEL < Lev) // 몬스터 레벨이 높을 경우
    //                        NickName.color = Color.red;
    //                    else if (Character.Player.Stat.LEVEL >= Lev)   //몬스터 레벨이 낮을경우
    //                        NickName.color = Color.white;

    //                    NickName.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, Nick_y, 0f));
    //                    Hp_Bar.transform.position = NickName.transform.position - new Vector3(0f, 22f, 0f);
    //                    Hp_Bar.fillAmount = Hp / Hp_max;






    //                }
    //                else if ((DiSTANCE > 5f && NickName != null)||( NickName != null && Hp <= 0) )
    //                {
    //                    NickName.gameObject.SetActive(false);
    //                    Hp_Bar.gameObject.SetActive(false);                       
    //                    NickName = null;                        
    //                    Hp_Bar = null;                                              

    //                }

    //            }



    //        }



    //        yield return null;


    //    }

    //}
    //public void Damaged()
    //{
    //    if (isDamage == true )
    //    {
    //        isDamage = false;                    
    //        anim.SetTrigger("Damage");
    //        SoundManager.soundmanager.soundsPlay(soundsName, this.gameObject);
    //        if (Hp <= 0f)
    //        {               
    //            Hp = 0;
    //            anim.SetBool("IsDie", true);
    //            Character.Player.Stat.EXP += Exp;
    //            Character.Player.Stat.GOLD += Gold;                
    //            ObjectPoolManager.objManager.MessageEffect("EXP " + Exp.ToString() + "+");
    //            ObjectPoolManager.objManager.MessageEffect("Gold" + Gold.ToString() + "+");
    //        }
    //    }       

    //}
    //public void Attack(float _Dmg_x=1)
    //{
    //    if (hitboxBounds.bounds.Intersects(Character.Player.Character_bounds.bounds))
    //    {
    //        SoundManager.soundmanager.soundsPlay("DragonAttack", Character.Player.gameObject);
    //        float Atk_Dmg = Attack_Dmg() * _Dmg_x;

    //        if((Character.Player.Stat.HP - Atk_Dmg) <= 0)
    //        {
    //            StopCoroutine(BurnDamage());
    //            StopCoroutine(IceState());
    //            Character.Player.Stat.HP -= Atk_Dmg;
    //        }
    //        else
    //        {
    //            Character.Player.Stat.HP -= Atk_Dmg;
    //        }
    //        ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, Atk_Dmg, Color.red, 1);

    //    }

    //}


    //public void burnStateOn()  // 화상
    //{
    //    if (isburn == false)
    //    {
    //        StartCoroutine(BurnDamage());
    //    }


    //}
    //public void icestateOn()     // 빙결
    //{
    //    if (isIce == false)
    //    {
    //        StartCoroutine(IceState());
    //    }
    //}
    //IEnumerator BurnDamage()
    //{                

    //    //

    //    GameObject Effect;
    //    Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Burn");
    //    float timer = 0;
    //    float holdTime = 5;
    //    isburn = true;
    //    while (true)
    //    {
    //        timer += Time.deltaTime;
    //        if (timer >= 1)
    //        {
    //            timer -= 1;
    //            holdTime -= 1;
    //            Hp -= 5;
    //            ObjectPoolManager.objManager.LoadDamage(this.gameObject, 10f, Color.red, 1);
    //            isDamage = true;
    //        }
    //        Effect.transform.position = this.gameObject.transform.position;



    //        if (holdTime <= 0)
    //        {
    //            Effect.SetActive(false);
    //            Effect = null;
    //            isburn = false;                
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}
    //IEnumerator IceState()
    //{
    //    GameObject Effect;
    //    Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Ice");
    //    float timer = 0;
    //    isIce = true;
    //    Character.Player.nav.speed -= 4f;
    //    Character.Player.anim.speed = 0.5f;
    //    while (true)
    //    {
    //        timer += Time.deltaTime;
    //        if (timer >= 5)
    //        {
    //            Effect.SetActive(false);
    //            Effect = null;
    //            isIce = false;
    //            Character.Player.nav.speed += 4f;
    //            Character.Player.anim.speed = 1f;

    //            yield break;
    //        }
    //        Effect.transform.position = Character.Player.transform.position;




    //        yield return null;
    //    }
    //}

    //public void soundPlayer(string _Name)
    //{
    //    SoundManager.soundmanager.soundsPlay(_Name, this.gameObject);


    //}

}