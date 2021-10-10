using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    //슬롯 모음
    public List<Slot> list = new List<Slot>();      // 인벤토리
    public List<ShopSlot> shop_list = new List<ShopSlot>(); // 상점   


    public ShopSlot buyitem;   //상점 영역
    public Vector2 OldClickPos; // 클릭조건
    public int WorkingSlot = -1; // 운용중인 슬롯
    // 드래그 클릭 관련 오브젝트
    public Image MoveIcon = null;
    public Item Moveitem = null;
    public Item Clickitem = null;
    public Image ClickitemImage = null;


        
    public MiniInfo miniinfo; // 정보창

    public bool isdrag = false;
    public bool isbuy = false;
    public bool issell = false;
    public bool ItemInfo = false;
    public bool isinventory = false;


    public int Price = 0;
    //판매
    public int sellItem_num = -1; // 판매할 슬롯 번호
    public int buyItem_num = -1; // 판매할 슬롯 번호
    public TextMeshProUGUI sellitem_name; // 판매할 아이템 이름
    public TextMeshProUGUI sell_Price;
    public Image sellMessage; // 판매 메세지    
    // 구매
    public TextMeshProUGUI buyitem_name; // 구매할 아이템 이름
    public TextMeshProUGUI buy_Price;
    public Image buyMessage; // 구매 메세지
    public Image CantBuy; // 금액부족 메세지



    //온오프

    public GameObject Inventory;    
    
   




    public void OnPointerDown(PointerEventData data)
    {        
        OldClickPos = data.position; // 클릭 포인트        
        if (miniinfo.gameObject.activeSelf == true)
            miniinfo.gameObject.SetActive(false);


        


        if (Input.GetMouseButton(0))
        {                    
            
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].isInRect(data.position) && list[i].itemImage.gameObject.activeSelf == true)
                {
                    Clickitem = list[i].item;
                    ClickitemImage = list[i].itemImage;
                    ItemInfo = true;
                    Begin_DragSlot(i);
                    MoveIcon.transform.position = data.position;
                    isinventory = true;
                    return;
                }
                

            }



            for(int j = 0; j < shop_list.Count; j++)
            {
                if (shop_list[j].isInRect(data.position) && shop_list[j].ItemImage.gameObject.activeSelf == true)
                {
                    Clickitem = shop_list[j].item;
                    ClickitemImage = shop_list[j].ItemImage;
                    ItemInfo = true;
                    WorkingSlot = j;
                    OldClickPos = data.position;                    
                    return;
                }
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].isInRect(data.position) && list[i].itemImage.gameObject.activeSelf == true)
                {
                    Clickitem = list[i].item;
                    ClickitemImage = list[i].itemImage;
                    issell = true;
                    OldClickPos = data.position;
                    WorkingSlot = i;

                    return;
                }

            }

            for (int j = 0; j < shop_list.Count; j++)
            {
                if (shop_list[j].isInRect(data.position) && shop_list[j].ItemImage.gameObject.activeSelf == true)
                {
                    Clickitem = shop_list[j].item;
                    ClickitemImage = shop_list[j].ItemImage;
                    isbuy = true;
                    OldClickPos = data.position;
                    WorkingSlot = j;
                    return;
                }
            }


        }

    }
    public void OnPointerUp(PointerEventData data)
    {
        if (Input.GetMouseButtonUp(0) && OldClickPos == data.position && ItemInfo == true)   // 좌 클릭 시
        {
            if (WorkingSlot < 0)
                return;
            if(isinventory == true)
            {
                isinventory = false;
                End_Drag_Empty(WorkingSlot);
            }
            Vector3 Pos;
            Pos = new Vector3(data.position.x + 75f, data.position.y - 100f, 0);           
            
            ItemInfo = false;
            End_Click_L(Pos);
            return;

        }
        if (Input.GetMouseButtonUp(1) && OldClickPos == data.position && isbuy == true) // 우 클릭 시 // 구매
        {
            if (WorkingSlot < 0)
                return;
            isbuy = false;
            buy_item_message();

            return;



        }
        else if(Input.GetMouseButtonUp(1) && OldClickPos == data.position && issell) // 판매
        {
            if (WorkingSlot < 0)
                return;
            issell = false;
            sell_Item_message(WorkingSlot);


            return;

        }

        if (WorkingSlot < 0 || isinventory == false)
            return;
        if (Input.GetMouseButtonUp(0))
        {

            if (buyitem.isInRect(data.position) )
            {
                sell_Item_message(WorkingSlot);
                return;
            }



            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].isInRect(data.position) && list[i].itemImage.gameObject.activeSelf == false)
                {
                    End_Drag_Empty(i);
                    return;
                }
                else if (list[i].isInRect(data.position) && list[i].item.Index == Moveitem.Index)
                {
                    End_Drag_Same(i);
                    return;
                }
                else if (list[i].isInRect(data.position) && list[i].itemImage.gameObject.activeSelf == true)
                {
                    End_Drag_Different(i);
                    return;
                }

            }           
            if (WorkingSlot >= 0 && MoveIcon.gameObject.activeSelf == true)
            {

                End_Drag_Empty(WorkingSlot);
                return;

            }
        }
    }

    //

 
    public void OnDrag(PointerEventData data)
    {        
        MoveIcon.rectTransform.position = data.position;
    }
   
 
    public void End_Click_L(Vector3 Pos)
    {
        if (Clickitem != null && ClickitemImage != null)
        {
            miniinfo.gameObject.SetActive(true);
            miniinfo.gameObject.transform.position = Pos;
            miniinfo.ItemImage.sprite = ClickitemImage.sprite;
            miniinfo.ItemName.text = Clickitem.ItemName;
            miniinfo.ItemType.text = Clickitem.itemType.ToString();
            miniinfo.ExPlain.text = Clickitem.ItemExplain;

            string[] tmp = Clickitem.ItemProperty.Split('/');
            miniinfo.Property.text = tmp[0] + " + " + tmp[1];
        }

    }



    





    //상점
    public void ShopUpdate(Npc _npc)
    {

        for(int i = 0; i < _npc.item_list.Count; i++)
        {
            shop_list[i].AddItem(_npc.item_list[i]);
        }
    }

    public void sell_Item_message(int _num) // 물건판매 메세지
    {
        if (buyMessage.gameObject.activeSelf == true)
        {
            buyMessage.gameObject.SetActive(false);
        }

        sellItem_num = WorkingSlot;
        if(isdrag == true)
            End_Drag_Empty(_num);

        sellMessage.gameObject.SetActive(true);
        sellitem_name.text = list[sellItem_num].item.ItemName;
        Price = (int)(list[sellItem_num].item.ItemPrice / 2);
        sell_Price.text = "+ "+Price+" G";
         
    } // 판매 메세지
    public void sell_item()
    {        
        sellMessage.gameObject.SetActive(false);
        Character.Player.Stat.GOLD += Price;
        Price = 0;
        list[sellItem_num].SlotClear();
        sellItem_num = -1;
        
    }  //물건을 판매
    public void Cancle_sell_item()// 물건을 판매 취소
    {
        sellItem_num = -1;
        sellMessage.gameObject.SetActive(false);
        
    } 
    public void CantbuyItem()   // 금액부족
    {
        
        sellItem_num = -1;
        CantBuy.gameObject.SetActive(false);
        
    }
    public void buy_item_message() // 물건구매 메세지
    {
        if (sellMessage.gameObject.activeSelf== true)
        {
            sellMessage.gameObject.SetActive(false);
        }
        buyItem_num = WorkingSlot;        
        buyMessage.gameObject.SetActive(true);
        buyitem_name.text = shop_list[buyItem_num].item.ItemName;
        Price = shop_list[buyItem_num].item.ItemPrice;
        buy_Price.text = "- " + Price+" G";
        WorkingSlot = -1;

    }

    public void buy_item()
    {
        buyMessage.gameObject.SetActive(false);
        if(Character.Player.Stat.GOLD >= Price) // 돈이 있다면
        {
            Character.Player.Stat.GOLD-= Price;
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].itemImage.gameObject.activeSelf == false)
                {
                    list[i].AddItem(shop_list[buyItem_num].item);
                    break;
                }
                    


            }

            Price = 0;
            buyItem_num = -1;
        }
        else // 돈이없다면
        {
            CantBuy.gameObject.SetActive(true);            
            Price = 0;
        }
    }
    public void Cancle_buy_item()
    {
        buyItem_num = -1;
        buyMessage.gameObject.SetActive(false);
    }



    //드래그
    public void Begin_DragSlot(int _num)  // 드래그 시작
    {       
        MoveIcon.gameObject.SetActive(true);
        MoveIcon.sprite = list[_num].itemImage.sprite;
        Moveitem = list[_num].item;
        list[_num].SlotClear();        
        WorkingSlot = _num;
        isdrag = true;
    }
    public void End_Drag_Same(int _num)  // 드래그 목표가 같을 때
    {
        if (list[_num].item.itemType == Item.ItemType.Equipment)
        {

            End_Drag_Different(_num);

            return;
        }


        list[_num].item.ItemCount += Moveitem.ItemCount;
        list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
        isdrag = false;
        isinventory = false;
    }
    public void End_Drag_Different(int _num) // 드래그 목표가 다를 때
    {     
        list[WorkingSlot].item = list[_num].item;
        list[WorkingSlot].item.SlotNum = WorkingSlot;
        list[WorkingSlot].itemImage.gameObject.SetActive(true);
        list[WorkingSlot].itemImage.sprite = list[_num].itemImage.sprite;
        list[WorkingSlot].SetSlotCount();


        list[_num].item = Moveitem;
        list[_num].item.SlotNum = _num;
        list[_num].itemImage.sprite = MoveIcon.sprite;
        list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
        isdrag = false;
        isinventory = false;
    }
    public void End_Drag_Empty(int _num) // 빈곳에 드래그 했을때 
    {

        list[_num].item = Moveitem;
        list[_num].item.SlotNum = _num;
        list[_num].itemImage.gameObject.SetActive(true);
        list[_num].itemImage.sprite = MoveIcon.sprite;

        list[_num].SetSlotCount();
        Moveitem = null;
        MoveIcon.gameObject.SetActive(false);
        WorkingSlot = -1;
        isdrag = false;
        isinventory = false;
    }


}