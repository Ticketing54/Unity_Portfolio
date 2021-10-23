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
        
       
    }

}
