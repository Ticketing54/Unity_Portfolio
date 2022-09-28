using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NomalMonsterDamage : StateMachineBehaviour
{
    Monster mob;
    NavMeshAgent nav;
    Character character;
    float timer;
    Vector3 targetPos;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mob == null||nav == null|| character==null)
        {
            mob = animator.GetComponent<Monster>();
            nav = animator.GetComponent<NavMeshAgent>();
            character = GameManager.gameManager.character;
        }
        timer = 0f;

        mob.transform.LookAt(character.transform);
        Vector3 tempPos = mob.transform.position + mob.transform.forward * -1;
        NavMeshHit hitinfo;
        if (nav.Raycast(tempPos, out hitinfo))
        {
            targetPos = hitinfo.position;
        }
        else
        {
            targetPos = tempPos;
        }        
        
        nav.updatePosition = false;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if(mob.transform.position != targetPos)
        {
            mob.transform.position = Vector3.MoveTowards(mob.transform.position, targetPos, 0.05f);
        }
        if (timer >= 1f)
        {
            animator.SetBool("Damaged", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Damaged", false);
        nav.updatePosition = true;
        nav.Warp(nav.gameObject.transform.position);        
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
