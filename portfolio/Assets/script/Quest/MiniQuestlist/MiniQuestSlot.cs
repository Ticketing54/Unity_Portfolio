using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniQuestSlot : MonoBehaviour
{
    public Quest quest;
    public TextMeshProUGUI quest_Name;
    public TextMeshProUGUI quest_Explain;
    public TextMeshProUGUI quest_Prograss;

    
  



    public void TextingQuestSlot()
    {
        if(quest.goal_Need == 0)
        {
            quest_Name.text = quest.Name;
            quest_Explain.text = quest.Explain;
            quest_Prograss.text = "";
            return;
        }
        quest_Name.text = quest.Name;
        quest_Explain.text = quest.Explain;
        quest_Prograss.text = "( " + quest.goal_Current + " / " + quest.goal_Need + " )";

    }

    public void UpdatePrograss()   
    {
        quest_Prograss.text = "( " + quest.goal_Need + " / " + quest.goal_Current + " )";
    }

    public void finishQuest()   //퀘스트 완료
    {
        quest_Name.color = Color.gray;
        quest_Explain.color = Color.gray;
        quest_Prograss.color = Color.gray;
        quest_Prograss.text = "완료";
    }







}
