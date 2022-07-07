using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniQuestSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI quest_Name;
    [SerializeField]
    TextMeshProUGUI quest_Explain;
    [SerializeField]
    TextMeshProUGUI quest_Prograss;

    
  



    public void TextingQuestSlot(Quest _quest)
    {
        quest_Name.color = Color.white;
        quest_Explain.color = Color.white;
        quest_Prograss.color = Color.white;
        quest_Name.text = _quest.questName;
        quest_Explain.text = _quest.Explain;


        if (_quest.State == QUESTSTATE.COMPLETE)
        {
            finishQuest();
            return;
        }
        else
        {
            quest_Prograss.text = "( " + _quest.Goal_Current + " / " + _quest.Goal_Need + " )";
        }
        
        

    }

    public void UpdatePrograss(Quest _quest)   
    {
        if(_quest.State == QUESTSTATE.COMPLETE)
        {
            finishQuest();
        }
        else
        {
            quest_Prograss.text = "( " + _quest.Goal_Current + " / " + _quest.Goal_Need + " )";
        }
        
    }

    public void finishQuest()   //퀘스트 완료
    {
        quest_Name.color = Color.green;
        quest_Explain.color = Color.green;
        quest_Prograss.color = Color.green;
        quest_Prograss.text = "완료";
    }







}
