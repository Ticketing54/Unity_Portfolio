using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Phase1 : StateMachineBehaviour
{
    Character character;
    TutorialBoss boss;
    bool isAttack;
    float delay;
    int phaseNumber = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character = GameManager.gameManager.character;
        boss = animator.GetComponent<TutorialBoss>();
        isAttack = false;
        delay = 0;
        if(phaseNumber > 2)
        {
            phaseNumber = 0;
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delay += Time.deltaTime;
        if (delay >= 1f)
        {
            if (boss.Distance < boss.Range)
            {
                if(isAttack == false)
                {
                    isAttack = true;
                    SetStopDistance();
                    animator.SetFloat("PhaseNumber", phaseNumber++);
                    animator.SetTrigger("Attack");
                }                
            }
            else
            {
                animator.SetBool("Move", true);
            }
        }        
    }
    void SetStopDistance()
    {
        switch(phaseNumber)
        {
            case 0:
                boss.Range = 1.5f;
                break;
            case 1:
                boss.Range = 5f;
                break;
            case 2:
                boss.Range = 1.5f;
                break;
            default:
                Debug.Log("Wrong Number : Boss_Phase");
                break;
        }
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
