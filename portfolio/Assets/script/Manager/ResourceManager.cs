using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class ResourceManager
{
    Dictionary<string, GameObject> ObjRes;
    Dictionary<string, Sprite> ImgRes;
    Dictionary<TABLETYPE, List<string>> TableRes;
    public ResourceManager()
    {
        ObjRes = new Dictionary<string, GameObject>();
        ImgRes = new Dictionary<string, Sprite>();
        TableRes = new Dictionary<TABLETYPE, List<string>>();
    }    

    GameObject GetGameObject(string _Name)
    {
        GameObject Obj;
        if(ObjRes.TryGetValue(_Name,out Obj))
        {
            return Obj;
        }
        return null;
    }

    Sprite GetImage(string _Name)
    {
        Sprite img;
        if (ImgRes.TryGetValue(_Name, out img))
        {
            return img;
        }
        return null;
    }
    
    public string GetTable(TABLETYPE _Type, int _Index)
    {
        List<string> table;
        if(TableRes.TryGetValue(_Type,out table))
        {
            return table[_Index];
        }
        else
        {
            Debug.LogError("테이블이 존재하지 않습니다.");
            return null;
        }
    }
    public void LoadGameObjectRes()
    {
        GameObject[] obj = Resources.LoadAll<GameObject>("obj/");
        foreach (GameObject one in obj)
        {
            ObjRes.Add(one.name, one);            
        }
       
    }
    public void LoadImageResources()     //이미지 파일 저장
    {
        Sprite[] obj = Resources.LoadAll<Sprite>("Image" + "/");
        foreach (Sprite one in obj)
        {
            ImgRes.Add(one.name, one);
        }
    }
    public void LoadTable(TABLETYPE _Type, string _Path)
    {
        TextAsset tmp = Resources.Load<TextAsset>(_Path);
        List<string> Table = new List<string>();

        string[] DATA = tmp.text.Split('\r');

        for (int i = 1; i < DATA.Length - 1; i++)
        {
            Table.Add(DATA[i]);
            // 각각의 데이터 읽기
            //string[] sData = DATA[i].Split(',');
            //for (int j = 0; j < sData.Length; j++)
            //{                
            //}
        }

        TableRes.Add(_Type, Table);
    }

    //굳이 리스트를 더 넣지말고 여러개를 배치해서 사용처에맞게 만들자!

}

