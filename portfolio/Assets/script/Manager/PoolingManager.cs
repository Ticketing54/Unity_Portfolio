using System.Collections;
using System.Collections.Generic;
using UnityEngine;



class PoolData<Type>
{
    Queue<Type> Pool;
    public Type instantiateObject { get; set; }
    int count = 0;

    public PoolData(Type _instantiateObject)
    {
        Pool = new Queue<Type>();
        instantiateObject = _instantiateObject;
        count = 0;
    }    

    public void Add(Type _PoolingObject)
    {
        Pool.Enqueue(_PoolingObject);
        count++;
    }
    public Type GetData()
    {
        if (count <= 0)
        {
            return default(Type);
        }
        count--;
        return Pool.Dequeue();
    }
    public int Count { get { return count; } }
}

public class PoolingManager_PrimitiveObject<Type> where Type : MonoBehaviour
{
    Dictionary<string, PoolData<Type>> poolingData;

    public PoolingManager_PrimitiveObject()
    {
        poolingData = new Dictionary<string, PoolData<Type>>();
    }

    public void Add(string _Key,Type _NewPoolingData)
    {

        if (_NewPoolingData == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다.");
            return;
        }
            
        PoolData<Type> poolData;
        if(poolingData.TryGetValue(_Key, out poolData))
        {
            poolData.Add(_NewPoolingData);

        }
        else
        {
            poolData = new PoolData<Type>(_NewPoolingData);
            Type NewPoolObject = GameObject.Instantiate<Type>(_NewPoolingData);
            poolData.Add(_NewPoolingData);
            poolingData.Add(_Key, poolData);
        }
    }

  
    public Type GetData(string _Key)
    {
        PoolData<Type> poolData;
        if (poolingData.TryGetValue(_Key, out poolData))
        {
            Type poolDataObject = poolData.GetData();
            if (poolDataObject == null)
            {
                return GameObject.Instantiate<Type>(poolData.instantiateObject);
            }
            else
            {
                return poolDataObject;
            }
        }
        else
        {
            Debug.LogError("존재하지 않는 데이터 입니다.");
            return default(Type);
        }
    }

  
}
