using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Rush : StateMachineBehaviour
{
    Character character;
    Vector3 targetPos;
    NavMeshAgent nav;
    TutorialBoss boss;
    float timer;
    

    bool isEnd;
    bool isHit;


    int HitCount = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null)
        {
            character = GameManager.gameManager.character;
        }
        if(nav == null)
        {
            nav = animator.GetComponent<NavMeshAgent>();
        }
        if(boss ==null)
        {
            boss = animator.GetComponent<TutorialBoss>();
        }        

        Vector3 tempPos = boss.transform.position + boss.transform.forward * 10;
        NavMeshHit hitinfo;
        if(nav.Raycast(tempPos,out hitinfo))
        {
            targetPos = hitinfo.position;
        }
        else
        {
            targetPos = tempPos;
        }
        isEnd = false;
        isHit = false;        
        timer = 0f;
        nav.updatePosition = false;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPos, 0.05f);
        timer += Time.deltaTime;
        if(boss.transform.position == targetPos&& isEnd == false&& timer>=3f)
        {
            isEnd = true;                 
            animator.SetTrigger("RushEnd");
        }



        if(isHit == false && boss.DISTANCE <=1)
        {
            isHit = true;
            boss.StartCoroutine(CoHitControl());
            bool isCri = boss.IsCri();
            character.stat.Damaged(isCri, boss.AttackDmg(isCri));
            Debug.Log("È÷Æ® :" + HitCount++);

        }

    }
    IEnumerator CoHitControl()
    {
        yield return new WaitForSeconds(1f);
        isHit = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nav.Warp(targetPos);
        nav.updatePosition = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
