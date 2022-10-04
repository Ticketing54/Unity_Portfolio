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

    public float actionTime;
    public Bounds AttackBox { get => new Bounds(transform.position+transform.forward*0.5f, new Vector3(3f, 3f, 3f)); }

    public HashSet<GameObject> hitMob;

    HashSet<InteractInterface> nearUnit;
    
    Coroutine action;
    Animator anim;
    NavMeshAgent nav;

    Dictionary<KeyCode, Action> KeyboardSortCut;


    public bool IsPossableControl = true;

    public List<Item> dropBox;


    public void AddNearInteract(InteractInterface _unit)
    {
        if (!nearUnit.Contains(_unit))
        {
            nearUnit.Add(_unit);
        }
    }
    public void RemoveInteract(InteractInterface _unit)
    {
        if (nearUnit.Contains(_unit))
        {
            nearUnit.Remove(_unit);
        }
    }
  
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
            inven.AddGold(_gold);
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
        hitMob = new HashSet<GameObject>();
        nearUnit = new HashSet<InteractInterface>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        KeyboardSortCut = new Dictionary<KeyCode, Action>();

        skill = new CharacterSkill(this, nav, anim);
        quest = new CharacterQuest(this);
        quickSlot = new Character_Quick(this);
        stat = new Status(this);
        equip = new Equipment(this);
        inven = new Inventory(this);


        actionTime = 0f;
        nav.updateRotation = false;

        KeyboardSortCut.Add(KeyCode.N, UIManager.uimanager.TryOpenMinimap_Min);
        KeyboardSortCut.Add(KeyCode.M, UIManager.uimanager.TryOpenMinimap_Max);
        KeyboardSortCut.Add(KeyCode.G, Interact);
    }

    void Update()
    {
        if(IsPossableControl == false)
        {
            return;
        }
        Inputkeyboard();
        Click();
        LevelUpTest();        
    }
    
    public void Sturn(float _time)
    {
        actionTime = _time;
        anim.SetBool("Sturn", true);
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


    AudioSource test;
    void LevelUpTest()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {

            stat.LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (test == null)
            {
                test = SoundManager.soundmanager.GetSounds("Turtle");
            }        
            
            test.Play();
        }
    }

    

    public Monster ClosestMonster(float _distance)
    {
        Collider[] mobs = Physics.OverlapSphere(transform.position,_distance);
        if(mobs == null)
        {
            return null;
        }
        else
        {
            Monster mob = null;
            for (int i = 0; i < mobs.Length; i++)
            {
                if(mobs[i].tag == "Monster")
                {
                    Monster otherMonster = mobs[i].GetComponent<Monster>();
                    if(mob == null)
                    {
                        mob = otherMonster;
                    }
                    else
                    {
                        if (mob.Distance > otherMonster.Distance)
                        {
                            mob = otherMonster;
                        }
                    }

                }
            }

            return mob;
        }        
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

    void Interact()
    {  
        InteractInterface nearestUnit = NearestUnit();

        if(nearestUnit == null)
        {
            return;
        }
        nav.ResetPath();
        nearestUnit.interact();
    }

    public InteractInterface NearestUnit()
    {
        if(nearUnit.Count == 0)
        {
            return null;
        }
        else
        {
            List<InteractInterface> units = new List<InteractInterface>(nearUnit);
            InteractInterface nearestUnit = units[0];
            for (int i = 0; i < units.Count; i++)
            {
                if (nearestUnit.Distance >= units[i].Distance)
                {
                    nearestUnit = units[i];
                }
            }
            return nearestUnit;
        }
    }
    #region Move


    bool isMove = false;

    public void Move(Vector3 _targetPos, bool isTarget = false)
    {      
        if (action != null)
        {
            StopCoroutine(action);
            action = null;
        }

        action = StartCoroutine(CoMove(_targetPos,isTarget));
    }  

    public void CanMove()
    {
        nav.Warp(transform.position);
        nav.updatePosition = true;
        isPossableMove = true;
    }
    public void StopMove()
    {
        if(action != null)
        {
            StopCoroutine(action);
            action = null;
        }
        
        
        anim.SetBool("IsMove", false);        
        nav.ResetPath();
        nav.stoppingDistance = 0;
        nav.updatePosition = false;
    }
    IEnumerator CoMove(Vector3 _targetPos,bool isTarget = false)
    {
        
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
                
                anim.SetBool("IsMove", false);
                break;
            }
        }
    }

    #endregion
   
    public void Click()
    {
      
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1))
            { 
                if(isPossableMove == false)
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
                    MouseButtonRIght(hitinfo.collider.gameObject);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }

        }
    }
    Coroutine attack;
    public bool isPossableAttack = true;
    void Attack()
    {   
        if(isPossableAttack == false)
        {
            return;
        }
        nav.ResetPath();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo))
        {
            if (hitinfo.collider.gameObject.layer == 9)
            {
                if (attack != null)
                {
                    StopCoroutine(attack);
                }
                attack = StartCoroutine(CoAttack(hitinfo.point));
            }
        }
    }
    IEnumerator CoAttack(Vector3 _targetPos)
    {
        Vector3 dir = (_targetPos - transform.position).normalized;
        dir.y = 0;
        Quaternion targetdir = Quaternion.LookRotation(dir);
        while (true)
        {
            yield return null;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * 30f, Time.deltaTime * 30f);
            transform.rotation = Quaternion.LookRotation(newdir);

            if(gameObject.transform.rotation == targetdir)
            {
                break;
            }
        }
        isPossableAttack = false;
        anim.SetTrigger("Attack");
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
                    break;
                }
            default:
                break;
        }

        Move(_target.transform.position, true);
    }

    public void AddKeyBoardSortCut(KeyCode _keycode, Action _action)
    {
        if (KeyboardSortCut.ContainsKey(_keycode))
        {
            KeyboardSortCut.Remove(_keycode);
            KeyboardSortCut.Add(_keycode, _action);
        }
        else
        {
            KeyboardSortCut.Add(_keycode, _action);
        }
    }
    public void RemoveKeyBoardSortCut(KeyCode _keycode)
    {
        if (KeyboardSortCut.ContainsKey(_keycode))
        {
            KeyboardSortCut.Remove(_keycode);
        }
        else
        {
            Debug.Log("없는 단축키를 없애려 합니다.");
        }
    }

    void Inputkeyboard()
    {
        if (Input.anyKey)
        {
            foreach (KeyValuePair<KeyCode, Action> input in KeyboardSortCut)
            {
                if (Input.GetKeyDown(input.Key))
                {
                    input.Value();
                }
            }
        }
    }
}
