using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Tutorial_Monster : Nomal_Monster
{
    public override void Awake()
    {
        anim = GetComponent<Animator>();
        hitBox = GetComponent<Collider>();        
    }

    public override void SetMonster(int _index, Vector3 _startPos)
    {
        index = _index;


        List<string> tableInfo = ResourceManager.resource.GetTable_Index("MonsterTable", _index);


        unitName = tableInfo[2];


        float t_NickPos;
        if (float.TryParse(tableInfo[3], out t_NickPos))
        {
            nick_YPos = t_NickPos;
        }
        else
        {
            Debug.LogError("NickPos 변환 오류");
        }


        if (string.IsNullOrEmpty(tableInfo[4]))
        {
            Debug.LogError("소리파일이름 없음");
        }
        else
        {
            sound = tableInfo[4];
        }


        int t_Lev;
        if (int.TryParse(tableInfo[5], out t_Lev))
        {
            lev = t_Lev;
        }
        else
        {
            Debug.LogError("Lev 변환 오류");
        }


        float t_HpMax;
        if (float.TryParse(tableInfo[6], out t_HpMax))
        {
            hp_Max = t_HpMax;
        }
        else
        {
            Debug.LogError("HpMax 변환 오류");
        }

        hp_Cur = hp_Max;

        float t_Atk;
        if (float.TryParse(tableInfo[7], out t_Atk))
        {
            atk = t_Atk;
        }
        else
        {
            Debug.LogError("Atk 변환 오류");
        }

        float t_Range;
        if (float.TryParse(tableInfo[8], out t_Range))
        {
            range = t_Range;
        }
        else
        {
            Debug.LogError("Range 변환 오류");
        }

        //
        if (string.IsNullOrEmpty(tableInfo[9]))
        {
            Debug.Log("아이템 없음");
        }
        else
        {
            string itemInfo = tableInfo[9];
        }
        //


        if (string.IsNullOrEmpty(tableInfo[10]))
        {
            gold = 0;
        }
        else
        {
            int t_gold;
            if (int.TryParse(tableInfo[10], out t_gold))
            {
                gold = t_gold;
            }
            else
            {
                Debug.LogError("Gold 변환 오류");
            }

        }

        if (string.IsNullOrEmpty(tableInfo[11]))
        {
            gold = 0;
        }
        else
        {
            int t_Exp;
            if (int.TryParse(tableInfo[11], out t_Exp))
            {
                exp = t_Exp;
            }
            else
            {
                Debug.LogError("Exp 변환 오류");
            }
        }

        startPos = _startPos;
    }
    public override void OnEnable()
    {
        StartCoroutine(CoApproachChracter());
    }
    
    public override void Damaged(bool _type,int _dmg)
    {
        if (GameManager.gameManager.character.quest.isQuestMonster(index))
        {
            GameManager.gameManager.character.quest.UpdateQuest_Etc(2);
        }

        base.Damaged(_type, _dmg);
    }

    public override void StatusEffect(STATUSEFFECT _state, float _duration)
    {
        return;
    }
}
