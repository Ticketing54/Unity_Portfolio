using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolingManager_Resource
{
    Dictionary<string, PoolData_Resource> poolingData;
    GameObject Parent;
    public PoolingManager_Resource(GameObject _Parent)
    {
        poolingData = new Dictionary<string, PoolData_Resource>();
        Parent = _Parent;
    }

    public void Add(string _Key, GameObject _NewPoolingData)
    {

        if (_NewPoolingData == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다.");
            return;
        }

        PoolData_Resource poolData;
        if (poolingData.TryGetValue(_Key, out poolData))
        {
            poolData.Add(_NewPoolingData);
        }
        else
        {
            poolingData.Add(_Key, new PoolData_Resource(_Key,Parent,_NewPoolingData));            
        }
    }   

    public GameObject GetData(string _Key)
    {
        PoolData_Resource poolData;
        if (poolingData.TryGetValue(_Key, out poolData))
        {
            return poolData.GetData();
        }
        else
        {
            Debug.LogError("존재하지 않는 데이터 입니다.");
            return null;
        }
    }
}

