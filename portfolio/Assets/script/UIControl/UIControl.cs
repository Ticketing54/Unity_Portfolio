using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UIControl
{
    bool UsingUi { get; set; }
    string NickName { get; }
    float Distance { get; }
    bool isClick();    
}
public interface BattleUiControl : UIControl
{
    float HP_Current { get; }
    float HP_Max { get; }
}

public interface MonsterUiControl : BattleUiControl
{
    bool mightyEnermy();
}

public interface NpcUiControl : UIControl
{

}

public interface ItemUiContorl
{
    void UpdateItemSlot(int _Index);

    void ClearSlot(int _Index);
}