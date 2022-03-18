﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemMove
{
    void AddItem(int _index, Item _NewItem);
    Item GetItem(int _Index);
    Item PopItem(int _Index);        
    Item Exchange(int _index, Item _NewItem);   

    bool PossableMoveItem(int _index, Item _MoveItem);
}