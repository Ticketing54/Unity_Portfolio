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
    private void OnEnable()
    {
        rectTrans.anchoredPosition = StartPos;
        timer = 0f;
        isActive = false;
    }
    public void SetQuestEffect(QUESTSTATE _state)
    {
        switch (_state)
        {
            case QUESTSTATE.PLAYING:
                {
                    text.text = "����Ʈ�� ���� �ϼ̽��ϴ�.";
                    break;
                }
            case QUESTSTATE.COMPLETE:
                {
                    text.text = "����Ʈ �Ϸ� Npc�� ã�ư�����!";
                    break;
                }
            case QUESTSTATE.DONE:
                {
                    text.text = "����Ʈ�� �Ϸ� �ϼ̽��ϴ�.";
                    break;
                }
        }
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
