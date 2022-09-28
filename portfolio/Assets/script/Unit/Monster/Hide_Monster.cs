using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Monster : Nomal_Monster
{
    public override void Awake()
    {
        base.Awake();
        gameObject.tag = "Item";
    }

    public override void DropItem()
    {
        if(hp_Cur <= 0)
        {
            base.DropItem();
        }
        else
        {
            if(gameObject.tag == "Item")
            {
                gameObject.tag = "Monster";
                GameManager.gameManager.character.stat.Damaged(true, 10);
                //½ºÅÏ
                anim.SetTrigger("Hide");
            }
            
        }
    }
    public override void Respawn()
    {
        base.Respawn();
        gameObject.tag = "Item";
    }
}
