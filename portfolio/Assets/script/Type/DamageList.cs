using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNode
{
    public DamageNode nextNode;
    public DamageNode prevNode;
    public Monster monster;
    
    public DamageNode(Monster _mob)
    {
        monster = _mob;
        nextNode = null;
        prevNode = null;
    }
}
public class DamageList
{
    public delegate void DamagedType();

    DamageNode firstNode;
    DamageNode lastNode;
    float count;    
    public DamageList()
    {
        firstNode = null;
        lastNode = null;
        count = 0;
    }


    public void Add(Monster _monster)
    {
        if(firstNode == null)
        {
            DamageNode rootNode = new DamageNode(_monster);
            firstNode = rootNode;
            lastNode = rootNode;
        }
        else
        {
            DamageNode currentNode = new DamageNode(_monster);
            currentNode.prevNode = lastNode;
            lastNode.nextNode = currentNode;
            lastNode = currentNode;            
        }
        count++;
    }

    public void DamedMonster (float _damage, STATUSEFFECT _statusEffect,float _duration)
    {
        if(count == 0)
        {
            return;
        }
        DamageNode target = firstNode;

        while (target != null)
        {
            if (Character.Player.DamageMob(0, target.monster))
            {
                target.monster.Damaged(_damage);
                target.monster.StatusEffect(_statusEffect, _duration);
                if(count == 1)
                {
                    firstNode = null;
                }

                if(target.nextNode != null)
                {
                    target.nextNode.prevNode = target.prevNode;
                }
                if(target.prevNode != null)
                {
                    target.prevNode.nextNode = target.nextNode;
                }
                
                count--;
            }
            target = target.nextNode;
        }
        
    }
}
