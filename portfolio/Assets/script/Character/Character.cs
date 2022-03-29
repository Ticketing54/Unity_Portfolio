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
    public Character(){}

    private void Awake()
    {
        if (Player == null)
        {
            Player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(GameManager.gameManager.character != this)
            {
                Destroy(gameObject);
            }            
        }
        SKILL = new CharacterSkill();
        QUEST = new CharacterQuest();
        INVEN = new Inventory();
        QUICKSLOT = new QuickSlot();
        QUICKQUEST = new Quest[4];        
        //Stat = new Status();
        //Equip = new Equipment();
    }


    NavMeshAgent nav;

    public Status STAT { get; set; }
    public Equipment EQUIP { get; set; }   
    public CharacterSkill SKILL { get; set; }
    public CharacterQuest QUEST { get; set; }    
    public Inventory INVEN { get; set; }
    public QuickSlot QUICKSLOT { get; set; }
    public Quest[] QUICKQUEST { get; set; }

    LinkedList<Unit> interactObj = new LinkedList<Unit>();
    BattleUnit nearEnermy;
    public void AddInteractObj(Unit _newObj)
    {
        interactObj.AddLast(_newObj);
    }
    public void RemoveInteractObj(Unit _newObj)
    {
        foreach(Unit one in interactObj)
        {
            if(one == _newObj)
            {
                interactObj.Remove(one);
            }
        }
    }
    public BattleUnit FindnearEnermy()
    {        
        foreach(Unit one in interactObj)
        {            
            if(one is BattleUnit)
            {
                if(nearEnermy == null)
                {
                    nearEnermy = (BattleUnit)one;
                }

                if(nearEnermy.DISTANCE > one.DISTANCE)
                {
                    nearEnermy = (BattleUnit)one;
                }
            }
        }
        return nearEnermy;
    }
    #region UiControl

    bool isUsingUi = false;
    public bool USINGUI
    {
        get
        {
            return isUsingUi;
        }
        set
        {
            isUsingUi = value;
        }
    }
    public float HP_CURENT => STAT.HP;
    public float Hp_Max => STAT.MAXHP;        
    public string NICKNAME => STAT.NAME;
    public float DISTANCE => 0;

   

    #endregion
    #region MoveItemControl   
    Item itemMoveItem;
    Item ITEMMOVEITEM { get { return itemMoveItem; } }
  
    ItemMove ChangeITemMove(ITEMLISTTYPE _itemListType)
    {
        switch (_itemListType)
        {
            case ITEMLISTTYPE.INVEN:
                return INVEN;
            case ITEMLISTTYPE.EQUIP:
                return EQUIP;
            case ITEMLISTTYPE.QUICK:
                return QUICKSLOT;

            default:
                Debug.LogError("존재하지 않는 ITEMLISTTYPE 입니다.");
                return null;
        }
    }
    
    public Item ItemList_GetItem(ITEMLISTTYPE _ListType, int _Index)
    {        
        itemMoveItem = ChangeITemMove(_ListType).GetItem(_Index);

        if(itemMoveItem== null)
        {
            return null;
        }
            
        return ITEMMOVEITEM;
    }



    public void ItemMove(ITEMLISTTYPE _StartListType, ITEMLISTTYPE _EndListType, int _StartListIndex, int _EndListIndex)
    {
        ItemMove start_ItemMove = ChangeITemMove(_StartListType);
        ItemMove end_ItemMove   = ChangeITemMove(_EndListType);

        Item startItem = start_ItemMove.GetItem(_StartListIndex);
        Item endItem   = end_ItemMove.GetItem(_EndListIndex);


        if(startItem == null && endItem == null)
        {
            Debug.LogError("빈 두아이템을 옮기려합니다.");            
        }
        else if(start_ItemMove.PossableMoveItem(_StartListIndex,endItem) && end_ItemMove.PossableMoveItem(_EndListIndex, startItem))
        {
            Item popItem = start_ItemMove.PopItem(_StartListIndex);  // 시작지점 아이템을 Pop 하여
            start_ItemMove.AddItem(_StartListIndex, end_ItemMove.Exchange(_EndListIndex, popItem));     // 목적지점 아이템과 교환
            


            UIManager.uimanager.updateUiSlot(_StartListType, _StartListIndex);
            UIManager.uimanager.updateUiSlot(_EndListType, _EndListIndex);

                     
        }
        
    }

    public void ItemMove_Auto(ITEMLISTTYPE _StartListType, int _StartListIndex)
    {
        ItemMove start_ItemMove = ChangeITemMove(_StartListType);        

        switch (_StartListType)
        {
            case ITEMLISTTYPE.EQUIP:
                {
                    ItemMove(_StartListType, ITEMLISTTYPE.INVEN, _StartListIndex, INVEN.Empty_SlotNum());
                    break;
                }
            case ITEMLISTTYPE.INVEN:
                {
                    Item rightClickitem = start_ItemMove.GetItem(_StartListIndex);
                    if(rightClickitem!= null&& rightClickitem.itemType == ITEMTYPE.EQUIPMENT)
                    {
                        ItemMove(_StartListType, ITEMLISTTYPE.EQUIP, _StartListIndex, (int)rightClickitem.EquipType);
                        break;
                        
                    }
                    break;
                }
            case ITEMLISTTYPE.QUICK:
                {
                    break;
                }
            default:
                break;

        }        
    }  
    #endregion

    public string GetChracterInfo()
    {        
        string Data = string.Empty;
        Data += Character.Player.STAT.NAME + ",";
        Data += GameManager.gameManager.MapName + ",";
        Data += Character.Player.transform.position.x + ",";
        Data += Character.Player.transform.position.y + ",";
        Data += Character.Player.transform.position.z + ",";
        Data += Character.Player.STAT.LEVEL + ",";
        Data += Character.Player.STAT.HP + ",";
        Data += Character.Player.STAT.MP + ",";
        Data += Character.Player.STAT.EXP + ",";
        Data += Character.Player.STAT.SkillPoint + ",";
        Data += Character.Player.STAT.GOLD + "\n";

        string invenInfo = INVEN.InvenInfo();
        string equipInfo = EQUIP.EqipInfo();
        string quickInfo = QUICKSLOT.QuickItemInfo();
        Data += invenInfo+"\n";
        Data += equipInfo+"\n";
        Data += quickInfo+"\n";

        return Data;
    }




    public List<SkinnedMeshRenderer> Weapon = new List<SkinnedMeshRenderer>();
    public SkinnedMeshRenderer Character_bounds;
    public List<Monster> MobList = new List<Monster>();
    public List<Npc> npcList = new List<Npc>();
    
    
    
    

    public GameObject Target;


    
    
    public Animator anim;
    
    
    // 상호작용
    public bool Interaction_B = false;
    public bool Interaction_T = false;   
    public bool Interaction_L = false;
   
    //상태
    
    public bool isInteract = false;          
    public bool isrecovery_Hp = false;
    public bool isrecovery_Mp = false;


    public bool OpenLootingbox = false; // 루팅박스 
    public Vector3 StartPos;

    public delegate void ItemUiSetting(ITEMLISTTYPE _Type,int _SlotNum);
    



    




    public void Deleteset()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Player = this;

        //
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < obj.Length; i++)
        {
            Weapon.Add(obj[i].GetComponent<SkinnedMeshRenderer>());
        }
        Character_bounds = transform.Find("Render").GetComponent<SkinnedMeshRenderer>();
        
        anim = GetComponent<Animator>();
        nav.updateRotation = false;
    }

   
    private void Start()
    {
        if(nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }
        nav.updateRotation = false;
        
    }

    void Update()
    {     
        Click();                
        
    }

    #region Move
    public bool isMove = false;
    IEnumerator prevMove;
    public void MovetoEmpty(Vector3 dest)
    {        
        nav.SetDestination(dest);
        isMove = true;
        if (prevMove != null)
        {
            StopCoroutine(prevMove);            
            prevMove = null;
        }
        prevMove = Move();
        StartCoroutine(prevMove);        
    }
    public void MovetoObject(GameObject _Target)
    {
        isMove = true;
        nav.SetDestination(_Target.transform.position);
        if (prevMove != null)
        {
            StopCoroutine(prevMove);            
            prevMove = null;
        }
        prevMove = MoveToObj(_Target);
        StartCoroutine(prevMove);        
    }
    IEnumerator Move()
    {
        while (true)
        {
            yield return null;
            if (nav.velocity.magnitude == 0f)
            {
                isMove = false;
            }           
            Vector3 dir = nav.steeringTarget - transform.position;
            dir.y = 0;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir.normalized, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);
            
        }
    }
    IEnumerator MoveToObj(GameObject _Target)
    {
        while (true)
        {
            yield return null;
            float Dis = Vector3.Distance(_Target.transform.position ,transform.position);
            if (nav.velocity.magnitude == 0f)
            {
                isMove = true;
            }           
            if (Dis <= STAT.ATK_RANGE)
            {
                nav.SetDestination(transform.position);
                yield break;
            }                
            Vector3 dir = new Vector3(nav.steeringTarget.x, transform.position.y, nav.steeringTarget.z) - transform.position;
            dir.y = 0;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir.normalized, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);
            
        }
    }
    #endregion
    #region Click / interact
    void Click()
    {
        if (isInteract == true)
        {
            return;
        }
            
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    if (hitinfo.collider.gameObject.layer == 9)
                    {
                        MovetoEmpty(hitinfo.point);
                        return;
                    }
                    else if (hitinfo.collider.gameObject.tag == "Item" ||
                        hitinfo.collider.gameObject.tag == "Npc"
                        || hitinfo.collider.gameObject.tag == "Monster")
                    {
                        interact(hitinfo.collider.gameObject);
                    }

                }

            }
        }
    }
    void interact(GameObject _target)
    {
        switch (_target.tag)
        {
            case "Monster":
                {
                    Monster mob = _target.GetComponent<Monster>();
                    if (mob == null)
                    {
                        Debug.LogError("잘못된 대상입니다 : Monster");
                        break;
                    }
                        
                    if (mob.DISTANCE <= STAT.ATK_RANGE)
                    {
                        nav.SetDestination(transform.position);
                        // Attck
                        break;
                    }
                    else
                    {
                        MovetoObject(mob.gameObject);
                        break;
                    }
                }                
            case "Npc":
                {
                    Npc npc = _target.GetComponent<Npc>();
                    if(npc == null)
                    {
                        Debug.LogError("잘못된 대상입니다 : Monster");
                        break;
                    }
                    if (npc.DISTANCE <= STAT.ATK_RANGE)
                    {
                        nav.SetDestination(transform.position);
                        UIManager.uimanager.OpenDialog(npc);
                        break;
                    }
                    else
                    {
                        MovetoObject(npc.gameObject);
                        break;
                    }
                }                
            case "Item":
                break;
            default:
                break;
        }
    }
    #endregion


    void LevelUp()
    {
        //STAT.LevelUp();
        ////GameObject LvEffect = ObjectPoolManager.objManager.EffectPooling("LevUp");
        ////TextMeshProUGUI LvString = ObjectPoolManager.objManager.stringEffect();
        //StartCoroutine(LevUpMove(LvEffect, LvString));
        //StartCoroutine(WaitForItLev(LvEffect, LvString.gameObject));
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
        //GameObject obj = ObjectPoolManager.objManager.EffectPooling(_name);        
        //obj.name = _name;
        //Vector3 Priset;
        //if (ObjectPoolManager.objManager.EffectPos_Dic.TryGetValue(obj,out Priset))
        //{
        //    obj.transform.position = transform.position + Priset;            
        //    obj.transform.rotation = transform.rotation;           
            
        //}        
        //StartCoroutine(WaitForIt(obj));
        
        
    }
    IEnumerator WaitForIt(GameObject obj)
    {
        yield return new WaitForSeconds(2f);        
        obj.SetActive(false);
        
    }
    public void IntersectsMob(int _num)
    {
        //for (int j = 0; j < MobList.Count; j++)
        //{
        //    if (Player.Weapon[_num].bounds.Intersects(MobList[j].MobBounds.bounds))
        //    {
        //        MobList[j].isDamage = true;
        //        float Dmg = Stat.ATK;
        //        MobList[j].Hp -= Stat.ATK;
        //        ObjectPoolManager.objManager.LoadDamage(MobList[j].gameObject, Dmg, Color.yellow, 1);
                
        //    }
        //}
    }
    public void  RangeMob()
    {
        //for (int i = 0; i < MobList.Count; i++)
        //{
        //    if (MobList[i].DiSTANCE <=2f)
        //    {
        //        MobList[i].isDamage = true;
        //        float Dmg = Stat.ATK * 3f;
        //        MobList[i].Hp -= Dmg;                
        //        ObjectPoolManager.objManager.LoadDamage(MobList[i].gameObject, Dmg, Color.yellow, 1);
        //    }
        //}
    }
    public void ListReset()
    {
        MobList.RemoveAll(x => true);
        npcList.RemoveAll(x => true);
    }
    //public Monster FindNearEnermy()
    //{
    //    Monster mob = MobList[0];
    //    for(int i =0; i < MobList.Count; i++)
    //    {
    //        if (mob.DiSTANCE > MobList[i].DiSTANCE)
    //            mob = MobList[i];
    //    }

    //    return mob;
    //}    
    public void burnStateOn()  // 화상
    {
        if(STAT.isburn == false)
        {
            StartCoroutine(BurnDamage());
        }
        

    }
    public void icestateOn()     // 빙결
    {
        if(STAT.isIce == false)
        {
            StartCoroutine(IceState());
        }
    }
    IEnumerator BurnDamage()
    {
        GameObject Effect;
        //Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Burn");
        float timer = 0;
        float holdTime = 5;
        STAT.isburn = true;
        while (true)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                timer -= 1;
                holdTime -= 1;
                STAT.HP -= 5;
                //ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, 10f, Color.red, 1);
            }
            //Effect.transform.position = Character.Player.transform.position;
            


            if(holdTime <= 0)
            {
                //Effect.SetActive(false);
                Effect = null;
                STAT.isburn = false;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator IceState()
    {
        GameObject Effect;
        //Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Ice");
        float timer = 0;
        STAT.isIce = true;        
        Character.Player.nav.speed -= 4f;
        Character.Player.anim.speed = 0.5f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >=5)
            {
                //Effect.SetActive(false);
                Effect = null;
                STAT.isIce = false;
                Character.Player.nav.speed += 4f;
                Character.Player.anim.speed = 1f;

                yield break;
            }
            //Effect.transform.position = Character.Player.transform.position;



            
            yield return null;
        }
    }
    public void soundPlayer(string _Name) 
    {
        SoundManager.soundmanager.soundsPlay(_Name, Character.Player.gameObject);    
    }

   
}
