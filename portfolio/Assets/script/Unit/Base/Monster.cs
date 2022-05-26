using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Unit
{
    protected NavMeshAgent nav;
    protected Animator anim;
    protected Coroutine action;
    
    public Collider hitBox;

    protected int index = -1;
    protected float hp_Max;
    protected float hp_Cur;
    protected float atk;
    protected float range;
    protected int lev;
    protected int gold;
    protected int exp;
    
    protected List<int[]> items = new List<int[]>();
    public float HP_CURENT { get => hp_Cur; }
    public float Hp_Max { get => hp_Max; }


    public override void Awake()
    {
        base.Awake();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hitBox = GetComponent<Collider>();
    }

    public override void Start()
    {
        base.Start();
        nav.Warp(startPos);
    }


    public void Attack(int _type)
    {
        switch (_type)
        {
            case 0:
                {
                    GameManager.gameManager.character.Damaged(DAMAGE.NOMAL, atk);
                    break;
                }
            case 1:
                {
                    GameManager.gameManager.character.Damaged(DAMAGE.CRITICAL, atk * 2);
                    break;
                }
        }

    }

    protected void SeleteAttack()
    {
        int atknum = Random.Range(0, 101);

        if (atknum < 5)
        {
            anim.SetFloat("Attack_num", 2);
        }
        else
        {
            anim.SetFloat("Attack_num", 1);
        }
        anim.SetTrigger("Attack");
    }

    public bool MightyEnermy()
    {
        if (GameManager.gameManager.character == null)
            return false;

        if (GameManager.gameManager.character.stat.LEVEL < this.lev)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void SetMonster(int _index, Vector3 _startPos)
    {
        index = _index;


        List<string> tableInfo = ResourceManager.resource.GetTable_Index("MonsterTable", _index);


        unitName = tableInfo[2];


        float t_NickPos;
        if (float.TryParse(tableInfo[3], out t_NickPos))
        {
            nick_YPos = t_NickPos;
        }
        else
        {
            Debug.LogError("NickPos 변환 오류");
        }


        if (string.IsNullOrEmpty(tableInfo[4]))
        {
            Debug.LogError("소리파일이름 없음");
        }
        else
        {
            sound = tableInfo[4];
        }


        int t_Lev;
        if (int.TryParse(tableInfo[5], out t_Lev))
        {
            lev = t_Lev;
        }
        else
        {
            Debug.LogError("Lev 변환 오류");
        }


        float t_HpMax;
        if (float.TryParse(tableInfo[6], out t_HpMax))
        {
            hp_Max = t_HpMax;
        }
        else
        {
            Debug.LogError("HpMax 변환 오류");
        }

        hp_Cur = hp_Max;

        float t_Atk;
        if (float.TryParse(tableInfo[7], out t_Atk))
        {
            atk = t_Atk;
        }
        else
        {
            Debug.LogError("Atk 변환 오류");
        }

        float t_Range;
        if (float.TryParse(tableInfo[8], out t_Range))
        {
            range = t_Range;
        }
        else
        {
            Debug.LogError("Range 변환 오류");
        }

        //
        if (string.IsNullOrEmpty(tableInfo[9]))
        {
            Debug.Log("아이템 없음");
        }
        else
        {
            string itemInfo = tableInfo[9];
        }
        //


        if (string.IsNullOrEmpty(tableInfo[10]))
        {
            gold = 0;
        }
        else
        {
            int t_gold;
            if (int.TryParse(tableInfo[10], out t_gold))
            {
                gold = t_gold;
            }
            else
            {
                Debug.LogError("Gold 변환 오류");
            }

        }

        if (string.IsNullOrEmpty(tableInfo[11]))
        {
            gold = 0;
        }
        else
        {
            int t_Exp;
            if (int.TryParse(tableInfo[11], out t_Exp))
            {
                exp = t_Exp;
            }
            else
            {
                Debug.LogError("Exp 변환 오류");
            }
        }

        startPos = _startPos;
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


    protected virtual IEnumerator CoCombat()
    {
        float agro = 5f;
        while (true)
        {
            if (DISTANCE <= range)
            {
                nav.SetDestination(transform.position);
                anim.SetBool("IsMove", false);


                transform.LookAt(GameManager.gameManager.character.transform.position);

                SeleteAttack();

                yield return new WaitForSeconds(1f);
            }
            else if (DISTANCE > range && DISTANCE < 10f)
            {
                anim.SetBool("IsMove", true);
                nav.SetDestination(GameManager.gameManager.character.transform.position);
            }
            else if (DISTANCE >= 10f)
            {
                agro -= Time.deltaTime;

                if (agro <= 0)
                {
                    action = StartCoroutine(CoResetMob());
                    yield break;
                }

                if (nav.destination != transform.position)
                {
                    nav.SetDestination(transform.position);
                    anim.SetBool("IsMove", false);
                }

            }
            yield return null;
        }
    }
    protected virtual IEnumerator CoDamaged()
    {
        anim.SetTrigger("Damaged");
        yield return StartCoroutine(CoKnockBack(1f));
        action = StartCoroutine(CoCombat());
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
    protected virtual IEnumerator CoResetMob()
    {
        gameObject.tag = "Item";
        anim.SetBool("IsMove", true);

        nav.SetDestination(startPos);
        float recoveryHp = hp_Max / 5;
        while (true)
        {
            yield return null;
            if (hp_Cur != hp_Max)
            {
                hp_Cur += recoveryHp;
            }

            if (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f)
            {
                break;
            }
        }
        anim.SetBool("IsMove", false);        
        action = null;
    }
    void Sturn(float _duration)
    {
        // 스턴 애니메이션
        StartCoroutine(CoStrun(_duration));
    }
    IEnumerator CoStrun(float _duration)
    {
        //isstrun = true;
        Debug.Log("스턴");
        yield return new WaitForSeconds(_duration);
        Debug.Log("스턴해제");
        //isstrun = false;
    }
    void KnockBack(float _duration)
    {
        // 넉백 애니메이션
        StartCoroutine(CoKnockBack(_duration));
    }
    protected IEnumerator CoKnockBack(float _duration)
    {
        float timer = 0f;
        nav.ResetPath();
        Vector3 dir = transform.position - GameManager.gameManager.character.transform.position;

        nav.velocity += dir.normalized *2;

        yield return null;
    }



}


