using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Flying : StateMachineBehaviour
{
    Character character;
    AudioSource audio;
    GameObject screamsound;
    TutorialBoss boss;
    NavMeshAgent nav;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (screamsound == null)
        {
            screamsound = new GameObject("DraonFly");
            screamsound.transform.SetParent(animator.gameObject.transform);
            audio = screamsound.AddComponent<AudioSource>();
            audio.clip = Resources.Load<AudioClip>("Sounds/DraonFly");
            audio.loop = true;
            audio.Play();
        }

        if(boss == null)
        {
            boss = animator.GetComponent<TutorialBoss>();
            nav = animator.GetComponent<NavMeshAgent>();
            character = GameManager.gameManager.character;
        }

        
        audio.gameObject.SetActive(true);
        nav.updateRotation = false;
        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 dir = (character.transform.position - animator.transform.position).normalized;
        Vector3 newdir = Vector3.RotateTowards(boss.transform.forward, dir, Time.deltaTime * 30, Time.deltaTime * 30);
        boss.transform.rotation = Quaternion.LookRotation(newdir);
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audio.gameObject.SetActive(false);
        nav.updateRotation = true;
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
