using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Character_Sturn : StateMachineBehaviour
{
    Character character;
    NavMeshAgent nav;
    GameObject sturnEffect;
    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null || nav == null|| sturnEffect == null)
        {
            character = animator.GetComponent<Character>();
            nav = animator.GetComponent<NavMeshAgent>();
            sturnEffect = ResourceManager.resource.GetEffect("stun");
            sturnEffect.gameObject.transform.SetParent(character.transform);
            sturnEffect.gameObject.transform.localPosition = new Vector3(0f, 1.8f, 0f);
        }
        nav.ResetPath();
        timer = character.actionTime;
        character.IsPossableControl = false;
        sturnEffect.gameObject.SetActive(true);
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;


        if (timer <= 0)
        {
            animator.SetBool("Sturn", false);
        }

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sturnEffect.gameObject.SetActive(false);
        character.actionTime = 0f;
        character.IsPossableControl = true;
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
