using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Passerby_Run : StateMachineBehaviour
{
    Npc npc;
    NavMeshAgent nav;
    TextMeshProUGUI talk;
    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (npc == null)
            npc = animator.GetComponent<Npc>();
        if (nav == null)
            nav = animator.GetComponent<NavMeshAgent>();
        if(npc.NpcTalk != null)
        {
            talk = npc.NpcTalk;
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nav.SetDestination(new Vector3(48.2f, 0, 49.7f));



        if (nav.destination == nav.transform.position)
        {
            npc.NpcTalk = null;
            talk.gameObject.SetActive(false);
            animator.SetBool("IsMove", false);
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
