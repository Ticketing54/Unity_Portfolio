using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager_PrimitiveObject<Type> where Type : Object
{
    Dictionary<string, PoolData_Instantiate<Type>> poolingData;

    public PoolingManager_PrimitiveObject()
    {
        poolingData = new Dictionary<string, PoolData_Instantiate<Type>>();
    }

    public void Add(string _Key, Type _NewPoolingData)
    {

        if (_NewPoolingData == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다.");
            return;
        }

        PoolData_Instantiate<Type> poolData;
        if (poolingData.TryGetValue(_Key, out poolData))
        {
            poolData.Add(_NewPoolingData);
        }
        else
        {            
            poolingData.Add(_Key, new PoolData_Instantiate<Type>(_NewPoolingData));
        }
    }


    public Type GetData(string _Key)
    {
        PoolData_Instantiate<Type> poolData;
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

