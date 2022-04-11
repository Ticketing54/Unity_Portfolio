using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATUSEFFECT
{
    NOMAL,
    STURN,
    KNOCKBACK
}
public enum STATUS
{
    HP,
    MP,
    ATK,
}
public enum CHARACTERANIMATION
{
    STOP,
    MOVE,
    ATTACK,
    SKILL,
}
public enum DAMAGE
{
    NOMAL,
    CRITICAL
}
public enum CLICKEFFECT
{
    NORMAL,
    ENERMY,
    FRIEND,
    NONE
}
public enum TABLETYPE
{
    ITEM,
    LEVEL,
    MONSTER,
    QUEST,
    SKILL,
    USER,
    NONE,
}


public enum RESTYPE
{
    GAMEOBJECT,
    IMAGE,
    NONE,
}
public enum HAVEQUESTSTATE
{
    PLAYING,
    FINISH
}
public enum QUESTTYPE
{
    BATTLE,
    COLLECT,
    DIALOG,
    ETC
}
public enum QUESTSTATE
{
    NONE,
    READY =0,
    PLAYING,
    COMPLETE,
    DONE
}
public enum GOALTYPE
{
    NPC,
    MONSTER,
    ITEM,
    ETC
}
public enum ITEMLISTTYPE
{
    INVEN,
    QUICK,
    EQUIP,
    NONE
}
public enum ITEMTYPE
{
    EQUIPMENT,
    USED,
    ETC,
    NONE,
}
public enum EQUIPTYPE
{
    HEAD,
    ARMOR,
    RIGHTARM,
    LEFTARM,
    SHOSE,
    NONE

}
public enum BUSINESSTYPE
{    
    BUY,
    SELL,
    NONE,
}

