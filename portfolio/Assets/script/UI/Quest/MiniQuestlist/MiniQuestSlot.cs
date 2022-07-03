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
        
        if(_quest.Goal_Need == 0)
        {
            quest_Name.text = _quest.Name;
            quest_Explain.text = _quest.Explain;
            quest_Prograss.text = "";
            return;
        }
        quest_Name.text = _quest.Name;
        quest_Explain.text = _quest.Explain;
        quest_Prograss.text = "( " + _quest.Goal_Current + " / " + _quest.Goal_Need + " )";

    }

    public void UpdatePrograss(Quest _quest)   
    {
        quest_Prograss.text = "( " + _quest.Goal_Need + " / " + _quest.Goal_Current + " )";
    }

    public void finishQuest()   //퀘스트 완료
    {
        quest_Name.color = Color.gray;
        quest_Explain.color = Color.gray;
        quest_Prograss.color = Color.gray;
        quest_Prograss.text = "완료";
    }







}
