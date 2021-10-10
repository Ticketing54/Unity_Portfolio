using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic_Idle_Hide : StateMachineBehaviour
{
    Monster mob;
    public bool TouchMob = false;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
        {
            mob = animator.GetComponent<Monster>();
            mob.tag="Item";
        }
        animator.SetBool("FindMob", false);
        animator.SetBool("DropItem",false);
        animator.SetBool("IsDie", false);
        mob.Hp = mob.Hp_max;
        TouchMob = false;
        mob.tag = "Item";
        
        
            

        

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (Character.Player.Interaction_L == true && mob.DiSTANCE < 2f && TouchMob == false)
        {
            TouchMob = true;
            SoundManager.soundmanager.soundsPlay("Character");
            Character.Player.Stat.HP -= 10f;
            ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, 10f,Color.red,1);
            animator.SetBool("FindMob", true);
            Character.Player.anim.SetTrigger("Damage");
            mob.tag = "Monster";

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

