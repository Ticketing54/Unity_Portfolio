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
    int skillIndex;
    Character character;

    private void Start()
    {
        type = ITEMLISTTYPE.NONE;
        slotNum = -1;
        skillIndex = -1;

        UIManager.uimanager.MoveItemIcon += SetMoveIcon_item;
        UIManager.uimanager.AMoveSkillIcon += SetMoveIcon_Skill;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        character = GameManager.gameManager.character;
    }

    void SetMoveIcon_item(ITEMLISTTYPE _type, int _slotNum, Vector2 _pos)
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
    void SetMoveIcon_Skill(int _skillIndex, Vector2 _pos)
    {
        if (this.gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
        if(_skillIndex != skillIndex)
        {
            skillIndex = _skillIndex;
            Skill skill = new Skill(skillIndex);
            iconImage.sprite = ResourceManager.resource.GetImage(skill.spriteName);
        }
        gameObject.transform.position = _pos;
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            type            = ITEMLISTTYPE.NONE;
            slotNum         = -1;
            skillIndex      = -1;
            gameObject.SetActive(false);
        }
    }
}
