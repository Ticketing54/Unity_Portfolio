using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_Scream : StateMachineBehaviour
{
    TutorialBoss boss;
    GameObject iceField;
    
    AudioSource sound;
    GameObject screamsound;
    Character character;

    float timer;
    bool isHit;
    bool soundPlay;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss == null)
            boss = animator.GetComponent<TutorialBoss>();

        if (iceField == null||sound == null)
        {
            iceField = ResourceManager.resource.GetEffect("IceField");            
            iceField.transform.position = boss.transform.position;
            iceField.SetActive(false);
            sound = SoundManager.soundmanager.GetSounds("Shout");
            sound.gameObject.transform.SetParent(boss.transform);
            sound.gameObject.transform.localPosition = Vector3.zero;
        }
        
        if (character == null)
        {
            character = GameManager.gameManager.character;
        }
        isHit = false;
        soundPlay = false;
        timer = 0f;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timer += Time.deltaTime;

        if(timer>=1f && soundPlay == false)
        {
            soundPlay = true;
            sound.Play();                
        }

        if (timer >= 1f)
        {
            if(iceField.gameObject.activeSelf == false)
            {
                iceField.gameObject.SetActive(true);
                iceField.gameObject.transform.position = boss.transform.position;
            }
            
            

            if (boss.Distance > 8f && boss.Distance < 12f && isHit == false)
            {
                isHit = true;
                boss.StartCoroutine(CoHitControl());
                bool isCri = boss.IsCri();
                character.stat.Damaged(isCri, boss.AttackDmg(isCri));
            }








        }
    }
   
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        soundPlay = false;
        iceField.gameObject.SetActive(false);       
    }

    IEnumerator CoHitControl()
    {
        yield return new WaitForSeconds(1f);
        isHit = false;
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
