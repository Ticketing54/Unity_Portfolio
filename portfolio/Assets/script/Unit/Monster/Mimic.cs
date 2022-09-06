using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Nomal_Monster
{
    bool contact = false;
    
    public override void OnEnable()
    {   
        gameObject.tag = "Item";
    }
   
    public void SetContect()
    {
        gameObject.tag = "Monster";
        GameManager.gameManager.character.stat.Damaged(false,10);
        action = StartCoroutine(CoStartCombat());
        uiUpdate = StartCoroutine(CoApproachChracter());
    }

    public override void DropItem()
    {
        if(Hp_Curent <= 0)
        {
            base.DropItem();
        }
        else
        {
            SetContect();
        }
    }

   
    IEnumerator CoStartCombat()
    {   
        anim.SetBool("IsContect", true);
        yield return new WaitForSeconds(1f);

        action = StartCoroutine(CoCombat());
    }
    protected override IEnumerator CoCombat()
    {
        float agro = 5f;
        while (true)
        {
            if(DISTANCE < 1f)
            {
                contact = true;
                nav.SetDestination(transform.position);
                anim.SetBool("IsMove", false);


                transform.LookAt(GameManager.gameManager.character.transform.position);

                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(1f);
            }
            else if(DISTANCE>=1f && DISTANCE < 10f)
            {
                contact = true;
                anim.SetBool("IsMove", true);
                nav.SetDestination(GameManager.gameManager.character.transform.position);
            }
            else if (DISTANCE >= 10f && contact == true)
            {
                agro -= Time.deltaTime;

                if (agro <= 0)
                {
                    contact = false;
                    action = StartCoroutine(CoResetMonster());
                    yield break;
                }

                if(nav.destination != transform.position)
                {
                    nav.SetDestination(transform.position);
                    anim.SetBool("IsMove", false);
                }
                
            }


            yield return null;
        }
    }
    protected override IEnumerator CoResetMonster()
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
        anim.SetBool("IsContect", false);
        anim.SetBool("IsMove", false);
        action = null;
    }
}
