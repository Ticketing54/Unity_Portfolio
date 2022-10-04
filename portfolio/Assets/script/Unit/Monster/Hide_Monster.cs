using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Monster : Nomal_Monster
{
    bool hideMonster;
    bool approachChracter = false;
    public override void Awake()
    {
        base.Awake();
        gameObject.tag = "Item";
        hideMonster = true;
    }
    public override void OnEnable()
    {
        StartCoroutine(CoHideMonster());
    }
    public override void OnDisable()
    {
        StopAllCoroutines();
    }
    public override void DropItem()
    {
        if(hp_Cur <= 0)
        {
            hideMonster = true;            
            StopAllCoroutines();
            base.DropItem();
            
        }
        else
        {
            if(gameObject.tag == "Item")
            {
                gameObject.tag = "Monster";
                GameManager.gameManager.character.RemoveInteract(this);
                UIManager.uimanager.ARemoveNearUnitUi(this);
                hideMonster = false;
                approachChracter = false;
                GameManager.gameManager.character.stat.Damaged(true, 10);
                GameManager.gameManager.character.Sturn(2f);
                anim.SetTrigger("Hide");
            }
            
        }
    }
    public override void Respawn()
    {
        base.Respawn();
        hideMonster = true;
    }
    protected IEnumerator CoHideMonster()
    {
        yield return null;

        
        while (true)
        {
            if (GameManager.gameManager.character != null)
            {

                if(hideMonster == true)
                {
                    if (this.Distance < 6f && approachChracter == false)
                    {
                        approachChracter = true;
                        GameManager.gameManager.character.AddNearInteract(this);
                    }

                    if (this.Distance >= 6f && approachChracter == true)
                    {
                        approachChracter = false;                        
                        GameManager.gameManager.character.RemoveInteract(this);
                    }
                }
                else
                {
                    if (this.Distance < 6f && approachChracter == false)
                    {
                        approachChracter = true;
                        UIManager.uimanager.AAddNearUnitOnUi(this);

                        if (gameObject.tag == "item")
                        {
                            GameManager.gameManager.character.AddNearInteract(this);
                        }
                    }

                    if (this.Distance >= 6f && approachChracter == true)
                    {
                        approachChracter = false;
                        UIManager.uimanager.ARemoveNearUnitUi(this);

                        if (gameObject.tag == "item")
                        {
                            GameManager.gameManager.character.RemoveInteract(this);
                        }

                    }
                }





                
            }
            yield return null;
        }
    }
}
