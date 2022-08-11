using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]
    TextMeshPro text;
    [SerializeField]
    GameObject bubble;

    public delegate void ResetBubble(Unit _unit ,SpeechBubble _speechBubble);

    private void OnEnable()
    {
        text.text = "";
    }
    public void SetSpeechBubble(GameObject _bubble, TextMeshPro _text)
    {
        text = _text;        
        bubble = _bubble;
        bubble.gameObject.transform.SetParent(_text.transform);
    }
    public void TextingSpeechBubble(Unit _unit,string _text,ResetBubble _resetBubble)
    {   
        bubble.transform.localScale = new Vector3(text.rectTransform.rect.width, text.rectTransform.rect.height+ 0.3f, 0);
        bubble.transform.localPosition = new Vector3(0,0,-0.2f);

        StartCoroutine(CoPositioningBubble(_unit));
        StartCoroutine(CoTextingSpeechBubble(_unit,_text, _resetBubble));
    }

    IEnumerator CoTextingSpeechBubble(Unit _unit,string _text, ResetBubble _resetBubble)
    {
        int checkDone = 0;


        while(checkDone < _text.Length)
        {
            yield return new WaitForSeconds(0.1f);
            text.text = _text.Substring(0, checkDone);
            checkDone++;
        }

        yield return new WaitForSeconds(1f);

        
        _resetBubble(_unit,this);
    }
    IEnumerator CoPositioningBubble(Unit _unit)
    {
        while (true)
        {
            yield return null;

            Vector3 dir = (Camera.main.transform.position - transform.position).normalized;            
            transform.position = new Vector3(_unit.transform.position.x,_unit.transform.position.y+_unit.Nick_YPos+0.5f,_unit.transform.position.z);
            transform.rotation = Quaternion.LookRotation(-dir);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();        
    }

}
