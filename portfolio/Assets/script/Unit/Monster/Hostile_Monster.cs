using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile_Monster : Monster
{
    bool contact = false;
    public override void Start()
    {
        base.Start();
        action = StartCoroutine(CoCombat());        
    }

    public override void Damaged(DAMAGE _type, float _dmg)
    {
        if (action != null)
        {
            StopCoroutine(action);
        }

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

        if (hp_Cur > 0)
        {   
            action = StartCoroutine(CoDamaged());
        }
        else
        {

            StopCoroutine(action);
            anim.SetTrigger("Die");
        }
    }
    
    protected override IEnumerator CoCombat()
    {
        float agro = 5f;
        while (true)
        {
            if (DISTANCE < 1f)
            {
                contact = true;
                agro = 5f;
                nav.SetDestination(transform.position);
                anim.SetBool("IsMove", false);


                transform.LookAt(GameManager.gameManager.character.transform.position);

                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(1f);
            }
            else if (DISTANCE >= 1f && DISTANCE < 10f)
            {
                contact = true;
                agro = 5f;
                anim.SetBool("IsMove", true);
                nav.SetDestination(GameManager.gameManager.character.transform.position);
            }
            else if (DISTANCE >= 10f && contact == true)
            {
                agro -= Time.deltaTime;

                if (agro <= 0)
                {
                    contact = false;
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
   
}

