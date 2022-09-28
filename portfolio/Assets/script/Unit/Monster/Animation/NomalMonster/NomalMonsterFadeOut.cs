using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonsterFadeOut : StateMachineBehaviour
{
    Material mater;
    Monster mob;
    Color fade;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mater == null || mob == null)
        {
            mater = animator.GetComponent<Material>();
            mob = animator.GetComponent<Monster>();
        }

        fade = mater.color;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fade.a -= Time.deltaTime;
        mater.color = fade;


        if (fade.a <= 0)
        {
            mob.dropItem = false;            
            mob.HpCur = mob.HpMax;
            ObjectManager.objManager.StartRespawnMob(mob);
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
