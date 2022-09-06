using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Character : MonoBehaviour
{

    public Character() { }

    public Vector3 StartPos { get; set; }
    public float HP_CURENT => stat.Hp;
    public float Hp_Max => stat.MaxHp;
    public string NICKNAME => stat.NAME;

    Bounds hitBox;
    public Bounds AttackBox { get => new Bounds(transform.position+transform.forward*0.5f, new Vector3(3f, 3f, 3f)); }
    
    

    
    Coroutine action;
    Animator anim;

    public HashSet<Unit> nearUnit;
    public HashSet<Monster> nearMonster;
    Rigidbody rig;


    NavMeshAgent nav;

    public delegate void NearUnit(Unit _unit);
    public NearUnit addNearUnit;
    public NearUnit removeNearUnit;

    public List<Item> dropBox;

  
    public void OpenDropBox(List<Item> _dropBox)
    {
        dropBox = _dropBox;
        UIManager.uimanager.AOpenDropBox();
    }
    public Action<string> RewardUpdate;
    public void GetReward_Monster(int _gold, int _exp)
    {
        if (_gold != 0)
        {
            inven.GetGold(_gold);
        }

        if (_exp != 0)
        {
            stat.GetExp(_exp);
        }
    }
    public void MoveToDropItem(int _index, int _count)
    {
        Item item = dropBox[_index];
        Item moveItem = new Item(item.index, _count);
        if (inven.PushItem(moveItem))
        {
            if (item.ItemCount <= _count)
            {
                dropBox[_index] = null;
            }
            else
            {
                dropBox[_index].ItemCount -= _count;
            }
        }
        else
        {
            return;
        }
    }
    public bool MoveToDropItem_All()
    {
        if(dropBox == null)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < dropBox.Count; i++)
            {
                Item item = dropBox[i];
                if(inven.PushItem(item))
                {
                    dropBox[i] = null;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
 
    public Item GetDropBoxItem(int _index)
    {
        if (dropBox == null || _index < 0 || _index > dropBox.Count - 1)
        {
            return null;
        }
        else
        {
            return dropBox[_index];
        }
    }

    private void Awake()
    {
        nearUnit = new HashSet<Unit>();
        nearMonster = new HashSet<Monster>();

        addNearUnit += AddNearUnit;
        removeNearUnit += RemoveNearUnit;


        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();        


        skill = new CharacterSkill(this, nav, anim);
        quest = new CharacterQuest(this);
        quickSlot = new Character_Quick(this);
        stat = new Status(this);
        equip = new Equipment(this);
        inven = new Inventory();
        
        


        nav.updateRotation = false;        
    }

    void Update()
    {
        Click();
        LevelUpTest();        
    }
    
    public List<Quest> GetQuestList(QUESTSTATE _state)
    {
        return quest.GetQuestList(_state);
    }

    public void SetPosition(Vector3 _Pos)
    {
        nav.Warp(_Pos);
    }
    public void SetPosition()
    {
        nav.Warp(StartPos);
    }


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
    public Character_Quick quickSlot;


    void LevelUpTest()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            stat.LevelUp();
        }
    }

    #region ApproachUnit
    void AddNearUnit(Unit _unit)
    {
        if (_unit is Monster == true)
        {
            Monster unit = (Monster)_unit;
            Monster mob;
            if (!nearMonster.TryGetValue(unit, out mob))
            {
                nearMonster.Add(unit);
            }
        }
        nearUnit.Add(_unit);
    }

    void RemoveNearUnit(Unit _unit)
    {
        if (_unit is Monster == true)
        {
            nearMonster.Remove((Monster)_unit);
        }
        nearUnit.Remove(_unit);
    }

    public Monster ClosestMonster()
    {
        if (nearMonster.Count == 0)
        {
            return null;
        }
        List<Monster> mobList = new List<Monster>(nearMonster);
        Monster nearMob = mobList[0];

        foreach (Monster one in mobList)
        {
            if (one.DISTANCE < nearMob.DISTANCE)
                nearMob = one;
        }
        return nearMob;
    }
    #endregion

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

        if (itemMoveItem == null)
        {
            return null;
        }

        return ITEMMOVEITEM;
    }



    public void ItemMove(ITEMLISTTYPE _startListType, ITEMLISTTYPE _endListType, int _startListIndex, int _endListIndex)
    {
        if (_startListIndex == _endListIndex && _startListIndex == _endListIndex)
        {
            return;
        }

        ItemMove start_ItemMove = ChangeITemMove(_startListType);
        ItemMove end_ItemMove = ChangeITemMove(_endListType);

        Item startItem = start_ItemMove.GetItem(_startListIndex);
        Item endItem = end_ItemMove.GetItem(_endListIndex);


        if (startItem == null && endItem == null)
        {
            Debug.Log("빈 두아이템을 옮기려합니다.");
        }
        else if (start_ItemMove.PossableMoveItem(_startListIndex, endItem) && end_ItemMove.PossableMoveItem(_endListIndex, startItem))
        {
            Item popStartItem = start_ItemMove.PopItem(_startListIndex);
            Item popEndItem = end_ItemMove.PopItem(_endListIndex);

            if (popStartItem != null && popEndItem != null)
            {
                if ((popStartItem.index == popEndItem.index) && (popStartItem.itemType != ITEMTYPE.EQUIPMENT || popEndItem.itemType != ITEMTYPE.EQUIPMENT))
                {
                    popStartItem.ItemCount += popEndItem.ItemCount;
                    end_ItemMove.AddItem(_endListIndex, popStartItem);
                }
                else
                {
                    start_ItemMove.AddItem(_startListIndex, popEndItem);
                    end_ItemMove.AddItem(_endListIndex, popStartItem);
                }
            }
            else
            {

                start_ItemMove.AddItem(_startListIndex, popEndItem);
                end_ItemMove.AddItem(_endListIndex, popStartItem);

            }

            UIManager.uimanager.ItemUpdateSlot(_endListType, _endListIndex);
        }
    }
    public void ItemMove_DropBoxtoInven(ITEMLISTTYPE _endListtype, int startListIndex, int _endListIndex)
    {
        if (inven.IsInvenFull())
        {
            return;
        }




    }

    public void ItemMove_Auto(ITEMLISTTYPE _StartListType, int _StartListIndex)
    {
        ItemMove start_ItemMove = ChangeITemMove(_StartListType);

        switch (_StartListType)
        {
            case ITEMLISTTYPE.EQUIP:
                {
                    ItemMove(ITEMLISTTYPE.EQUIP, ITEMLISTTYPE.INVEN, _StartListIndex, inven.Empty_SlotNum());
                    break;
                }
            case ITEMLISTTYPE.INVEN:
                {
                    Item rightClickitem = start_ItemMove.GetItem(_StartListIndex);
                    if (rightClickitem != null && rightClickitem.itemType == ITEMTYPE.EQUIPMENT)
                    {
                        ItemMove(ITEMLISTTYPE.INVEN, ITEMLISTTYPE.EQUIP, _StartListIndex, (int)rightClickitem.equipType);
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

    public bool isPossableMove = true;
    public bool OpenLootingbox = false; // 루팅박스 


    #region Move


    bool isMove = false;

    public void Move(Vector3 _targetPos, bool isTarget = false)
    {
        if(isPossableMove == false)
        {
            return;
        }

        if (action != null)
        {
            StopCoroutine(action);
            action = null;
        }

        action = StartCoroutine(CoMove(_targetPos,isTarget));
    }   
    IEnumerator CoMove(Vector3 _targetPos,bool isTarget = false)
    {
        isMove = true;
        anim.SetBool("IsMove", true);        
        nav.SetDestination(_targetPos);

        if(isTarget == true)
        {
            nav.stoppingDistance = stat.ATK_RANGE;
        }
        else
        {
            nav.stoppingDistance = 0;
        }
        
        while (true)
        {
            yield return null;

            Vector3 dir = (nav.steeringTarget - transform.position).normalized;
            dir.y = 0;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);

            if (nav.remainingDistance <= stat.ATK_RANGE && nav.velocity.magnitude == 0f)
            {
                isMove = false;
                anim.SetBool("IsMove", false);
                break;
            }
        }
    }

    Coroutine waitForDoing;
    bool InteractSuccess = false;



    void MoveToScene(string _sceneName, Vector3 _pos,MAPTYPE _type,string _cutScene = "")
    {
        if (waitForDoing != null)
        {
            return;
        }

        waitForDoing = StartCoroutine(CoMoveToScene(5f, _sceneName,_pos, _type,_cutScene));
    }

    IEnumerator CoMoveToScene(float _timer, string _sceneName, Vector3 _pos,MAPTYPE _type,string _cutScene)
    {
        string waitForDoingText = _sceneName + " 으로 이동 하는중 입니다.";
        yield return StartCoroutine(CoWaitForDoing(_timer, waitForDoingText));


        if (InteractSuccess == true)
        {
            GameManager.gameManager.MoveToScene(_sceneName, _pos,_type,_cutScene);
            InteractSuccess = false;
        }
        waitForDoing = null;
    }

    IEnumerator CoWaitForDoing(float _timerMax, string _text)
    {
        UIManager.uimanager.OpenWaitForDoing(_text);

        float timer = 0.01f;
        while (timer <= _timerMax)
        {
            if (isMove == true)
            {
                InteractSuccess = false;
                UIManager.uimanager.ExitWaitForDoing();
                yield break;
            }
            UIManager.uimanager.RunningWaitForDoing(timer / _timerMax);
            timer += Time.deltaTime;
            yield return null;
        }
        InteractSuccess = true;
        UIManager.uimanager.ExitWaitForDoing();
    }

    #endregion
    #region Click / interact
    void Click()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (isPossableMove == false)
                {
                    return;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    if (hitinfo.collider.gameObject.layer == 9)
                    {
                        EffectManager.effectManager.ClickEffectOn(hitinfo.point);
                        Move(hitinfo.point);
                        return;
                    }
                    else if (hitinfo.collider.gameObject.tag.Equals("Item") ||
                        hitinfo.collider.gameObject.tag.Equals("Npc")
                        || hitinfo.collider.gameObject.tag.Equals("Monster")
                        || hitinfo.collider.gameObject.tag.Equals("Potal"))
                    {

                        MouseButtonRIght(hitinfo.collider.gameObject);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    if (hitinfo.collider.gameObject.tag.Equals("Item") ||
                        hitinfo.collider.gameObject.tag.Equals("Npc")
                        || hitinfo.collider.gameObject.tag.Equals("Monster")
                        || hitinfo.collider.gameObject.tag.Equals("Potal"))
                    {
                        MouseButtonLeft(hitinfo.collider.gameObject);
                    }
                    else
                    {
                        Attack();
                    }
                }
                else
                {
                    Attack();
                }
            }

        }
    }
    void MouseButtonRIght(GameObject _target)
    {
        switch (_target.tag)
        {
            case "Monster":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.ENERMY, _target.transform);                    
                    break;
                }
            case "Npc":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.FRIEND, _target.transform);
                    break;
                }
            case "Item":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.FRIEND, _target.transform);

                    if (_target.layer == 10)            // 거리에 따른 효과
                    {
                        Monster mob = _target.GetComponent<Monster>();

                        if (mob == null)
                        {
                            Debug.LogError("잘못된 대상입니다 : Monster");
                            break;
                        }
                        if (mob.DISTANCE <= stat.ATK_RANGE)
                        {
                            mob.DropItem();
                            break;
                        }
                        else
                        {
                            Move(mob.transform.position,true);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }


                }
            case "Potal":
                {
                    Potal potal = _target.GetComponent<Potal>();

                    if (potal == null)
                    {
                        Debug.LogError("잘못된 대상입니다. : Potal");
                        break;
                    }

                    if (potal.DISTANCE <= stat.ATK_RANGE)
                    {
                        nav.SetDestination(transform.position);
                        MoveToScene(potal.MapName,potal.Pos, potal.MapType,potal.CutScene);
                        return;
                    }
                    else
                    {
                        
                        break;
                    }
                }
            default:
                break;
        }

        Move(_target.transform.position, true);
    }
    void MouseButtonLeft(GameObject _target)
    {
        switch (_target.tag)
        {
            case "Monster":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.ENERMY, _target.transform);
                    transform.LookAt(_target.transform);
                    Attack();
                    break;
                }
            case "Npc":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.FRIEND, _target.transform);
                    Npc npc = _target.GetComponent<Npc>();
                    if(npc == null)
                    {
                        Debug.LogError("wrong Target : Npc");
                        return;
                    }
                    npc.Interact();                
                    break;
                }
            case "Item":
                {
                    EffectManager.effectManager.ClickEffectOn(CLICKEFFECT.FRIEND, _target.transform);

                    if (_target.layer == 10)            // 거리에 따른 효과
                    {
                        Monster mob = _target.GetComponent<Monster>();

                        if (mob == null)
                        {
                            Debug.LogError("잘못된 대상입니다 : Monster");
                            break;
                        }
                        if (mob.DISTANCE <= stat.ATK_RANGE)
                        {
                            mob.DropItem();
                            break;
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }


                }
            case "Potal":
                {
                    Potal potal = _target.GetComponent<Potal>();

                    if (potal == null)
                    {
                        Debug.LogError("잘못된 대상입니다. : Potal");
                        break;
                    }

                    if (potal.DISTANCE <= stat.ATK_RANGE)
                    {   
                        MoveToScene(potal.MapName, potal.Pos,potal.MapType,potal.CutScene);
                        break;
                    }
                    break;
                }
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

    bool isPossableAttack = true;
    Coroutine delayAttack;
    public void Attack()
    {
        nav.destination = transform.position;
        if (isPossableAttack == false)
        {
            return;
        }
        else
        {
            isPossableAttack = false;
            isPossableMove   = false;
        }

        if (delayAttack != null)
        {
            StopCoroutine(delayAttack);
            isPossableAttack = false;
            isPossableMove = false;
        }
        
        delayAttack = StartCoroutine(DelayAttack());
        anim.SetTrigger("Attack");

    }
    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.7f);
        isPossableAttack = true;
        yield return new WaitForSeconds(0.5f);
        isPossableMove   = true;
    }
    IEnumerator CoKnockBack()
    {
        float timer = 0f;
        nav.ResetPath();
        while (timer <= 4)
        {
            timer += Time.deltaTime;
            nav.velocity = -transform.forward * 8;
            yield return null;
        }
    }
    public void meleeDamageMob(int _num)
    {
        List<Monster> mob = new List<Monster>(nearMonster);
        Bounds hitbox = AttackBox;
        foreach (Monster one in mob)
        {
            if (hitbox.Intersects(one.hitBox.bounds))
            {
                one.Damaged(stat.DamageType(), (int)stat.AttckDamage);
            }
        }
        Debug.Log(transform.position);
        Debug.Log(transform.forward * 0.5f);
    }
    public bool DamageMob(int _num, Monster _target)
    {
        if (AttackBox.Intersects(_target.hitBox.bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }   
    public void RangeDamageMob()
    {
        foreach (Monster _mob in nearMonster)
        {
            if (_mob.DISTANCE <= 2f)
            {
                _mob.StatusEffect(STATUSEFFECT.KNOCKBACK, 2f);
                _mob.StatusEffect(STATUSEFFECT.STURN, 2f);
                _mob.Damaged(stat.DamageType(), (int)stat.AttckDamage);
            }
        }
    }
    #endregion




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






    IEnumerator LevUpMove(GameObject obj, TextMeshProUGUI _string)  //레벨업 
    {

        Vector3 Pos = transform.position;
        Pos.y += 1f;
        _string.fontSize = 33;
        while (obj.activeSelf == true)
        {

            obj.transform.position = gameObject.transform.position;
            _string.transform.position = Camera.main.WorldToScreenPoint(new Vector3(Pos.x, Pos.y += Time.deltaTime * 0.6f, Pos.z));
            _string.fontSize += Time.deltaTime * 20f;
            _string.alpha -= Time.deltaTime * 0.6f;
            yield return null;

        }

        yield return null;

    }
    IEnumerator WaitForItLev(GameObject obj, GameObject _string) // 레벨업
    {
        yield return new WaitForSeconds(2.5f);
        obj.SetActive(false);
        _string.SetActive(false);
    }


    //public void burnStateOn()  // 화상
    //{
    //    if(stat.isburn == false)
    //    {
    //        StartCoroutine(BurnDamage());
    //    }


    //}
    //public void icestateOn()     // 빙결
    //{
    //    if(stat.isIce == false)
    //    {
    //        StartCoroutine(IceState());
    //    }
    //}
    IEnumerator BurnDamage()
    {
        GameObject Effect;
        //Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Burn");
        float timer = 0;
        float holdTime = 5;
        //stat.isburn = true;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer -= 1;
                holdTime -= 1;
                stat.Hp -= 5;
                //ObjectPoolManager.objManager.LoadDamage(Character.Player.gameObject, 10f, Color.red, 1);
            }
            //Effect.transform.position = Character.Player.transform.position;



            if (holdTime <= 0)
            {
                //Effect.SetActive(false);
                Effect = null;
                //stat.isburn = false;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator IceState()
    {
        yield return null;
        //GameObject Effect;
        ////Effect = Effect = ObjectPoolManager.objManager.EffectPooling("Ice");
        //float timer = 0;
        //stat.isIce = true;        
        //Character.Player.nav.speed -= 4f;
        //Character.Player.anim.speed = 0.5f;
        //while (true)
        //{
        //    timer += Time.deltaTime;
        //    if (timer >=5)
        //    {
        //        //Effect.SetActive(false);
        //        Effect = null;
        //        stat.isIce = false;
        //        Character.Player.nav.speed += 4f;
        //        Character.Player.anim.speed = 1f;

        //        yield break;
        //    }
        //    //Effect.transform.position = Character.Player.transform.position;




        //    yield return null;
        //}
    }
    public void soundPlayer(string _Name)
    {
        //SoundManager.soundmanager.soundsPlay(_Name, Character.Player.gameObject);    
    }


}
