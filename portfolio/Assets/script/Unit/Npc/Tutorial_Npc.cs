using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Npc : Npc
{
    Quest tutorial_hp;
    Quest tutorial_Attack;

    public override void Interact()
    {
        base.Interact();
    }
    public override void EtcQuest(int _questIndex)
    {
        switch (_questIndex)
        {
            case 3:
                {
                    StartCoroutine(CoTutorial_Recovery());
                }
                break;
            default:
                Debug.Log("알수없는 인덱스입니다.");
                return;
        }
    }   
    IEnumerator CoTutorial_Recovery()
    {
        GameManager.gameManager.character.Damaged(50f);
        while (true)
        {
            if(GameManager.gameManager.character.stat.MAXHP == GameManager.gameManager.character.stat.HP)
            {
                GameManager.gameManager.character.quest.QuestComplete(3);
                break;
            }
            yield return null;
        }
    }
}
