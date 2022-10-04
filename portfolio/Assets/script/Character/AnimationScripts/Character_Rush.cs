using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character_Rush : StateMachineBehaviour
{
    Character character;
    NavMeshAgent nav;
    ParticleSystem effect;    
    Vector3 destination;   

    HashSet<GameObject> hitMonster;


    float timer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (character == null || nav == null || effect == null|| hitMonster == null)
        {
            character = animator.GetComponent<Character>();
            nav = animator.GetComponent<NavMeshAgent>();
            GameObject effectobj = ResourceManager.resource.GetEffect("attack04");
            effectobj.transform.SetParent(character.transform);
            effectobj.transform.localPosition = new Vector3(0, 0.7f, 4.297f);
            effectobj.transform.localRotation = new Quaternion(0,0,0,0);
            effect = effectobj.GetComponent<ParticleSystem>();
            effect.gameObject.SetActive(false);
            
            hitMonster = character.hitMob;
        }

        character.StopMove();
        character.skill.isUsingSkill = true;

        Monster closestMob = character.ClosestMonster(4f);                    // closestMob direction
        
        Vector3 tempPos;
        if(closestMob == null)
        {
            
            tempPos = character.transform.position + character.transform.forward * 4f;
        }
        else
        {
            character.transform.LookAt(closestMob.transform);
            tempPos = closestMob.transform.position;
        }

        NavMeshHit hitinfo;
        if(nav.Raycast(tempPos,out hitinfo))
        {
            destination = hitinfo.position;
        }
        else
        {
            destination = tempPos;
        }

        timer = 0f;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        
        if(timer >= 0.7f )
        {
            Collider[] mobs = Physics.OverlapBox(character.transform.position + character.transform.forward * 1f + character.transform.up * 0.9f, new Vector3(0.5f, 0.5f, 1f));
            if (mobs != null)
            {
                for (int i = 0; i < mobs.Length; i++)
                {
                    if (mobs[i].tag == "Monster" && !hitMonster.Contains(mobs[i].gameObject))
                    {
                        Monster mob = mobs[i].gameObject.GetComponent<Monster>();
                        mob.KnockBack();
                        bool iscri = character.stat.DamageType();
                        mob.Damaged(iscri, character.stat.AttckDamage(iscri));
                        hitMonster.Add(mobs[i].gameObject);
                    }
                }
            }


            character.transform.position = Vector3.MoveTowards(character.transform.position, destination, 0.5f);

            if(effect.gameObject.activeSelf == false)
            {
                effect.gameObject.SetActive(true);
                effect.Play();
            }


            
            
        }
        
        if(character.transform.position == destination && timer >= 1.2f)
        {
            animator.SetBool("Skill", false);
        }

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.CanMove();        
        
        animator.SetInteger("SkillNum", -1);
        effect.gameObject.SetActive(false);

        
        character.skill.isUsingSkill = false;
        hitMonster.Clear();
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
