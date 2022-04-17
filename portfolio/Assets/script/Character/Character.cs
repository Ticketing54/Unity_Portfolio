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
    public float HP_CURENT => stat.HP;
    public float Hp_Max => stat.MAXHP;
    public string NICKNAME => stat.NAME;

    public List<Collider> weapons;    
    Animator anim;    
  
    


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
        skill = new CharacterSkill();
        quest = new CharacterQuest();
        inven = new Inventory();
        quickSlot = new QuickSlot();
        quickQuest = new Quest[4];
        
        //stat = new Status();
        //equip = new Equipment();        
    }
    private void Start()
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }
        nav.updateRotation = false;
        anim = GetComponent<Animator>();
        
        HitBoxSetting();
    }
    
    void Update()
    {
        Click();


    }


    public LinkedList<Monster> nearMonster = new LinkedList<Monster>();    

    public void AddNearMonster(Monster _mob)
    {
        if(_mob is Monster)
        {
            nearMonster.AddLast((Monster)_mob);
        }
    }
    public void RemoveNearMonster(Monster _mob)
    {
        if(_mob is Monster)
        {
            nearMonster.Remove((Monster)_mob);
        }
    }


    public Monster ClosestMonster()
    {
        if (nearMonster.Count == 0)
        {
            return null;
        }            
        Monster nearMob = nearMonster.First.Value;
        foreach(Monster one in nearMonster)
        {
            if (one.DISTANCE < nearMob.DISTANCE)
                nearMob = one;
        }
        return nearMob;
    }
    NavMeshAgent nav;
    [Header("스텟")]
    public Status stat;
    [Header("장비")]
    public Equipment equip;
    [Header("스킬")]
    public CharacterSkill skill;
    [Header("퀘스트")]    
    public CharacterQuest quest;
    [Header("인벤토리")]
    public Inventory inven;
    [Header("퀵슬롯")]
    public QuickSlot quickSlot;


    public Quest[] quickQuest;

    
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
    
  
   

    #endregion
    #region MoveItemControl   
    Item itemMoveItem;
    Item ITEMMOVEITEM { get { return itemMoveItem; } }
  
    ItemMove ChangeITemMove(ITEMLISTTYPE _itemListType)
    {
        
        switch (_itemListType)
        {
            case ITEMLISTTYPE.INVEN:
                return inven;
            case ITEMLISTTYPE.EQUIP:
                return equip;
            case ITEMLISTTYPE.QUICK:
                return quickSlot;

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
                    ItemMove(_StartListType, ITEMLISTTYPE.INVEN, _StartListIndex, inven.Empty_SlotNum());
                    break;
                }
            case ITEMLISTTYPE.INVEN:
                {
                    Item rightClickitem = start_ItemMove.GetItem(_StartListIndex);
                    if(rightClickitem!= null&& rightClickitem.itemType == ITEMTYPE.EQUIPMENT)
                    {
                        ItemMove(_StartListType, ITEMLISTTYPE.EQUIP, _StartListIndex, (int)rightClickitem.equipType);
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
        Data += Character.Player.stat.NAME + ",";
        Data += GameManager.gameManager.MapName + ",";
        Data += Character.Player.transform.position.x + ",";
        Data += Character.Player.transform.position.y + ",";
        Data += Character.Player.transform.position.z + ",";
        Data += Character.Player.stat.LEVEL + ",";
        Data += Character.Player.stat.HP + ",";
        Data += Character.Player.stat.MP + ",";
        Data += Character.Player.stat.EXP + ",";
        Data += Character.Player.stat.SkillPoint + ",";
        

        string invenInfo = inven.InvenInfo();
        string equipInfo = equip.EqipInfo();
        string quickInfo = quickSlot.QuickItemInfo();
        Data += invenInfo+"\n";
        Data += equipInfo+"\n";
        Data += quickInfo+"\n";

        return Data;
    }

    public bool isCantMove = false;              
    public bool OpenLootingbox = false; // 루팅박스 
    
  
    #region Move
    bool isMove = false;
    Coroutine move;
    public void MovetoEmpty(Vector3 _dest)
    {
        if (move != null)
        {
            StopCoroutine(move);
        }

        anim.SetBool("IsMove", true);
        move =StartCoroutine(Move(_dest));        
    }    
    public void MovetoObject(GameObject _Target)
    {
        if(move != null)
        {
            StopCoroutine(move);
        }
        isMove = true;        
        float dis = Vector3.Distance(this.transform.position, _Target.transform.position);
        Vector3 stopPos = Vector3.MoveTowards(this.transform.position, _Target.transform.position,(dis-2f));//stat.ATK_RANGE
        nav.SetDestination(stopPos);
        move = StartCoroutine(Move(stopPos));
    }
    IEnumerator Move(Vector3 _dest)
    {
        nav.SetDestination(_dest);
        isMove = true;
        while (isMove)
        {            
            Vector3 dir = (nav.steeringTarget - transform.position).normalized;
            dir.y = 0;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);
            yield return null;

            if (nav.remainingDistance <=0.02&&nav.velocity.magnitude == 0f)
            {
                anim.SetBool("IsMove", false);
                isMove = false;
            }
            
        }
    }
   
    #endregion
    #region Click / interact
    void Click()
    {
        if (isCantMove == true)
        {
            return;
        }
            
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo))
            {
                if (hitinfo.collider.gameObject.layer == 9)
                {
                    EffectManager.effectManager.ClickEffectOn(hitinfo.point);
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
            //if (!EventSystem.current.IsPointerOverGameObject())
            //{
               

            //}
        }
    }
    void interact(GameObject _target)
    {
        switch (_target.tag)
        {
            case "Monster":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.ENERMY, _target.transform);
                    Monster mob = _target.GetComponent<Monster>();
                    if (mob == null)
                    {
                        Debug.LogError("잘못된 대상입니다 : Monster");
                        break;
                    }
                        
                    if (mob.DISTANCE <= 2f)//stat.ATK_RANGE
                    {
                        Attack();
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
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.FRIEND, _target.transform);
                    Npc npc = _target.GetComponent<Npc>();
                    if(npc == null)
                    {
                        Debug.LogError("잘못된 대상입니다 : Monster");
                        break;
                    }
                    if (npc.DISTANCE <= stat.ATK_RANGE)
                    {
                        nav.SetDestination(transform.position);
                        npc.Interact();                        
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

    #region Attack 
    public void KnockBack() 
    {
        StartCoroutine(CoKnockBack());
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    IEnumerator CoKnockBack()
    {
        float timer = 0f;
        nav.ResetPath();
        while (timer <= 4)
        {
            timer += Time.deltaTime;
            nav.velocity = - transform.forward * 8;
            yield return null;
        }
    }
    public void meleeDamageMob(int _num)
    {        
        foreach(Monster one in nearMonster)
        {
            if (weapons[_num].bounds.Intersects(one.hitBox.bounds))
            {
                one.Damaged(5f);       
            }
        }
       
    }
    public bool DamageMob(int _num,Monster _target)
    {
        if (weapons[_num].bounds.Intersects(_target.hitBox.bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void HitBoxSetting()
    {
        List<Collider> weaponList = new List<Collider>();
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < obj.Length; i++)
        {
            weaponList.Add(obj[i].GetComponent<Collider>());
        }
        weapons = weaponList;
    }
    #endregion



    #region Skill
    public delegate void UpdateBuffUi(string _skillSpriteName, float _amount);
    UpdateBuffUi updateBufUi;
    public void TakeDownSword() // 애니메이션 이벤트                                     //skill 0번
    {
        isCantMove = true;
        //이펙트 연출
        StartCoroutine(CoTakeDownSword());
        anim.SetFloat("SkillNum", 0);
        anim.SetTrigger("Skill");
        
    }
    public void RushSkill()                                                              //skill 1번
    {
        // 이펙트 추가 할 것
        anim.SetFloat("SkillNum", 1);
        anim.SetTrigger("Skill");        
        Monster closestMob = ClosestMonster();
        if (nearMonster != null)
        {
            transform.LookAt(closestMob.transform);
        }
        StartCoroutine(CoRushSkill());
    }
    public void BuffSkill(int _skillIndex, int _count)                                     //skill 2번
    {
        anim.SetFloat("SkillNum", 2);

    }
    public void SkillEffectOn(string _effectName)
    {

    }
    public void RangeDamageMob() 
    {
        foreach (Monster _mob in nearMonster)
        {
            if (_mob.DISTANCE <= 2f)
            {
                _mob.StatusEffect(STATUSEFFECT.KNOCKBACK, 2f);
                _mob.StatusEffect(STATUSEFFECT.STURN, 2f);
                _mob.Damaged(5f);
            }
        }
    }
    IEnumerator CoTakeDownSword()
    {
        yield return new WaitForSeconds(2f);
        isCantMove = false;
    }
    
    IEnumerator CoRushSkill()
    {        
        isCantMove = true;
        nav.ResetPath();        
        float timer = 0f;        
        DamageList list = new DamageList();
        foreach(Monster mob in nearMonster)
        {
            list.Add(mob);
        }
        while (timer <= 1.2)
        {
            timer += Time.deltaTime;
            nav.velocity = transform.forward * 2;
            list.DamedMonster(5f,STATUSEFFECT.STURN,1f);
            yield return null;
        }
        isCantMove = false;
    }


    #endregion

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
        if(stat.isburn == false)
        {
            StartCoroutine(BurnDamage());
        }
        

    }
    public void icestateOn()     // 빙결
    {
        if(stat.isIce == false)
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
        stat.isburn = true;
        while (true)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                timer -= 1;
                holdTime -= 1;
                stat.HP -= 5;
                //ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, 10f, Color.red, 1);
            }
            //Effect.transform.position = Character.Player.transform.position;
            


            if(holdTime <= 0)
            {
                //Effect.SetActive(false);
                Effect = null;
                stat.isburn = false;
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
        stat.isIce = true;        
        Character.Player.nav.speed -= 4f;
        Character.Player.anim.speed = 0.5f;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >=5)
            {
                //Effect.SetActive(false);
                Effect = null;
                stat.isIce = false;
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
