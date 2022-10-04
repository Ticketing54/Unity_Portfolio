using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Move : StateMachineBehaviour
{
    TutorialBoss boss;
    NavMeshAgent nav;
    Character character;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<TutorialBoss>();

        if (nav == null)
            nav = animator.GetComponent<NavMeshAgent>();
        character = GameManager.gameManager.character;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(boss.Distance< boss.Range)
        {
            nav.destination = boss.transform.position;
            animator.SetBool("Move", false);
        }
        else
        {
            nav.destination = character.transform.position;
        }

        //if (mob.DiSTANCE < 6f)
        //{
        //    nav.destination = mob.transform.position;
        //    animator.SetBool("Move", false);
        //}            
        //else
        //{
        //    nav.destination = Character.Player.transform.position;
        //}
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
