using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NomalMonsterCombat : StateMachineBehaviour
{
    Monster mob;
    Character character;
    NavMeshAgent nav;
    bool attack = false;
    float timer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mob == null||nav == null||character == null)
        {
            mob = animator.GetComponent<Monster>();
            nav = animator.GetComponent<NavMeshAgent>();
            character = GameManager.gameManager.character;
        }
        timer = 0;        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(attack == true)
        {
            return;
        }

        timer += Time.deltaTime;
        if(mob.Distance<=mob.Range)
        {
            mob.transform.LookAt(character.transform);
            if (timer >= 1f)
            {                
                animator.SetBool("Attack", true);
                attack = true;
            }            
        }
        else
        {
            nav.SetDestination(character.transform.position);
            animator.SetBool("Move", true);
        }
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attack = false;
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
