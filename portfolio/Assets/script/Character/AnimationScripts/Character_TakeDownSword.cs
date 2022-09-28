using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Character_TakeDownSword : StateMachineBehaviour
{
    Character character;
    NavMeshAgent nav;
    Vector3 destination;
    ParticleSystem effect;
    float timer;
    HashSet<GameObject> hitMonster;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null || nav == null||effect == null||hitMonster==null)
        {
            character = animator.GetComponent<Character>();
            GameObject effectobj = ResourceManager.resource.GetEffect("attack03");
            effectobj.transform.SetParent(character.transform);
            effectobj.transform.localPosition = new Vector3(-0.036f, 0.7f, 0.941f);
            effectobj.transform.localRotation = new Quaternion(0, 0, 0, 0);
            effect = effectobj.GetComponent<ParticleSystem>();
            effect.gameObject.SetActive(false);
            nav = animator.GetComponent<NavMeshAgent>();
            hitMonster = character.hitMob;
        }
        character.StopMove();
        character.skill.isUsingSkill = true;

        Vector3 tempPos;
        
        tempPos = character.transform.position + character.transform.forward * 2f;        
        NavMeshHit hitinfo;
        if (nav.Raycast(tempPos, out hitinfo))
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
        if (timer >= 0.4f)
        {
            if (timer >= 1.2f)
            {
                if (effect.gameObject.activeSelf == false)
                {
                    effect.gameObject.SetActive(true);
                    effect.Play();
                }


                Collider[] mobs = Physics.OverlapSphere(destination, 2f);
                if (mobs != null)
                {
                    for (int i = 0; i < mobs.Length; i++)
                    {
                        if (mobs[i].tag == "Monster" && !hitMonster.Contains(mobs[i].gameObject))
                        {
                            Monster mob = mobs[i].gameObject.GetComponent<Monster>();
                            bool iscri = character.stat.DamageType();
                            mob.Damaged(iscri, character.stat.AttckDamage(iscri));
                            hitMonster.Add(mobs[i].gameObject);
                        }
                    }
                }
            }
           

        }
        character.transform.position = Vector3.MoveTowards(character.transform.position, destination, 0.1f);
        if (timer >= 2f)
        {
            animator.SetBool("Skill", false);
        }

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        character.CanMove();

        animator.SetInteger("SkillNum", -1);
        effect.gameObject.SetActive(false);           
        character.skill.isUsingSkill = true;
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
