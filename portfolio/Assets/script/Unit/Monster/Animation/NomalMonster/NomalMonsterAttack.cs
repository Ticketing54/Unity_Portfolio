using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonsterAttack : StateMachineBehaviour
{   
    Monster mob;
    Character character;
    float timer;
    bool hit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(mob == null||character == null)
        {
            mob = animator.GetComponent<Monster>();
            character = GameManager.gameManager.character;           
        }
        timer = 0f;
        hit = false;
        animator.SetBool("Attack", false);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if(timer >= 0.15f && timer <=0.22f&& hit == false)
        {
            Collider[] hitcharacter = Physics.OverlapBox(mob.transform.position + mob.transform.forward  + mob.transform.up, new Vector3(0.5f, 0.5f, 2f));

            for (int i = 0; i < hitcharacter.Length; i++)
            {
                if(hitcharacter[i].tag == "Player")
                {
                    hit = true;
                    bool isCri = mob.IsCri();
                    character.stat.Damaged(isCri, mob.AttackDmg(isCri));
                }
            }
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
