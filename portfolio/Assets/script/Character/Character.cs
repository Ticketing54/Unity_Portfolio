using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    public static Character Player;
    public Status Stat = null;
    public Equipment Equip = null;
    public CharacterSkill Skill = null;
    public CharacterQuest Quest = null;
    public Inventory Inven = null;
    public QuickSlot Quick = null;

    public List<SkinnedMeshRenderer> Weapon = new List<SkinnedMeshRenderer>();
    public SkinnedMeshRenderer Character_bounds;
    public List<Monster> MobList = new List<Monster>();
    public List<Npc> npcList = new List<Npc>();
    
    
    
    

    public GameObject Target;

    
    public NavMeshAgent nav;
    public Animator anim;
    
    
    // 상호작용
    public bool Interaction_B = false;
    public bool Interaction_T = false;   
    public bool Interaction_L = false;

    //클릭
    public bool ClickObj = false;

    //상태
    public bool isMove = false;
    public bool DontMove = false;
    public bool isCombo = false;          
    public bool isrecovery_Hp = false;
    public bool isrecovery_Mp = false;


    public bool OpenLootingbox = false; // 루팅박스 
    public Vector3 StartPos;
    public delegate Item SwapStart(int _Start);
    public delegate void SwapEnd(int _End, Item item);



    #region MoveItemControl
    
    public bool ItemMove(ItemListType _StartType,int _S_Num,ItemListType _EndType,int _E_Num)
    {
        Item Start = ItemMoveStart(_StartType)(_S_Num);
        if (Start == null)
            return false;
        if (CheckMoveItem(Start, _EndType, _E_Num))
        {
            ItemMoveEnd(_StartType)(_S_Num, Start);             // 클릭한 아이템을 보낼수 있는지 체크
            return false;
        }
        else
        {
            Item End = ItemMoveStart(_EndType)(_E_Num);
            if (End == null)                                    // 빈곳으로 보냈을때
            {
                ItemMoveEnd(_EndType)(_E_Num, Start);
            }
            else if (Start.Index == End.Index)                  
            {
                if (Start.itemType == Item.ItemType.Equipment)  // 같은 아이템일때(장비아이템일 경우 교환)
                {
                    ItemMoveEnd(_StartType)(_S_Num, End);
                    ItemMoveEnd(_EndType)(_E_Num, Start);
                }
                else                                            // 같은 아이템일때(합산)
                {
                    Start.ItemCount += End.ItemCount;
                    ItemMoveEnd(_EndType)(_E_Num, End);
                }
            }
            else                                                // 다른 아이템일때 (교환)
            {
                ItemMoveEnd(_StartType)(_S_Num, End);
                ItemMoveEnd(_EndType)(_E_Num, Start);
            }

            return true;
        }
    }
    
    bool CheckMoveItem(Item _item, ItemListType _EndType,int _E_Num)
    {
        switch (_EndType)
        {
            case ItemListType.INVEN:
                return false;
            case ItemListType.EQUIP:
                if (_item.itemType != Item.ItemType.Equipment)
                    return true;
                else
                {
                    if ((int)_item.EquipType != _E_Num)
                        return true;
                    else
                        return false;
                }                
            case ItemListType.QUICK:
                return _item.itemType != Item.ItemType.Used;
            default:
                return true;
        }
    }
    
    SwapStart ItemMoveStart(ItemListType _Start)
    {
        switch (_Start)
        {
            case ItemListType.INVEN:
                return Inven.StartItemMove;                
            case ItemListType.EQUIP:
                return Equip.StartItemMove;                
            case ItemListType.QUICK:
                return Quick.StartItemMove;                
            default:
                return null;                
        }
    }
    SwapEnd ItemMoveEnd(ItemListType _End)
    {
        switch (_End)
        {
            case ItemListType.INVEN:
                return Inven.AddItem;                
            case ItemListType.EQUIP:
                return Equip.AddItem;                
            case ItemListType.QUICK:
                return Quick.AddItem;                
            default:
                return null;
        }
    }
    #endregion



    private void Awake()
    {        
        if(Player == null)
        {
            Player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            Player = this;
            DontDestroyOnLoad(gameObject);
        }
        GameObject [] obj = GameObject.FindGameObjectsWithTag("Weapon");
        for(int i = 0; i < obj.Length; i++)
        {
            Weapon.Add(obj[i].GetComponent<SkinnedMeshRenderer>());
        }
        Character_bounds = transform.Find("Render").GetComponent<SkinnedMeshRenderer>();

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.updateRotation = false;        
    }


    //public void QuestUpdate(int _index,int _completenum)
    //{
    //    foreach(Quest one in myQuest)
    //    {
    //        if(one.Index == _index)
    //        {
    //            one.QuestComplete = _completenum;
    //        }
    //    }
    //}
    private void Start()
    {
        if (nav != null)
            nav.Warp(StartPos);        
    }

    void Update()
    {        
        if (UIManager.uimanager.FadeUi.alpha == 0)
            return;
        Click();             
        Interation();
        Move();        
    }   
    
    public void SetDestination(Vector3 dest)
    {
        if (DontMove == true)
            return;

        nav.SetDestination(dest);
        isMove = true;

    }
    void Click()
    {
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    if (hitinfo.collider.gameObject.layer == 9)
                    {
                        SetDestination(hitinfo.point);
                        ObjectPoolManager.objManager.ClickMove("Click", hitinfo.point);                        
                        Target = null;
                        return;
                    }
                    else if (hitinfo.collider.gameObject.tag == "Item" ||
                        hitinfo.collider.gameObject.tag == "Npc")
                    {
                        Target = hitinfo.collider.gameObject;
                        ObjectPoolManager.objManager.ClickEffect("Friend", Target.transform);
                        ClickObj = true;
                    }
                    else if (hitinfo.collider.gameObject.tag == "Monster")
                    {
                        Target = hitinfo.collider.gameObject;
                        ObjectPoolManager.objManager.ClickEffect("Enermy", Target.transform);
                        ClickObj = true;
                    }
                    else
                    {
                        Target = null;
                    }
                }

            }
        }
        else
        {
            ClickObj = false;

        }
    }
    void Move()
    {        
        if (isMove )
        {
            if (nav.velocity.magnitude == 0f)
            {
                isMove = false;                
                return;
            }
            Vector3 dir = new Vector3 (nav.steeringTarget.x,transform.position.y,nav.steeringTarget.z) - transform.position;
            dir.y = 0;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir.normalized, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);           
        }
    }
    void LevelUp()
    {
        Stat.LevelUp();
        GameObject LvEffect = ObjectPoolManager.objManager.EffectPooling("LevUp");
        TextMeshProUGUI LvString = ObjectPoolManager.objManager.stringEffect();
        StartCoroutine(LevUpMove(LvEffect, LvString));
        StartCoroutine(WaitForItLev(LvEffect, LvString.gameObject));
    }
    IEnumerator LevUpMove(GameObject obj, TextMeshProUGUI _string)  //레벨업 
    {

        Vector3 Pos = transform.position;
        Pos.y += 1f;
        _string.fontSize = 33;
        while (obj.activeSelf == true)
        {
                        
            obj.transform.position = gameObject.transform.position;
            _string.transform.position = Camera.main.WorldToScreenPoint(new Vector3(Pos.x,Pos.y+=Time.deltaTime*0.6f,Pos.z));
            _string.fontSize += Time.deltaTime* 20f;
            _string.alpha -= Time.deltaTime *0.6f;
            yield return null;

        }

        yield return null;
        
    }
    IEnumerator WaitForItLev(GameObject obj,GameObject _string) // 레벨업
    {
        yield return new WaitForSeconds(2.5f);        
        obj.SetActive(false);
        _string.SetActive(false);
    }

    void Interation()
    {
        if (Target != null)
        {
            float dis = Vector3.Distance(transform.position, Target.transform.position);

            if (Target.tag == "Monster" && dis < Stat.ATK_RANGE)
            {
                Interaction_B = true;
                SetDestination(transform.position);
                Vector3 dir = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z) - transform.position;
                transform.forward = dir;
                return;
            }
            else if (Target.tag == "Npc" && dis < 1.5f)
            {
                Interaction_T = true;
                SetDestination(transform.position);
                Vector3 dir = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z) - transform.position;
                transform.forward = dir;
                return;
            }
            else if (Target.tag == "Item" && dis < 1.5f)
            {
                Interaction_L = true;
                SetDestination(transform.position);
                Vector3 dir = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z) - transform.position;
                transform.forward = dir;
                return;
            }
            else
            {
                Interaction_B = false;
                Interaction_T = false;
                Interaction_L = false;
                if (Target.activeSelf == true)
                {
                    SetDestination(Target.transform.position);
                }

            }

        }
        else
        {
            Interaction_B = false;
            Interaction_T = false;
            Interaction_L = false;
        }

    }

    public void Attack_Combo()
    {
        isCombo = true;
    }
    public void Attack_Reset()
    {
        isCombo = false;        
    }

   
    public Monster GetMonster(GameObject _Target)
    {
        if(Target.tag == "Monster")
        {
            foreach (Monster one in MobList)
            {
                if (one.gameObject.Equals(_Target))
                    return one;

            }
        }
        return null;
    }
  
    public void EffectEvent(string _name)
    {
        GameObject obj = ObjectPoolManager.objManager.EffectPooling(_name);        
        obj.name = _name;
        Vector3 Priset;
        if (ObjectPoolManager.objManager.EffectPos_Dic.TryGetValue(obj,out Priset))
        {
            obj.transform.position = transform.position + Priset;            
            obj.transform.rotation = transform.rotation;           
            
        }        
        StartCoroutine(WaitForIt(obj));
        
        
    }
    IEnumerator WaitForIt(GameObject obj)
    {
        yield return new WaitForSeconds(2f);        
        obj.SetActive(false);
        
    }
    public void IntersectsMob(int _num)
    {
        for (int j = 0; j < MobList.Count; j++)
        {
            if (Player.Weapon[_num].bounds.Intersects(MobList[j].MobBounds.bounds))
            {
                MobList[j].isDamage = true;
                float Dmg = Stat.ATK;
                MobList[j].Hp -= Stat.ATK;
                ObjectPoolManager.objManager.LoadDamage(MobList[j].gameObject, Dmg, Color.yellow, 1);
                
            }
        }
    }
    public void  RangeMob()
    {
        for (int i = 0; i < MobList.Count; i++)
        {
            if (MobList[i].DiSTANCE <=2f)
            {
                MobList[i].isDamage = true;
                float Dmg = Stat.ATK * 3f;
                MobList[i].Hp -= Dmg;                
                ObjectPoolManager.objManager.LoadDamage(MobList[i].gameObject, Dmg, Color.yellow, 1);
            }
        }
    }
    public void ListReset()
    {
        MobList.RemoveAll(x => true);
        npcList.RemoveAll(x => true);
    }
    public Monster FindNearEnermy()
    {
        Monster mob = MobList[0];
        for(int i =0; i < MobList.Count; i++)
        {
            if (mob.DiSTANCE > MobList[i].DiSTANCE)
                mob = MobList[i];
        }

        return mob;
    }    
    public void burnStateOn()  // 화상
    {
        if(Stat.isburn == false)
        {
            StartCoroutine(BurnDamage());
        }
        

    }
    public void icestateOn()     // 빙결
    {
        if(Stat.isIce == false)
        {
            StartCoroutine(IceState());
        }
    }
    IEnumerator BurnDamage()
    {
        GameObject Effect;
        Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Burn");
        float timer = 0;
        float holdTime = 5;
        Stat.isburn = true;
        while (true)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                timer -= 1;
                holdTime -= 1;
                Stat.HP -= 5;
                ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, 10f, Color.red, 1);
            }
            Effect.transform.position = Character.Player.transform.position;
            


            if(holdTime <= 0)
            {
                Effect.SetActive(false);
                Effect = null;
                Stat.isburn = false;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator IceState()
    {
        GameObject Effect;
        Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Ice");
        float timer = 0;
        Stat.isIce = true;        
        Character.Player.nav.speed -= 4f;
        Character.Player.anim.speed = 0.5f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >=5)
            {
                Effect.SetActive(false);
                Effect = null;
                Stat.isIce = false;
                Character.Player.nav.speed += 4f;
                Character.Player.anim.speed = 1f;

                yield break;
            }
            Effect.transform.position = Character.Player.transform.position;



            
            yield return null;
        }
    }
    public void soundPlayer(string _Name) 
    {
        SoundManager.soundmanager.soundsPlay(_Name, Character.Player.gameObject);    
    }


}
