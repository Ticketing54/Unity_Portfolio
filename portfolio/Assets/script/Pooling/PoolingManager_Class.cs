using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager_Class <Type> where Type : MonoBehaviour,PoolData_Class
{
    Dictionary<string, PoolData_Class<Type>> poolDic;
    

    public PoolingManager_Class()
    {
        poolDic = new Dictionary<string, PoolData_Class<Type>>();
    }

    public void Add(string _Key, Type _NewPoolingData)
    {
        if (_NewPoolingData == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다.");
            return;
        }

        
        PoolData_Class<Type> poolData;
        if (poolDic.TryGetValue(_Key, out poolData))
        {
            poolData.Add(_NewPoolingData);
        }
        else
        {
            poolData = new PoolData_Class<Type>(_NewPoolingData);                             
            poolDic.Add(_Key, poolData);
        }
    }

    public Type GetData(string _Key)
    {
        PoolData_Class<Type> poolData;
        if (poolDic.TryGetValue(_Key, out poolData))
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