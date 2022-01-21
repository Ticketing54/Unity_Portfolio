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
    
   
    public CharacterSkill skill { get; set; }
    public CharacterQuest quest { get; set; }    
    public Inventory Inven { get; set; }


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

    public delegate void ItemUiSetting(ItemListType _Type,int _SlotNum);
    



    #region MoveItemControl
    
    public void ItemMove(ItemListType _StartType,int _S_Num,ItemListType _EndType,int _E_Num, ItemUiSetting _Setting)
    {
        if (ItemMoveCheck(_StartType, _S_Num, _EndType, _E_Num) == false)       // 이동이 가능한지 체크
            return;
        Item start = ItemMoveStart(_StartType,_S_Num);
        if (start == null)
            return; 
        Item end = ItemMoveEnd(_EndType,_E_Num, start);
        if (end != null)
        {
            Item Temp = ItemMoveEnd(_StartType, _S_Num, end);
            if(Temp != null)
                Debug.LogError("아이템 장착 오류");
        }
        _Setting(_StartType, _S_Num);
        _Setting(_EndType, _E_Num);
    }
    public void ItemMove(ItemListType _Type, int _SlotNum, ItemUiSetting _Setting)
    {
        Item start = ItemMoveStart(_Type, _SlotNum);

        if (start == null)
            return;
        Item end;

        if (start.itemType != ItemType.EQUIPMENT)
        {
            end = ItemMoveEnd(_Type, _SlotNum, start);
            if (end != null)
                Debug.LogError("아이템 장착 오류");
            _Setting(_Type, _SlotNum);
            return;
        }
        else
        {
            switch (_Type)
            {
                case ItemListType.EQUIP:
                    int EmptySlotNum = Inven.EmptySlot();
                    if (EmptySlotNum < 0)         // 가득 찼을 때
                    {
                        end = ItemMoveEnd(_Type, _SlotNum, start);
                        _Setting(_Type, _SlotNum);
                        if (end != null)
                            Debug.LogError("아이템 장착 오류");
                        return;
                    }
                    end = ItemMoveEnd(ItemListType.INVEN, EmptySlotNum, start);
                    _Setting(_Type, _SlotNum);
                    _Setting(ItemListType.INVEN, EmptySlotNum);
                    if (end != null)
                        Debug.LogError("아이템 장착 오류");
                    return;
                case ItemListType.INVEN:
                    end = Equip.PushEquip((int)start.EquipType, start);
                    if (end != null)
                    {
                        Item Temp;
                        Temp = ItemMoveEnd(_Type, _SlotNum, end);
                        if (Temp != null)
                            Debug.LogError("아이템 장착 오류 ");
                    }
                    _Setting(_Type, _SlotNum);
                    _Setting(ItemListType.EQUIP, (int)start.EquipType);
                    return;
                default:
                    return;
            }
        }
    }
    bool ItemMoveCheck(ItemListType _StartType, int _S_Num, ItemListType _EndType, int _E_Num)
    {
        Item start = GetItem(_StartType, _S_Num);
        if (start == null)
            return false;
        Item end = GetItem(_EndType, _E_Num);
        if(end == null)                     // 목표점이 비어있을때
        {
            switch (start.itemType)
            {
                case ItemType.EQUIPMENT:
                    if (_EndType == ItemListType.EQUIP && start.EquipType == (EquipMentType)_E_Num)
                        return true;
                    if (_EndType == ItemListType.INVEN)
                        return true;
                    return false;
                case ItemType.USED:
                    if (_EndType == ItemListType.INVEN || _EndType == ItemListType.QUICK)
                        return true;
                    return false;
                case ItemType.ETC:
                    if (_EndType == ItemListType.INVEN)
                        return true;
                    return false;
                default:
                    return false;
            }
        }
        else
        {
            switch(start.itemType)          // 목표점에 아이템이 존재할때
            {
                case ItemType.EQUIPMENT:
                    if (_StartType != _EndType)     
                    {
                        if((_StartType == ItemListType.INVEN && _EndType == ItemListType.EQUIP)||(_StartType == ItemListType.EQUIP && _EndType == ItemListType.INVEN))          // 인벤토리에서 장비창으로
                        {
                            if (start.EquipType != end.EquipType)       // 부위가 다를경우
                                return false;
                            return true;                            
                        }                        
                        return false;                                                                  // 나머진 불가능
                    }
                    else
                    {
                        if (_StartType != ItemListType.INVEN)       // 인벤토리 내에서만
                            return false;
                        return true;
                    }                                               // 나머진 불가능
                case ItemType.USED:
                    if (_EndType == ItemListType.INVEN || _EndType == ItemListType.QUICK)
                    return true;
                return false;
                case ItemType.ETC:
                    if (_EndType == ItemListType.INVEN)
                    return true;
                return false;
                default:
                    return false;
            }

        }





    }   
    
    Item ItemMoveStart(ItemListType _Start, int _SlotNum)
    {
        switch (_Start)
        {
            case ItemListType.INVEN:
                return Inven.PopItem(_SlotNum);
            case ItemListType.EQUIP:
                return Equip.PopEquip(_SlotNum);
            case ItemListType.QUICK:
                return Quick.PopItem(_SlotNum);
            default:
                return null;                
        }
    }
    Item ItemMoveEnd(ItemListType _End, int SlotNum, Item _StartITem)
    {
        switch (_End)
        {
            case ItemListType.INVEN:
                return Inven.AddItem(SlotNum,_StartITem);
            case ItemListType.EQUIP:
                return Equip.PushEquip(SlotNum, _StartITem);
            case ItemListType.QUICK:
                return Quick.AddItem(SlotNum, _StartITem);
            default:
                Debug.LogError("잘못된 리스트 타입 입니다.");
                return null;
        }
    }
    public Item GetItem(ItemListType _Type, int _SlotNum)
    {
        switch (_Type)
        {
            case ItemListType.INVEN:
                return Inven.GetItem(_SlotNum);
            case ItemListType.EQUIP:
                return Equip.GetItem(_SlotNum);
            case ItemListType.QUICK:
                return Quick.GetItem(_SlotNum);
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
        TestQuest();
    }   
    


    public void TestQuest()
    {
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            
            quest.Add(0, new Quest(0, "테스트 퀘스트 입니다.", "DIALOG", "테스트 중입니다.", "MONSTER", 0, 0,"PLAYING"));
        }
        
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
