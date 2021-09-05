﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemTableManager: SingleTon<ItemTableManager>
{

    public ResourceManager<int, string> Item_Table = new ResourceManager<int, string>();

    public void LoadTable()
    {
        TextAsset tmp = Resources.Load<TextAsset>("csv/Table/ItemTable");
        string[] DATA = tmp.text.Split('\r');
        for (int i = 1; i < DATA.Length - 1; i++)
        {
            string[] sData = DATA[i].Split(',');
            for (int j = 0; j < sData.Length; j++)
            {
                Item_Table.AddData(int.Parse(sData[0]), sData[j]);
            }

        }

    }
    
}
