using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Monster : Monster
{
    public override void Damaged(float _dmg)
    {
        if (Character.Player.quest.isQuestMonster(index))
        {
            Character.Player.quest.UpdatePlayingQuest(2,1);
        }
    }
}
