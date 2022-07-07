using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpc : Npc
{   

    public override void SetQuestMark()
    {
        base.SetQuestMark();
        CheckQuest();
    }

    void CheckQuest()
    {
        if (quests.Count <= 0)
        {
            return;
        }
        Quest quest;
        for (int i = 0; i < quests.Count; i++)
        {
            quest = GameManager.gameManager.character.quest.GetQuest(quests[i]);

            if(quest == null)
            {
                transform.position = quest.startPos;
                transform.rotation = Quaternion.Euler(quest.startDir);
                return;
            }
            else
            {
                if(quest.State == QUESTSTATE.COMPLETE)
                {
                    transform.position = quest.completePos;
                    transform.rotation = Quaternion.Euler(quest.completeDir);
                    return;
                }                
            }
        }
    }




}
