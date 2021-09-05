using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character_Skill : StateMachineBehaviour
{

    float Skill_num = -1;
    NavMeshAgent nav;
    Vector3 End = Vector3.zero;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (nav == null)
            nav = Character.Player.nav;
        Character.Player.DontMove = true;
        Character.Player.Target = null;
        Skill_num = animator.GetFloat("SkillNum");

        if(Skill_num == 1)
        {
            End = Character.Player.transform.forward*4f + Character.Player.transform.position;
        }
      
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(Skill_num == 1)
        {
            nav.speed = 7f ;
            nav.avoidancePriority = 51;
            nav.destination = End;





        }
        

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Character.Player.DontMove = false;
        if (Skill_num == 1)
        {
            nav.avoidancePriority = 50;
            nav.speed = 7f;
        }
        animator.SetFloat("SkillNum", 0);
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
