using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_idle : StateMachineBehaviour
{
    Monster mob;
    float atknum = 0;
    bool attack = false;
    float Timer = 0;
    Vector3 dir = Vector3.zero;
    float IceField = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mob == null)
            mob = animator.GetComponent<Monster>();
        attack = true;


        IceField = Random.Range(0, 100f);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer += Time.deltaTime;


      
        if (mob.DiSTANCE < 8f && attack== true)
        {

            dir = Character.Player.transform.position - mob.transform.position;
            Vector3 tmp = Vector3.RotateTowards(mob.transform.forward, dir.normalized, Time.deltaTime * 5f, Time.deltaTime * 5f);
            mob.transform.rotation = Quaternion.LookRotation(tmp);
            if (IceField <= 20)
            {
                animator.SetBool("ScreamReady", true);
                return;
            }
            if (Timer >= 2f)
            {
                attack = false;
                if (atknum > 2)
                {
                    atknum = 0;
                    animator.SetBool("flyAttack", true);
                    return;
                }

                animator.SetFloat("Attack_num", atknum);
                atknum++;
                animator.SetTrigger("Attack");
            }
            
        }
        else if(mob.DiSTANCE >=8f)
        {
            animator.SetBool("Move", true);
        }
        
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer = 0;
        
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
