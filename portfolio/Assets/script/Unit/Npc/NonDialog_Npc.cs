using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDialog_Npc : Npc
{

    List<List<string>> dialog;

    public override float HpMax { get => -1; set { } }
    public override float HpCur { get => -1; set { } }

    private void Start()
    {
        dialog = new List<List<string>>();
        GetDialogData();
        action = StartCoroutine(CoSpeechBubble());
    }

    public override void OnDisable()
    {
        base.OnDisable();
        EffectManager.effectManager.RemoveSpeechBubble(this);
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
        

    }

    IEnumerator CoSpeechBubble()
    {
        int currentDialogNum = Random.Range(1, dialog.Count);
        List<string> currentDialogData = dialog[currentDialogNum];
        EffectManager.effectManager.SpeechBubble(this, currentDialogData[1]);
        yield return new WaitForSeconds(5f);
        action = StartCoroutine(CoSpeechBubble());
    }
    void CheckQuest()
    {
        if (quests.Count <= 0)
        {
            return;
        }
        Quest quest;
        for (int i = 0; i < quests.Count; i++)
        {
            quest = GameManager.gameManager.character.quest.GetQuest(quests[i]);

            if (quest == null)
            {
                Quest newQuest = new Quest(quests[i], QUESTSTATE.NONE);
                transform.position = newQuest.startPos;
                transform.rotation = Quaternion.Euler(newQuest.startDir);
                return;
            }
            else
            {
                if (quest.State == QUESTSTATE.COMPLETE)
                {
                    transform.position = quest.completePos;
                    transform.rotation = Quaternion.Euler(quest.completeDir);
                    return;
                }
            }
        }
    }

    public override bool IsEnermy()
    {
        return false;
    }

  
}
