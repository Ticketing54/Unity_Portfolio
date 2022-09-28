using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NotMonster : Monster
{   
    protected List<Item> dropItemList;
    
    public override float HpMax
    {
        get
        {
            return hp_Max;
        }
        set
        {
          
        }
    }
    public override float HpCur
    {
        get
        {
            return hp_Cur;
        }
        set
        {
           
        }
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

        stateTime = 0f;                
    }

    public override void DropItem()
    {
        
    }

    public override void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    #region State

    public override void Respawn()
    {
        
    }

    public override void Sturn(float _time)
    {
        
    }

    void Die()
    {
        
    }
    
    public override void Damaged(bool _isCritical , int _damage)
    {                  
        anim.SetBool("Damaged",true);        
        UIManager.uimanager.ALoadDamageEffect(_damage, this.gameObject, _isCritical);
    }

  
    public override string MiniDotSpriteName() { return "DotE"; }

    public override bool IsEnermy() { return true; }

   
    public override void KnockBack()
    {
        
    }




    #endregion
}
