using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RewardMessage : MonoBehaviour
{
    Action<RewardMessage> removeMessage;
    
    [SerializeField]
    CanvasGroup alpha;
    [SerializeField]
    TextMeshProUGUI textMessage;

    private void OnDisable()
    {
        alpha.alpha = 0;
        textMessage.text = "";
    }
    public void SetMessage(Action<RewardMessage> _remove, string _message)
    {
        removeMessage = _remove;
        textMessage.text = _message;
        StartCoroutine(FadeInMessage());
    }
    
    

   IEnumerator FadeInMessage()
    {   
        while(alpha.alpha != 1)
        {
            alpha.alpha += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeOutMessage());
    }

    IEnumerator FadeOutMessage()
    {
        while (alpha.alpha != 0)
        {
            alpha.alpha -= Time.deltaTime;
            yield return null;
        }
        removeMessage(this);
    }
}
