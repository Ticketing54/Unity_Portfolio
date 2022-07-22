using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Unit
{
    protected NavMeshAgent nav;
    protected Animator anim;
    protected Coroutine action;
    [SerializeField]
    public Material material;

    
    
    public Collider hitBox;

    protected int index = -1;
    protected float hp_Max;
    protected float hp_Cur;
    protected float atk;
    protected float range;
    protected int lev;
    protected int gold;
    protected int exp;

    protected List<List<int>> itemDropInfo;
    protected List<Item> dropItem;

    
    
    public float Hp_Curent
    {
        get
        {
            return hp_Cur;
        }

        set
        {
            hp_Cur = value;     
            if(hp_Cur <= 0)
            {
                Die();                
            }
        }
    }

    protected void Die()
    {
        StopCoroutine(action);
        gameObject.tag = "Item";
        anim.SetBool("IsDie", true);        
        GameManager.gameManager.character.GetReward_Monster(gold, exp);
        GameManager.gameManager.character.quest.UpdateQuest_Monster(index);

        SetItem();
        if (dropItem == null)
        {
            gameObject.tag = "Untagged";
            StartCoroutine(CoFadeOut());
        }

    }
    IEnumerator CoFadeIn()
    {
        nav.Warp(startPos);
        anim.SetTrigger("Respawn");

        Color color = material.color;
        float alpha = 0;
        while (alpha <1)
        {
            alpha += Time.deltaTime;
            color.a = alpha;
            material.color = color;
            yield return null;
        }
        gameObject.tag = "Monster";
    }
    IEnumerator CoFadeOut()
    {        
        Color color = material.color;
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            color.a = alpha;
            material.color = color;
            yield return null;
        }
        anim.SetBool("IsDie", false);        
        ObjectManager.objManager.StartRespawnMob(this);
    }
    public float Hp_Max { get => hp_Max; }


    public virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hitBox = GetComponent<Collider>();
        material = transform.Find("Render").GetComponent<SkinnedMeshRenderer>().material;
        


    }
    public override void OnEnable()
    {
        StartCoroutine(CoFadeIn());
        base.OnEnable();        
    }
    public override void OnDisable()
    {
        ResetInfo();
        base.OnDisable();        
    }
    public virtual void DropItem()
    {
        StartCoroutine(CoFadeOut());
        GameManager.gameManager.character.OpenDropBox(dropItem);                      
    }

    
    public virtual void ResetInfo()
    {
        hp_Cur = hp_Max;        
    }
   
    void SetItem()
    {   
        if(itemDropInfo == null)
        {
            return;
        }

        List<Item> newItems = new List<Item>();
        for (int i = 0; i < itemDropInfo.Count; i++)
        {
            List<int> itemInfo = DropItemList(itemDropInfo[i]);

            newItems.Add(new Item(itemInfo[0], itemInfo[1]));
        }
        
        dropItem = newItems;
    }
    
    List<int> DropItemList(List<int> _itemDropInfo)
    {
        int itemIndex       = _itemDropInfo[0];
        int itemDropPercent = _itemDropInfo[1];
        int itemMaxCount    = _itemDropInfo[2];

        int dropcount = 1;
        for(int itemcount = 1; itemcount < itemMaxCount; itemcount++)
        {
            int drop = Random.Range(0, 101);
            if(drop <= itemDropPercent)
            {
                dropcount++;
            }
        }
        List<int> dropList = new List<int>();
        dropList.Add(itemIndex);
        dropList.Add(dropcount);

        return dropList;


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

        if (GameManager.gameManager.character.stat.Level < this.lev)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public virtual void SetMonster(int _index, Vector3 _startPos)
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

        
        if (string.IsNullOrEmpty(tableInfo[9]))
        {
            Debug.Log("아이템 없음");
        }
        else
        {
            string[] infoArr = tableInfo[9].Split('/');
            List<List<int>> t_dropList = new List<List<int>>();
            for (int i = 0; i < infoArr.Length; i++)
            {
                string[]iteminfo = infoArr[i].Split('#');

                List<int> t_itemList = new List<int>();
                t_itemList.Add(int.Parse(iteminfo[0]));
                t_itemList.Add(int.Parse(iteminfo[1]));
                t_itemList.Add(int.Parse(iteminfo[2]));
                t_dropList.Add(t_itemList);
            }
            itemDropInfo = t_dropList;
        }
        


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
        nav.Warp(_startPos);
    }

    public virtual void Damaged(DAMAGE _type, float _dmg)
    {
        if(Hp_Curent <= 0)
        {
            return;
        }

        float finalyDmg = 0;
        
        switch (_type)
        {
            case DAMAGE.NOMAL:
                {
                    finalyDmg = _dmg;
                    Hp_Curent -= finalyDmg;
                }
                break;
            case DAMAGE.CRITICAL:
                {
                    finalyDmg = _dmg * 2;
                    Hp_Curent -= finalyDmg;
                }
                break;
        }

        if(Hp_Curent <= 0)
        {
            Die();
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
        if(action != null)
        {
            StopCoroutine(action);
        }
        
        yield return StartCoroutine(CoKnockBack());
        action = StartCoroutine(CoCombat());
    }
    public void StatusEffect(STATUSEFFECT _state, float _duration)
    {
        switch (_state)
        {
            case STATUSEFFECT.KNOCKBACK:
                {
                    KnockBack();
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
        anim.SetBool("IsMove", true);

        nav.SetDestination(startPos);
        float recoveryHp = hp_Max / 5;
        while (true)
        {
            yield return null;
            if (hp_Cur != hp_Max)
            {
                Hp_Curent += recoveryHp;
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
    void KnockBack()
    {
        // 넉백 애니메이션
        StartCoroutine(CoKnockBack());
    }
    protected IEnumerator CoKnockBack()
    {
        float timer = 0f;
        nav.updateRotation = false;
        transform.LookAt(GameManager.gameManager.character.transform);
        Vector3 dir = (transform.position - GameManager.gameManager.character.transform.position).normalized;
        nav.speed = 5;
        nav.SetDestination(transform.position + dir * 1f);
        while (timer<0.5f)
        {
            yield return null;

            timer += Time.deltaTime;
        }
        nav.speed = 3.5f;
        nav.updateRotation = true;
        yield return null;
    }



}


