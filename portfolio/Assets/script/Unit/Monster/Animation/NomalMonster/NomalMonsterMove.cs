using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NomalMonsterMove : StateMachineBehaviour
{
    Character character;
    NavMeshAgent nav;
    Monster mob;
    float agro = 5f;
    float agroWait;
    bool reset = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null||mob==null||nav == null)
        {
            character = GameManager.gameManager.character;
            nav = animator.GetComponent<NavMeshAgent>();
            mob = animator.GetComponent<Monster>();
        }
        agroWait = 5f;        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
        if(reset == true)
        {
            return;
        }

        if (mob.Distance <= mob.Range)
        {
            animator.SetBool("Move", false);
            nav.ResetPath();
        }
        else
        {
            nav.SetDestination(character.transform.position);
            if (agroWait <= 0)
            {
                agro -= Time.deltaTime;
                if (agro <= 0)
                {
                    reset = true;
                    agro = 5f;
                    animator.SetBool("Reset", true);
                }
            }
            else
            {
                agroWait -= Time.deltaTime;
            }
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        reset = false;
        
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
