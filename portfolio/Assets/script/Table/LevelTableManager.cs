using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelTableManager: SingleTon<LevelTableManager>
{
    List<string> Table;

    public ResourceManager<int, string> Level_Table = new ResourceManager<int, string>();
    public void LoadTable()
    {
        TextAsset tmp = Resources.Load<TextAsset>("csv/Table/LevelTable");
        string[] DATA = tmp.text.Split('\r');
        for (int i = 1; i < DATA.Length - 1; i++)
        {
            string[] sData = DATA[i].Split(',');
            for (int j = 0; j < sData.Length; j++)
            {
                Level_Table.AddData(int.Parse(sData[0]), sData[j]);
            }

        }

    }
    public void GetLevelTable(int _Level,ref float _Hp, ref float _Mp,ref float _Exp,ref int _Str,ref int _Dex,ref int _Int,ref int _Luk)
    {
        Table = Level_Table.GetData(_Level);
        _Level = int.Parse(Table[0]);
        _Hp = float.Parse(Table[1]);
        _Mp = float.Parse(Table[2]);
        _Exp = float.Parse(Table[3]);
        _Str = int.Parse(Table[4]);
        _Dex = int.Parse(Table[5]);        
        _Int = int.Parse(Table[6]);
        _Luk = int.Parse(Table[7]);
       
    }

}
