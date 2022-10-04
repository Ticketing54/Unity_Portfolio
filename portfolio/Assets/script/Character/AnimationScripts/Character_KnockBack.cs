using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character_KnockBack : StateMachineBehaviour
{
    Character character;
    NavMeshAgent nav;
    Vector3 targetPos;
    float timer;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null || nav == null)
        {
            character = animator.GetComponent<Character>();
            nav = animator.GetComponent<NavMeshAgent>();
        }
        nav.ResetPath();
        timer = 0f;

        Vector3 tempPos = character.transform.position + character.transform.forward * -1;
        NavMeshHit hitinfo;
        if (nav.Raycast(tempPos, out hitinfo))
        {
            targetPos = hitinfo.position;
        }
        else
        {
            targetPos = tempPos;
        }
        character.IsPossableControl = false;    
        nav.updatePosition = false;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;


        if(character.transform.position != targetPos)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetPos, 0.05f);
        }
        
        if(timer >= 1f)
        {
            animator.SetTrigger("KnockBackEnd");
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.IsPossableControl = true;
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
