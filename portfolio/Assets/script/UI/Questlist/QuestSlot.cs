using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestSlot : MonoBehaviour
{
    public Quest quest;
    public TextMeshProUGUI Text;

    public RectTransform tr;
    Rect rc;
    public Rect RC
    {
        get
        {
            rc.x = tr.position.x - tr.rect.width * 0.5f;
            rc.y = tr.position.y + tr.rect.height * 0.5f;
            return rc;
        }
    }


    void Start()
    {
        rc.x = tr.transform.position.x - tr.rect.width / 2;
        rc.y = tr.transform.position.y + tr.rect.height / 2;
        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;
    }







    public bool isInRect(Vector2 pos)
    {
        if (pos.x >= RC.x && pos.x <= RC.x + RC.width && pos.y >= RC.y - RC.height && pos.y <= RC.y)
        {

            return true;
        }
        return false;
    }

    public void QuestWrite()
    {
        Text.text = ""+quest.questName;
    }

    public void DoneQuest()
    {
        Text.color = Color.gray;
    }
    
}
