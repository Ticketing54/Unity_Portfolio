using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : Unit
{
    protected NavMeshAgent nav;
    protected Animator anim;
    protected Coroutine action;       

    protected int index = -1;
    protected float hp_Max;
    protected float hp_Cur;
    protected float atk;
    protected float range;
    protected int lev;
    protected int gold;
    protected int exp;
    protected List<List<int>> itemDropInfo;
    public bool dropItem = false;
    public float Range { get => range; set => range = value; }
    public float stateTime;
    #region State
    public int Index { get => index; }
    
    public abstract void Respawn();    
    public abstract void DropItem();
    public virtual bool IsCri()
    {
        float cri = Random.Range(0, 101f);
        if (cri >= 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
    public virtual void OnEnable()
    {
        StartCoroutine(CoApproachChracter());
    }
    
    public virtual void OnDisable()
    {
        StopAllCoroutines();
        UIManager.uimanager.ARemoveNearUnitUi(this);        
    }
    public override void interact()
    {
        DropItem();
    }

    #endregion
    public virtual void UiOff()
    {
        UIManager.uimanager.ARemoveNearUnitUi(this);

    }
    public virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.stoppingDistance = range;
    }
    public bool IsCri(float _criPercentage)
    {
        float cri = Random.Range(0, 101);
        if(cri < _criPercentage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int AttackDmg(bool _isCri)
    {
        if(_isCri == true)
        {
            float finallyDamage = atk * 2;
            return (int)Random.Range(finallyDamage * 0.8f, finallyDamage * 1.2f);
        }
        else
        {
            float finallyDamage = atk;
            return (int)Random.Range(finallyDamage * 0.8f, finallyDamage * 1.2f);
        }

    }

    public abstract void Damaged(bool _cri, int _dmg);
    public abstract void Sturn(float _time);
    public abstract void KnockBack();

    public virtual void SetMonster(int _index, Vector3 _startPos)
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


        if (string.IsNullOrEmpty(tableInfo[9]))
        {
            Debug.Log("아이템 없음");
        }
        else
        {
            string[] infoArr = tableInfo[9].Split('/');
            List<List<int>> t_dropList = new List<List<int>>();
            for (int i = 0; i < infoArr.Length; i++)
            {
                string[] iteminfo = infoArr[i].Split('#');

                List<int> t_itemList = new List<int>();
                t_itemList.Add(int.Parse(iteminfo[0]));
                t_itemList.Add(int.Parse(iteminfo[1]));
                t_itemList.Add(int.Parse(iteminfo[2]));
                t_dropList.Add(t_itemList);
            }
            itemDropInfo = t_dropList;
        }



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
        stateTime = 0f;
        startPos = _startPos;
        nav.Warp(_startPos);
    }

    protected IEnumerator CoApproachChracter()
    {
        yield return null;

        bool approachChracter = false;
        while (true)
        {
            if (GameManager.gameManager.character != null)
            {
                if (this.Distance < 6f && approachChracter == false)
                {
                    approachChracter = true;
                    UIManager.uimanager.AAddNearUnitOnUi(this);
                    
                    if(gameObject.tag == "item")
                    {
                        GameManager.gameManager.character.AddNearInteract(this);
                    }
                }

                if (this.Distance >= 6f && approachChracter == true)
                {
                    approachChracter = false;
                    UIManager.uimanager.ARemoveNearUnitUi(this);

                    if (gameObject.tag == "item")
                    {
                        GameManager.gameManager.character.RemoveInteract(this);
                    }
                    
                }
            }
            yield return null;
        }
    }
}


