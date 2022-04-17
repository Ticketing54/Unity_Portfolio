using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager<Type> where Type : Component
{
    Dictionary<string, PoolData<Type>> poolingData;
    GameObject Parent;
    public PoolingManager(GameObject _Parent)
    {
        poolingData = new Dictionary<string, PoolData<Type>>();
        Parent = _Parent;
    }

    public void Add(string _Key, Type _NewPoolingData)
    {

        if (_NewPoolingData == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다.");
            return;
        }

        PoolData<Type> poolData;
        if (poolingData.TryGetValue(_Key, out poolData))
        {
            poolData.Add(_NewPoolingData);
        }
        else
        {
            
            poolingData.Add(_Key, new PoolData<Type>(_NewPoolingData,Parent,_Key));
        }
    }


    public Type GetData(string _Key)
    {
        PoolData<Type> poolData;
        if (poolingData.TryGetValue(_Key, out poolData))
        {
            return poolData.GetData();
        }
        else
        {
            Debug.LogError("존재하지 않는 데이터 입니다.");
            return default(Type);
        }
    }
}

