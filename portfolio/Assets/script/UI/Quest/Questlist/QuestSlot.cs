using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSlot :Slot
{    
    [SerializeField]
    TextMeshProUGUI Text;    
    
    public void QuestWrite(string _QuestName)
    {
        Text.text = _QuestName;
    }
    public override void Clear()
    {   
        Text.text = "";
        Text.color = Color.white;
    }

    public void DoneQuest()
    {
        Text.color = Color.gray;
    }
    
}
