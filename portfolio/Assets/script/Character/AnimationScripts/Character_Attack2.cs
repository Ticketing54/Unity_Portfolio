using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Attack2 : StateMachineBehaviour
{
    ParticleSystem effect;
    Character character;
    float timer;
    HashSet<GameObject> hitmob;
    AudioSource sound;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null || effect == null||sound == null)
        {
            character = animator.GetComponent<Character>();
            GameObject effectObj = ResourceManager.resource.GetEffect("attack02");
            effectObj.transform.SetParent(character.transform);
            effectObj.transform.localPosition = new Vector3(0, 1.118f, 1.109f);
            effectObj.transform.localRotation = new Quaternion(0, 0, 0, 0);
            effect = effectObj.GetComponent<ParticleSystem>();
            effect.gameObject.SetActive(false);
            hitmob = character.hitMob;
            sound = SoundManager.soundmanager.GetSounds("Shieldhit");
            sound.gameObject.transform.SetParent(character.transform);
            sound.gameObject.transform.localPosition = new Vector3(0, 1.118f, 1.109f);
        }
        timer = 0f;
        character.isPossableMove = false;

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= 0.4f && effect.gameObject.activeSelf == false)
        {
            sound.Play();
            Collider[] mobs = Physics.OverlapBox(character.transform.position + character.transform.forward * 0.6f + character.transform.up, new Vector3(1f, 1f, 1f));
            if (mobs != null)
            {
                for (int i = 0; i < mobs.Length; i++)
                {
                    if (mobs[i].tag == "Monster" && !hitmob.Contains(mobs[i].gameObject))
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
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.isPossableMove = true;
        character.isPossableAttack = true;
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
