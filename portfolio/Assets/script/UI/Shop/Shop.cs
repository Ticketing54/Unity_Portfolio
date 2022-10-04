using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler , IDragHandler, IEndDragHandler,IBeginDragHandler
{

    [Header("상점 슬롯")]
    [SerializeField]
    List<ShopSlot> shop_List;
    [Header("인벤 슬롯")]
    [SerializeField]
    List<ShopSlot> inven_List;

    [Header("판매 영역")]
    [SerializeField]
    Slot buyItemArea;
    [Header("메세지 윈도우")]
    [SerializeField]
    GameObject businessMessage;    
    
    
    [Header("구매/판매 버튼")]
    [SerializeField]
    GameObject businessButton;
    [SerializeField]
    TextMeshProUGUI businessButtonText;
    [Header("수량 메세지 오브젝트들")]
    [SerializeField]
    GameObject countMessage;
    [SerializeField]
    TextMeshProUGUI countMessage_Text;
    [SerializeField]
    TextMeshProUGUI countText;
    [SerializeField]
    TextMeshProUGUI countPriceText;
    [SerializeField]
    TextMeshProUGUI countOkButton_Text;
    [Header("마지막 거래 확인")]
    [SerializeField]
    GameObject finallyMessage;
    [SerializeField]
    TextMeshProUGUI finally_itemName;
    [SerializeField]
    TextMeshProUGUI finally_Price;
    [SerializeField]
    TextMeshProUGUI finally_Text;
    [Header("드래그 관련")]
    [SerializeField]
    Image dragImage;
    [SerializeField]
    TextMeshProUGUI invenGold;
    // 클릭정보
    BUSINESSTYPE    businessType;
    bool            activeMessage;
    int             clickIndex;
    int             activeIndex;
    int             count;

    Character character;
    Npc npc;    

    private void Start()
    {
        businessType = BUSINESSTYPE.NONE;
        activeMessage = false;
        clickIndex = -1;
        activeIndex = -1;
        count = 1;

        UIManager.uimanager.AOpenShop += (npc)=> 
        {

            UIManager.uimanager.FadeinFucout(() =>
            {
                UIManager.uimanager.ACloseDialog();
                UIManager.uimanager.OnBaseUI();
                gameObject.SetActive(true);
                GameManager.gameManager.character.IsPossableControl = true;
                OpenShop(npc);
            });
            
            
        };         
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        character = GameManager.gameManager.character;            
    }

    private void OnDisable()
    {
        ResetShop();
        
        if(businessMessage.gameObject.activeSelf == true)
        {
            businessMessage.gameObject.SetActive(false);
        }        
        npc = null;
    }
    public void OnPointerDown(PointerEventData _data)
    {
        if (activeMessage == true)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Click_LeftDown(_data.position);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Click_RightDown(_data.position);
        }
    }

    public void OnPointerUp(PointerEventData _data)
    {
        if (activeMessage == true)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Click_LeftUp(_data.position);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Click_RightUp(_data.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragImage.IsActive())
        {
            dragImage.rectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage.IsActive())
        {
            dragImage.gameObject.SetActive(false);


            if (buyItemArea.isInRect(eventData.position))
            {
                OpenBusinessWindow(true);
            }

        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (businessType == BUSINESSTYPE.SELL && clickIndex >= 0)
        {
            dragImage.gameObject.SetActive(true);
            dragImage.sprite = inven_List[clickIndex].ICON;
            dragImage.rectTransform.position = eventData.position;
        }
    }



    #region CountMessage
    public void OpenCountButton()
    {
        businessButton.SetActive(true);

        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                businessButtonText.text = "구매";
                countPriceText.text = " - " + (shop_List[activeIndex].itemPrice * count).ToString();
                countPriceText.color = Color.red;
                break;
            case BUSINESSTYPE.SELL:
                businessButtonText.text = "판매";
                countPriceText.text = " + " + (inven_List[activeIndex].itemPrice * count).ToString();    //
                countPriceText.color = Color.white;
                break;
            default:
                businessButton.SetActive(false);
                Debug.Log("잘못된 접근방식 : businessButton");
                break;
        }
    }
    void SetCountMessage(BUSINESSTYPE _Type)
    {
        switch (_Type)
        {
            case BUSINESSTYPE.BUY:
                countMessage_Text.text = "몇개를 구매 하시겠습니까?";
                countOkButton_Text.text = "구매";
                countPriceText.text = " - " + (shop_List[activeIndex].itemPrice * count).ToString();
                countPriceText.color = Color.red;
                break;
            case BUSINESSTYPE.SELL:
                countMessage_Text.text = "몇개를 판매 하시겠습니까?";
                countOkButton_Text.text = "판매";
                countPriceText.text = " + " + (shop_List[activeIndex].itemPrice * count).ToString();
                countPriceText.color = Color.white;
                break;
            default:
                Debug.LogError("판매에러");
                break;
        }
    }
    public void OkButton_Count()
    {
        countMessage.gameObject.SetActive(false);        
        finallyMessage.gameObject.SetActive(true);
        SetFinallyMessage();
    }
    public void NoButton_Count()
    {
        countMessage.gameObject.SetActive(false);
        businessMessage.gameObject.SetActive(false);
        activeMessage = false;
        BusinessReset();
    }

    public void CountUp()
    {
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
            {
                if (isPossableBuy())
                {
                    count++;
                }
                    countPriceText.text = " - " + (shop_List[activeIndex].itemPrice * count).ToString();
                    countPriceText.color = Color.red;
                    
                    break;
            }
            case BUSINESSTYPE.SELL:
                {
                    if (count < inven_List[activeIndex].itemCount)
                    {
                        count++;
                    }
                    countPriceText.text = " + " + (shop_List[activeIndex].itemPrice * count).ToString();
                    countPriceText.color = Color.white;
                    break;
                }
            default:
                Debug.Log("잘못된 접근방식 : countUp");
                break;
        }
        
        countText.text = count.ToString();
        
    }
    public void CountDown()
    {
        if (count > 1)
        {
            count--;
        }

        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                {                   
                    countPriceText.text = " - "+(shop_List[activeIndex].itemPrice * count).ToString();
                    countPriceText.color = Color.red;
                    break;
                }
            case BUSINESSTYPE.SELL:
                {
                    //countPriceText.text = " + "+(shop_List[activeIndex].itemPrice * count).ToString();
                    countPriceText.color = Color.white;
                    break;
                }
            default:
                Debug.Log("잘못된 접근방식 : countUp");
                break;
        }

        countText.text = count.ToString();
    }
    bool isPossableBuy()
    {
        if (GameManager.gameManager.character.inven.Gold < (shop_List[activeIndex].itemPrice * count + 1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    #region FinallyMessage
    public void SetFinallyMessage()
    {
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                {
                    finally_itemName.text = shop_List[activeIndex].itemName + " ( "+count+" ) ";
                    finally_Price.text = " - " + (shop_List[activeIndex].itemPrice * count).ToString();
                    finally_Price.color = Color.red;
                    finally_Text.text = "구매 하시겠습니까?";
                }
                break;
            case BUSINESSTYPE.SELL:
                {
                    finally_itemName.text = shop_List[activeIndex].itemName + " ( " + count + " ) ";
                    finally_Price.text = " + " + (shop_List[activeIndex].itemPrice * count).ToString();
                    finally_Price.color = Color.white;
                    finally_Text.text = "판매 하시겠습니까?";
                }
                break;
            default:
                break;
        }
    }
    public void OkButton_Finally()
    {
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                GameManager.gameManager.character.inven.BuyItem(shop_List[activeIndex].itemPrice * count,npc.ITEMS[activeIndex],count);
                BusinessReset();
                updateInven();
                
                break;
            case BUSINESSTYPE.SELL:
                GameManager.gameManager.character.inven.SellItem(activeIndex, count);
                BusinessReset();
                updateInven();
                break;
            default:
                break;
        }
    }
    public void NoButton_Finally()
    {
        finallyMessage.gameObject.SetActive(false);
        businessMessage.gameObject.SetActive(false);
    }
    void BusinessReset()
    {
        finallyMessage.gameObject.SetActive(false);
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                ResetClickinfo();
                NoButton_Finally();
                break;
            case BUSINESSTYPE.SELL:
                ResetClickinfo();
                NoButton_Finally();
                break;
            default:
                break;
        }
        activeMessage = false;
        businessMessage.gameObject.SetActive(false);
    }
    #endregion

    #region LeftClick
    void Click_LeftDown(Vector2 _clickPos)
    {
        ResetClickinfo();

        for (int shopIndex = 0; shopIndex < shop_List.Count; shopIndex++)
        {
            if (shop_List[shopIndex].isInRect(_clickPos) && npc)
            {

                clickIndex = shopIndex;
                businessType = BUSINESSTYPE.BUY;
                shop_List[shopIndex].ClickedEffectOn();
                return;
            }
        }

        for (int invenIndex = 0; invenIndex < inven_List.Count; invenIndex++)
        {
            if (inven_List[invenIndex].isInRect(_clickPos))
            {
                clickIndex = invenIndex;
                businessType = BUSINESSTYPE.SELL;
                inven_List[invenIndex].ClickedEffectOn();
                return;
            }
        }
    }
    void Click_LeftUp(Vector2 _clickPos)
    {
        if (clickIndex < 0)
        {
            return;
        }
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                {
                    if (shop_List[clickIndex].isInRect(_clickPos))
                    {
                        activeIndex = clickIndex;
                        OpenCountButton();
                        return;
                    }
                }
                break;
            case BUSINESSTYPE.SELL:
                {
                    if (inven_List[clickIndex].isInRect(_clickPos))
                    {
                        activeIndex = clickIndex;
                        OpenCountButton();
                        return;
                    }
                }
                break;
            default:
                break;
        }      
    }
    void ResetClickinfo()
    {
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                {
                    shop_List[clickIndex].ClickedEffectOff();
                }
                break;
            case BUSINESSTYPE.SELL:
                {
                    inven_List[clickIndex].ClickedEffectOff();
                }
                break;
            default:
                break;
        }        
        clickIndex = -1;
        activeIndex = -1;
        count = 1;
        businessType = BUSINESSTYPE.NONE;
    }


    #endregion

    #region RightClick
    void Click_RightDown(Vector2 _clickPos)
    {
        ResetClickinfo();

        for (int shopIndex = 0; shopIndex < shop_List.Count; shopIndex++)
        {
            if (shop_List[shopIndex].isInRect(_clickPos))
            {

                clickIndex = shopIndex;
                businessType = BUSINESSTYPE.BUY;
                shop_List[shopIndex].ClickedEffectOn();
                return;
            }
        }

        for (int invenIndex = 0; invenIndex < inven_List.Count; invenIndex++)
        {
            if (inven_List[invenIndex].isInRect(_clickPos) && character.inven.IsSlotEmpty(invenIndex) ==false)
            {
                clickIndex = invenIndex;
                businessType = BUSINESSTYPE.SELL;
                inven_List[invenIndex].ClickedEffectOn();
                return;
            }
        }
    }
    void Click_RightUp(Vector2 _clickPos)
    {
        if (clickIndex < 0)
        {
            return;
        }
        switch (businessType)
        {
            case BUSINESSTYPE.BUY:
                {
                    if (shop_List[clickIndex].isInRect(_clickPos))
                    {
                        activeIndex = clickIndex;
                        OpenBusinessWindow();
                        return;
                    }
                }
                break;
            case BUSINESSTYPE.SELL:
                {
                    if (inven_List[clickIndex].isInRect(_clickPos))
                    {
                        activeIndex = clickIndex;
                        OpenBusinessWindow();
                        return;
                    }
                }
                break;
            default:
                break;
        }
    }
    #endregion



    void OpenShop(Npc _npc)
    {
        gameObject.SetActive(true);
        npc = _npc;
        List<int> itemList = _npc.ITEMS;
        for (int i = 0; i < itemList.Count; i++)
        {
            shop_List[i].SetShopSlot(itemList[i]);
        }
        for (int shopSlotIndex = itemList.Count; shopSlotIndex < shop_List.Count; shopSlotIndex++)
        {
            shop_List[shopSlotIndex].ResetSlot();
        }
        updateInven();
    }
   
    public void OpenBusinessWindow(bool _isDrag = false)      // 드래그 시 activeIndex가 없으므로 확정
    {
        if (_isDrag == true)
        {
            activeIndex = clickIndex;
        }
        activeMessage = true;
        if (businessType == BUSINESSTYPE.BUY)
        {
            if (GameManager.gameManager.character.inven.Gold < shop_List[activeIndex].itemPrice)
            {
                Debug.Log("돈이 없습니다.");
                return;
            }

        }
        businessMessage.gameObject.SetActive(true);
        countMessage.gameObject.SetActive(true);
        SetCountMessage(businessType);
    }
    public void ExitShop()
    {
        GameManager.gameManager.character.isPossableMove = true;
        this.gameObject.SetActive(false);
    }

    public void Cancle()
    {
        businessMessage.gameObject.SetActive(false);
        activeMessage = false;
    }




    void updateInven()
    {
        Item getitem;
        for (int itemSlotNum = 0; itemSlotNum < inven_List.Count; itemSlotNum++)
        {
            getitem = character.ItemList_GetItem(ITEMLISTTYPE.INVEN, itemSlotNum);
            if (getitem == null)
            {
                inven_List[itemSlotNum].ResetSlot_Inven();
            }
            else
            {
                inven_List[itemSlotNum].SetShopSlot_Inven(getitem.itemName, getitem.itemSpriteName, getitem.itemPrice, getitem.ItemCount);
            }
            getitem = null;
        }
        invenGold.text = character.inven.Gold.ToString();
    }
    void ResetShop()
    {
        ResetClickinfo();

        foreach (ShopSlot slot in shop_List)
        {
            slot.ResetSlot();
        }
    }

    
}