using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MAPTYPE
{
    NOMAL,
    BOSS,
    EVENT,
}
public enum MONSTERSTATE
{    
    IDLE,
    COMBAT,
    MOVE,
    ATTACK,
    DAMAGED,
    DIE,
}
public enum MAINLOADING
{
    NEW,
    LOAD1,
    LOAD2,
    LOAD3,
    LOAD4
}
public enum SKILLTYPE
{
    NONE,
    PASSIVE,
    ACTIVE,
    BUFF
}


public enum RECOVERY
{
    HP,
    MP
}
public enum QUESTMARKTYPE
{
    STARTABLE,
    NOTCOMPLETE,
    COMPLETE
}
public enum STATUSEFFECT
{
    NOMAL,
    STURN,
    KNOCKBACK,
    SLOW,
}
public enum STATUS
{
    HP,
    MP,
    ATK,
}

public enum CLICKEFFECT
{
    NORMAL,
    ENERMY,
    FRIEND,
    NONE
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
    ITEMBOX,
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

