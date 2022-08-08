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

    public SpeechBubble(GameObject _bubble, TextMeshPro _text)
    {
        text = _text;
        bubble = _bubble;
        bubble.gameObject.transform.SetParent(_text.transform);
    }

    IEnumerator CoTextingSpeechBubble(Unit _unit, string _text)
    {
        for (int i = 0; i < _text.Length; i++)
        {
            text.text = _text.Substring(0, i);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }


    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
