using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Monster : Monster
{
    public override void Damaged(float _dmg)
    {
        if (GameManager.gameManager.character.quest.isQuestMonster(index))
        {
            GameManager.gameManager.character.quest.UpdatePlayingQuest(2,1);
        }
    }
}
