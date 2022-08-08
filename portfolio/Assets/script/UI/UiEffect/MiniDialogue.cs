using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MiniDialogue : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    
    public void SetText (string _text)
    {
        text.text = _text;
    }


    public IEnumerator CoTextingDialog(string _text, Unit _target)
    {
        float timer = 0f;
        int currentText = 0;
        while (currentText != _text.Length)
        {
            yield return null;
            transform.position = Camera.main.WorldToScreenPoint(_target.transform.position + new Vector3(0f, 3f, 0f));


            timer += Time.deltaTime;

            if (timer >= 0.1f)
            {
                currentText++;
                timer = timer - 0.1f;

                string dialogText = _text.Substring(0, currentText);
                string emptyText = string.Empty;
                int count = _text.Length - currentText - 1;
                while (count >= 0)
                {
                    count--;
                    emptyText += "  ";
                }
                emptyText += '\r';
                SetText(dialogText + emptyText);
            }


        }

        yield return new WaitForSeconds(1f);

        
        //for (int i = 0; i < _text.Length; i++)
        //{
        //    string dialogText = _text.Substring(0, i);
        //    string emptyDialog = string.Empty;
        //    int count = _text.Length - i - 1;
        //    while (count != 0)
        //    {
        //        count--;
        //        emptyDialog += "  ";
        //    }
        //    emptyDialog += '\r';
        //    _dialog.SetText(dialogText + emptyDialog);



        //    yield return new WaitForSeconds(0.1f);
        //}

        //_dialog.SetText(_text);

        //yield return new WaitForSeconds(1f);
    }

}
