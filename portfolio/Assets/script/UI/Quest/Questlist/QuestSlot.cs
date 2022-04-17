using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSlot :Slot
{    
    [SerializeField]
    TextMeshProUGUI Text;
    public int QuestIndex { get; set; }
    public QuestSlot(int _QuestIndex)
    {
        QuestIndex = _QuestIndex;
    }

    public void QuestWrite(string _QuestName)
    {
        Text.text = _QuestName;
    }
    public void Clear()
    {
        QuestIndex = -1;
        Text.text = "";
        Text.color = Color.white;
    }

    public void DoneQuest()
    {
        Text.color = Color.gray;
    }
    
}
