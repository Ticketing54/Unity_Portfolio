using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDialog_Npc : Npc
{

    List<List<string>> dialog;
    
    private void Start()
    {
        dialog = new List<List<string>>();
        GetDialogData();
        action = StartCoroutine(CoSpeechBubble());
    }
    void GetDialogData()
    {
        List<string> dialogResData = ResourceManager.resource.GetDialogue(DIALOGUE);
        List<List<string>> dialogData = new List<List<string>>();
        for (int i = 0; i < dialogResData.Count; i++)
        {
            List<string> subdialog = new List<string>();
            string[] data = dialogResData[i].Split(',');
            for (int j = 0; j < data.Length; j++)
            {
                subdialog.Add(data[j]);
            }
            dialogData.Add(subdialog);
        }
        dialog = dialogData;
    }
    public override void Interact()
    {
        if(UsingDialog == true)
        {
            return;
        }

        int maxdialog = dialog.Count;
        int currentDialogNum = Random.Range(0, maxdialog );
        List<string> currentDialogData = dialog[currentDialogNum];

        UIManager.uimanager.uiEffectManager.TextingMiniDialog(this ,currentDialogData[1]);

    }

    IEnumerator CoSpeechBubble()
    {
        int currentDialogNum = Random.Range(1, dialog.Count);
        List<string> currentDialogData = dialog[currentDialogNum];
        EffectManager.effectManager.SpeechBubble(this, currentDialogData[1]);
        yield return new WaitForSeconds(5f);
        action = StartCoroutine(CoSpeechBubble());
    }
}
