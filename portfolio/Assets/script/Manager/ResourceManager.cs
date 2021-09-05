using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ResourceManager<TKey, TValue> 
{
    
    public Dictionary<TKey, List<TValue>> dic = new Dictionary<TKey, List<TValue>>();

    public void AddData(TKey key, TValue val)
    {
        List<TValue> list;
        if(dic.TryGetValue(key, out list))
        {
            list.Add(val);
        }
        else
        {
            list = new List<TValue>();
            list.Add(val);
            dic.Add(key, list);
        }
    }
    public List<TValue> GetData(TKey key)
    {
        List<TValue> list;
        if(dic.TryGetValue(key, out list))
        {
            return list;
        }
        else
        {
            return null;
        }
    }    
}

