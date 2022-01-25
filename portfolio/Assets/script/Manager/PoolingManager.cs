using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager<T>{
    class PoolingData
    {
        public PoolingData(T _prefab)
        {
            Prefab = _prefab;
            Pool = new Queue<T>();
            Pool.Enqueue(_prefab);
            count = 1;
        }

        void Add(T _data)
        {
            Pool.Enqueue(_data);
            
            
        }
        T Prefab;
        Queue<T> Pool;
        int count;

        T 
    }


    Dictionary<string, PoolingData> dic = new Dictionary<string, PoolingData>();
    PoolingData data;
    public delegate void ResetObj(T _obj);
    
    public void Add(string _Key,T _t, ResetObj _Reset)
    {
        _Reset(_t);

        if (dic.TryGetValue(_Key,out data))
        {

        }
        else
        {
            dic.Add(_Key, new PoolingData(_t));            
        }

        data = null;
    }

   


       


}
