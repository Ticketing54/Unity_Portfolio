using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveIcon : MonoBehaviour
{
    [SerializeField]
    Image iconImage;

    ITEMLISTTYPE type;
    int slotNum;

    Character character;

    private void Start()
    {
        UIManager.uimanager.MoveItemIcon += SetMoveIcon;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        character = GameManager.gameManager.character;
    }

    void SetMoveIcon(ITEMLISTTYPE _type, int _slotNum, Vector2 _pos)
    {
        if (this.gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        if (_type != type || slotNum != _slotNum)
        {
            type = _type;
            slotNum = _slotNum;

            Item item = character.ItemList_GetItem(type, slotNum);
            iconImage.sprite = ResourceManager.resource.GetImage(item.itemSpriteName);

        }
        gameObject.transform.position = _pos;
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            gameObject.SetActive(false);
        }
    }
}
