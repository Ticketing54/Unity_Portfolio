﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charater_Run : StateMachineBehaviour
{
    AudioSource audiosource;
    GameObject runsounds;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(runsounds == null)
        {
            runsounds = new GameObject("Run");
            runsounds.transform.SetParent(SoundManager.soundmanager.transform);
            audiosource = runsounds.AddComponent<AudioSource>();
            audiosource.loop = true;
            AudioClip _clip = Resources.Load<AudioClip>("Sounds/Run");
            audiosource.clip = _clip;
            audiosource.Play();
        }
        audiosource.Play();




        animator.SetFloat("atkNum", 0);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        runsounds.transform.position = Character.Player.transform.position;


        if(Character.Player.isIce == true &&runsounds.name !="IceRun")
        {
            runsounds.name = "IceRun";
            audiosource.Stop();
            AudioClip _clip = Resources.Load<AudioClip>("Sounds/IceRun");
            audiosource.clip = _clip;            
            audiosource.Play();
        }
        else if (Character.Player.isIce == false && runsounds.name != "Run")
        {
            runsounds.name = "Run";
            audiosource.Stop();
            AudioClip _clip = Resources.Load<AudioClip>("Sounds/Run");
            audiosource.clip = _clip;
            audiosource.Play();
        }




        if (Character.Player.isMove == false)
        {
            animator.SetBool("Move", false);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audiosource.Stop();
        
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
