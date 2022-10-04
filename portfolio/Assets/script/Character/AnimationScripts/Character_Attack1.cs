using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Character_Attack1 : StateMachineBehaviour
{
    ParticleSystem effect;
    Character character;        
    float timer;
    HashSet<GameObject> hitmob;
    AudioSource sound;
    bool isDone = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null || effect == null|| hitmob == null|| sound == null)
        {
            character = animator.GetComponent<Character>();            
            GameObject effectObj = ResourceManager.resource.GetEffect("attack01");
            effectObj.transform.SetParent(character.transform);
            effectObj.transform.localPosition = new Vector3(0, 0.914f, 0.79f);
            effectObj.transform.localRotation = new Quaternion(0, 0, 0, 0);
            effect = effectObj.GetComponent<ParticleSystem>();
            effect.gameObject.SetActive(false);
            hitmob = character.hitMob;
            sound = SoundManager.soundmanager.GetSounds("swordsounds");            
            sound.gameObject.transform.SetParent(character.transform);
            sound.gameObject.transform.localPosition = new Vector3(0, 0.914f, 0.79f);
        }
        
        timer = 0f;

        character.isPossableMove = false;        
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;


        if (timer>=0.3f && effect.gameObject.activeSelf == false)
        {
            sound.Play();
            Collider[] mobs = Physics.OverlapBox(character.transform.position + character.transform.forward * 0.6f + character.transform.up, new Vector3(1f, 1f, 1f));
            if(mobs != null)
            {
                for (int i = 0; i < mobs.Length; i++)
                {
                    if(mobs[i].tag == "Monster" && !hitmob.Contains(mobs[i].gameObject))
                    {
                        Monster mob = mobs[i].gameObject.GetComponent<Monster>();
                        bool iscri = character.stat.DamageType();
                        mob.Damaged(iscri, character.stat.AttckDamage(iscri));
                        SoundManager.soundmanager.soundsPlay(mob.Sound, mob.gameObject);
                        hitmob.Add(mobs[i].gameObject);
                    }
                }
            }
            effect.gameObject.SetActive(true);
            effect.Play();
        }

        if (timer >= 0.6f && isDone == false)
        {
            isDone = true;
            character.isPossableAttack = true;            
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        character.isPossableMove = true;
        isDone = false;
        effect.gameObject.SetActive(false);
        hitmob.Clear();
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
