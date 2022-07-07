using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AddQuestText_Ui : MonoBehaviour
{
    [SerializeField]
    CanvasGroup addQuestText;
    [SerializeField]
    RectTransform rectTrans;
    [SerializeField]
    TextMeshProUGUI text;
    Vector2 StartPos = new Vector2(0, -330);    
    float timer = 0f;
    bool isActive = false;

    #region QuestEffect



    void QuestEffectOn(int _questIndex, QUESTSTATE _state)
    {
        switch (_state)
        {
            case QUESTSTATE.PLAYING:
                break;
            case QUESTSTATE.COMPLETE:
                {
                    CompleteQuestEffect(_questIndex);
                }
                break;
            case QUESTSTATE.DONE:
                {
                    DoneQuestEffect(_questIndex);
                }
                break;
        }
    }
    void AddQuestEffect(int _questIndex)
    {
        gameObject.SetActive(true);
        Quest quest = new Quest(_questIndex,QUESTSTATE.NONE);
        text.text = "[" + quest.questName+"]"+" ����Ʈ�� ���� �ϼ̽��ϴ�.";
    }
    void CompleteQuestEffect(int _questIndex)
    {
        gameObject.SetActive(true);
        Quest quest = new Quest(_questIndex, QUESTSTATE.NONE);
        text.text = "[" + quest.questName + "]" + " ����Ʈ�Ϸ�! Npc�� ã�ư�����.";
    }
    void DoneQuestEffect(int _questIndex)
    {
        gameObject.SetActive(true);
        Quest quest = new Quest(_questIndex, QUESTSTATE.NONE);
        text.text = "[" + quest.questName + "]" + " ����Ʈ�� �Ϸ��Ͽ����ϴ�. ";
    }
    #endregion
    private void Start()
    {
        UIManager.uimanager.AAddQuestUi      += AddQuestEffect;
        UIManager.uimanager.AQuestUpdateUi += QuestEffectOn;        

        gameObject.SetActive(false);        
    }
    private void OnEnable()
    {
        rectTrans.anchoredPosition = StartPos;
        timer = 0f;
        isActive = false;
    }
   
    private void Update()
    {
        if (isActive == true)
        {
            addQuestText.alpha -= Time.deltaTime * 3;
            rectTrans.anchoredPosition += Vector2.down*0.5f;
            if (addQuestText.alpha <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (rectTrans.anchoredPosition.y > -300&& addQuestText.alpha >= 1f)
            {
                timer += Time.deltaTime;

                if (timer > 1f)
                {
                    isActive = true;
                }
            }
            else
            {
                if(rectTrans.anchoredPosition.y <= -300)
                {
                    rectTrans.anchoredPosition += Vector2.up;
                }
                addQuestText.alpha += Time.deltaTime * 2f;                
            }
        }
    }


}
