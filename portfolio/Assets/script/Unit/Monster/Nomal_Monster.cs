using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Nomal_Monster : Monster
{   
    protected List<Item> dropItemList;
    
    public override float HpMax
    {
        get
        {
            return hp_Max;
        }
        set
        {
          
        }
    }
    public override float HpCur
    {
        get
        {
            return hp_Cur;
        }
        set
        {
            hp_Cur = value;
            if (hp_Cur <= 0)
            {
                Die();
                GameManager.gameManager.character.AddNearInteract(this);
            }
            else
            {
                if(hp_Cur >= hp_Max)
                {
                    hp_Cur = hp_Max;
                }   
            }
        }
    }

    #region DropItem

    public virtual void SetItem()
    {
        if (itemDropInfo == null)
        {
            return;
        }

        List<Item> newItems = new List<Item>();
        for (int i = 0; i < itemDropInfo.Count; i++)
        {
            List<int> itemInfo = DropItemList(itemDropInfo[i]);

            newItems.Add(new Item(itemInfo[0], itemInfo[1]));
        }

        dropItemList = newItems;
    }

    protected virtual List<int> DropItemList(List<int> _itemDropInfo)
    {

        int itemIndex = _itemDropInfo[0];
        int itemDropPercent = _itemDropInfo[1];
        int itemMaxCount = _itemDropInfo[2];

        int dropcount = 1;
        for (int itemcount = 1; itemcount < itemMaxCount; itemcount++)
        {
            int drop = Random.Range(0, 101);
            if (drop <= itemDropPercent)
            {
                dropcount++;
            }
        }
        List<int> dropList = new List<int>();
        dropList.Add(itemIndex);
        dropList.Add(dropcount);

        return dropList;

    }

    public override void DropItem()
    {
        dropItem = true;
        GameManager.gameManager.character.OpenDropBox(dropItemList);
        GameManager.gameManager.character.RemoveInteract(this);
    }

    #endregion


    #region State

    
    public override void Respawn()
    {
        nav.Warp(startPos);
        hp_Cur = hp_Max;
        // Ui ¿©±â¼­
        anim.SetBool("Respawn", true);
    }

    public override void Sturn(float _time)
    {
        stateTime = _time;
        anim.SetBool("Sturn", true);
    }

    void Die()
    {
        anim.SetBool("Die", true);        

        GameManager.gameManager.character.GetReward_Monster(gold, exp);
        GameManager.gameManager.character.quest.UpdateQuest_Monster(index);

        SetItem();
        if (dropItemList == null)
        {
            dropItem = true;
        }
        else
        {
            gameObject.tag = "Item";

            if (Distance < 6f)
            {
                GameManager.gameManager.character.AddNearInteract(this);
            }
        }
    }
    
    public override void Damaged(bool _isCritical , int _damage)
    {
        if (hp_Cur <= 0)
        {
            return;
        }        
        HpCur -= _damage;
        anim.SetBool("Damaged",true);        
        UIManager.uimanager.ALoadDamageEffect(_damage, this.gameObject, _isCritical);
    }

  
    public override string MiniDotSpriteName() { return "DotE"; }

    public override bool IsEnermy() { return true; }

   
    public override void KnockBack()
    {
        
    }




    #endregion
}
