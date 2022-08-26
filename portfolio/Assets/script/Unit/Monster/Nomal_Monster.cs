using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct StatusEffect
{
    string name;
    float duration;
    float timer;
}

public class Nomal_Monster : Monster
{
    protected Material material;
    
    protected List<Item> dropItem;    

    bool findTarget = false;
    bool isSturn = false;
  
    public override void Awake()
    {
        base.Awake();
        material = transform.Find("Render").GetComponent<SkinnedMeshRenderer>().material;        
    }
    public override void OnEnable()
    {
        base.OnEnable();
        action = StartCoroutine(CoFadeIn());
    }
    protected IEnumerator CoFadeIn()
    {
        Color color = material.color;
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            color.a = alpha;
            material.color = color;
            yield return null;
        }
        ResetMonster();
    }
    protected IEnumerator CoFadeOut()
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
        RespawnWait();        
    }
  

    void Sturn(float _duration)
    {
        // 스턴 애니메이션
       
    }
    IEnumerator CoStrun(float _duration)
    {   
        Debug.Log("스턴");
        isSturn = true;
        yield return new WaitForSeconds(_duration);
        isSturn = false;
        Debug.Log("스턴해제");        
    }
  
    protected IEnumerator CoKnockBack()
    {   
        float timer = 0f;
        nav.updateRotation = false;
        transform.LookAt(GameManager.gameManager.character.transform);
        Vector3 dir = (transform.position - GameManager.gameManager.character.transform.position).normalized;
        nav.speed = 5;
        nav.SetDestination(transform.position + dir * 1f);
        while (timer < 0.5f)
        {
            yield return null;

            timer += Time.deltaTime;
        }
        nav.speed = 3.5f;
        nav.updateRotation = true;
        action = StartCoroutine(CoCombat());
        yield return null;
    }

  
    protected virtual void ResetMonster()
    {
        
        gameObject.tag = "Monster";
        anim.SetTrigger("Respawn");
        action = StartCoroutine(CoIdle());
    }
  
    protected virtual void Respawn()
    {
        nav.Warp(startPos);
        StartCoroutine(CoFadeIn());
    }

   
    public virtual void RespawnWait()
    {
        anim.SetBool("IsDie", false);
        hp_Cur = hp_Max;
        ObjectManager.objManager.StartRespawnMob(this);
    }

    protected virtual void SetItem()
    {
        if (itemDropInfo == null)
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

    protected virtual List<int> DropItemList(List<int> _itemDropInfo)
    {

        int itemIndex = _itemDropInfo[0];
        int itemDropPercent = _itemDropInfo[1];
        int itemMaxCount = _itemDropInfo[2];

        int dropcount = 1;
        for (int itemcount = 1; itemcount < itemMaxCount; itemcount++)
        {
            int drop = Random.Range(0, 101);
            if (drop <= itemDropPercent)
            {
                dropcount++;
            }
        }
        List<int> dropList = new List<int>();
        dropList.Add(itemIndex);
        dropList.Add(dropcount);

        return dropList;

    }

    public override void DropItem()
    {
        StartCoroutine(CoFadeOut());
        GameManager.gameManager.character.OpenDropBox(dropItem);
    }


    #region State

    protected override IEnumerator CoIdle()
    {
        yield return null;
    }

    protected override IEnumerator CoCombat()
    {
        float agro = 5f;
        while (true)
        {
            if (DISTANCE <= range)
            {
                nav.SetDestination(transform.position);
                anim.SetBool("IsMove", false);


                transform.LookAt(GameManager.gameManager.character.transform.position);

                anim.SetTrigger("Attack");

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
                    action = StartCoroutine(CoResetMonster());
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

    protected override IEnumerator CoDie()
    {   
        gameObject.tag = "Item";
        anim.SetBool("IsDie", true);
        GameManager.gameManager.character.GetReward_Monster(gold, exp);
        GameManager.gameManager.character.quest.UpdateQuest_Monster(index);

        SetItem();
        if (dropItem == null)
        {
            gameObject.tag = "Untagged";
            StopAllCoroutines();
            StartCoroutine(CoFadeOut());
        }
        yield return null;
    }

    protected virtual IEnumerator CoResetMonster()
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

    public override void Damaged(bool _isCritical , int _damage)
    {
        if (Hp_Curent <= 0)
        {
            return;
        }
        
        Hp_Curent -= _damage;
        anim.SetTrigger("Damaged");
        StatusEffect(STATUSEFFECT.KNOCKBACK, 1f);
        UIManager.uimanager.uiEffectManager.LoadDamageEffect(_damage, this.gameObject, _isCritical);
    }

    public override void StatusEffect(STATUSEFFECT _stat, float _duration)
    {
        switch (_stat)
        {
            case STATUSEFFECT.KNOCKBACK:
                {
                    if (action != null)
                    {
                        StopCoroutine(action);
                    }
                    action = StartCoroutine(CoKnockBack());
                }
                break;
            case STATUSEFFECT.STURN:
                {
                    action = StartCoroutine(CoStrun(_duration));                    
                }
                break;
            case STATUSEFFECT.SLOW:
                {
                    
                }
                break;
            default:
                break;
        }
    }

  
    #endregion
}
