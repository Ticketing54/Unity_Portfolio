using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Npc : Npc
{
    Quest tutorial_hp;
    Quest tutorial_Attack;

    public override float HpMax { get => -1; set { } }
    public override float HpCur { get => -1; set { } }

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
        GameManager.gameManager.character.stat.Damaged(true,10);
        while (true)
        {
            if(GameManager.gameManager.character.stat.MaxHp == GameManager.gameManager.character.stat.Hp)
            {
                GameManager.gameManager.character.quest.UpdateQuest_Etc(3);
                break;
            }
            yield return null;
        }
    }

    public override bool IsEnermy() { return false; }

    
}
