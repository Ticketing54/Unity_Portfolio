using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MiniDialogue : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    

    public IEnumerator CoTextingDialog(string _dialog)
    {   
        for (int i = 0; i < _dialog.Length; i++)
        {            
            string dialogText  = _dialog.Substring(0, i);
            string emptyDialog = string.Empty;
            int count = _dialog.Length - i-1;
            while(count!= 0)
            {
                count--;
                emptyDialog += "  ";
            }
            emptyDialog += '\r';
            text.text = dialogText+emptyDialog;
            


            yield return new WaitForSeconds(0.1f);
        }

        text.text = _dialog;

        yield return new WaitForSeconds(1f);       
    }
}
