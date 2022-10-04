using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NomalMonsterReset : StateMachineBehaviour
{
    Monster mob;
    NavMeshAgent nav;
    float recoveryHp;
    float timer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if(mob == null|| nav == null)
        {
            mob = animator.GetComponent<Monster>();
            nav = animator.GetComponent<NavMeshAgent>();
        }

        nav.SetDestination(mob.startPos);
        recoveryHp = mob.HpMax / 5;
        timer = 0f;
        animator.SetBool("Move", false);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(timer >= 1)
        {
            mob.HpCur += recoveryHp;
            timer -= 1;
        }

        if(mob.transform.position.x == mob.startPos.x && mob.transform.position.z == mob.startPos.z&&mob.HpCur == mob.HpMax)
        {
            animator.SetBool("Reset", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
