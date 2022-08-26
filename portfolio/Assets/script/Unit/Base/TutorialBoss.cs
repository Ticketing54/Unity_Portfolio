using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoss : Monster
{
    [SerializeField]
    Character character;
    
    int patternNum = 0;
    bool combatPhase = false;
    GameObject breath;
    SkinnedMeshRenderer breathRange;
    GameObject gunfire;
    bool isStart = false;
    void Start()
    {   
        breath = gameObject.transform.Find("breath").gameObject;
        breathRange = breath.GetComponent<SkinnedMeshRenderer>();
        gunfire = GameObject.FindGameObjectWithTag("MobWeapon");
        range = 1.5f;
    }
    public override float Hp_Curent
    {
        get
        {
            return hp_Cur;
        }
        set
        {
            hp_Cur = value;

            if (hp_Cur <= 250 && combatPhase == false)
            {
                combatPhase = true;
                StopCoroutine(action);
                //action = startcouroutine(±¤ÆøÈ­);
            }
            else if (hp_Cur <= 0)
            {
                // Á×À½ ¾Ö´Ï¸ÞÀÌ¼Ç
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(isStart == false)
            {
                isStart = true;
                MoveStart();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            patternNum = 2;
        }
    }
    public void MoveStart()
    {
        if(action != null)
        {
            StopCoroutine(action);
        }
        action = StartCoroutine(CoStart());
    }
    IEnumerator CoStart()
    {
        yield return CoMove(range);
        action = StartCoroutine(CoNomalAttack());
    }
    protected IEnumerator CoMove(float _distance)
    {   
        nav.SetDestination(character.transform.position);
        nav.stoppingDistance = _distance;
        yield return null;

        if (nav.remainingDistance > _distance)
        {
            anim.SetBool("Move", true);
        }
        while (true)
        {
            yield return null;

            Vector3 dir = (nav.steeringTarget - transform.position).normalized;
            dir.y = 0;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);

            if (nav.remainingDistance <= _distance && nav.velocity.magnitude == 0f)
            {
                nav.SetDestination(transform.position);
                nav.stoppingDistance = 0;                            
                anim.SetBool("Move", false);
                yield break;
            }
        }
    }
   
    IEnumerator CoStandBreath()
    {
        anim.SetTrigger("Attack");
        float timer = 0f;
        float vr = 1;
        Vector3 dir;
        float count = 0;
        while (timer <= 2.2f)
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                breath.gameObject.SetActive(true);
                breath.gameObject.transform.position = gunfire.transform.position;
                vr -= (float)Time.deltaTime;
                dir = BressDir(this.gameObject, vr) - breath.transform.localPosition;
                breath.transform.rotation = Quaternion.LookRotation(dir.normalized);
                if (breathRange.bounds.Intersects(character.AttackBox))
                {
                    Debug.Log(count++);


                    //Character.Player.Stat.HP -= mob.Atk;

                    //ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, mob.Atk, Color.red, 1);
                }




                yield return null;
            }
            yield return null;
        }
        breath.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
    }
    Vector3 BressDir(GameObject tmp, float value)
    {
        Vector3 a, b, c, d, e;
        a = tmp.transform.right * -20f + tmp.transform.up * 5f;
        b = a + tmp.transform.forward * 20f;
        c = tmp.transform.right * 20f + tmp.transform.up * 5f;
        d = c + tmp.transform.forward * 20f;

        e = BezierTest(a, b, d, c, value);
        return e;

    }
    Vector3 BezierTest(Vector3 P_1, Vector3 P_2, Vector3 P_3, Vector3 P_4, float value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 C = Vector3.Lerp(P_3, P_4, value);

        Vector3 D = Vector3.Lerp(A, B, value);
        Vector3 E = Vector3.Lerp(B, C, value);

        Vector3 F = Vector3.Lerp(D, E, value);

        return F;
    }

   
   
    int pattern = 0;

    #region phase 1   
    IEnumerator CoNomalAttack()
    {   
        int pattern = patternNum++;
        anim.SetFloat("Attack_num", pattern);              
        if (pattern >= 2)
        {   
            yield return StartCoroutine(CoStandBreath());
            patternNum = 0;
            action = StartCoroutine(CoMove(range));
        }
        else
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(6f);
            if (patternNum >= 2)
            {
                yield return StartCoroutine(CoMove(5f));                
            }
            else
            {
                yield return StartCoroutine(CoMove(range));                
            }
        }

        
        
        action = StartCoroutine(CoNomalAttack());        
        yield return null;        
    }
    IEnumerator CoLading()
    {
        yield return null;
    }


    #endregion
    #region phase 2
    protected override IEnumerator CoCombat()
    {
        yield return null;
    }
    #endregion

    void Combat()
    {
        switch (pattern)
        {
            case 0:         // °ø°Ý
                break;
            case 1:         // 
                break;
            case 2:
                break;
        }
    }
    protected override IEnumerator CoDie()
    {
        throw new System.NotImplementedException();
    }

    public override void DropItem()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator CoIdle()
    {
        throw new System.NotImplementedException();
    }

    public override void Damaged(bool _type, int _dmg)
    {
        throw new System.NotImplementedException();
    }

    public override void StatusEffect(STATUSEFFECT _state, float _duration)
    {
        throw new System.NotImplementedException();
    }

  
}
