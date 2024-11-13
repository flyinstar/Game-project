using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    
    //从对象池中获取物品
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;
        //判断物品的池是否存在以及池中是否有该物品
        //使用物品名查找该物品是否在字典中
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);
            //生成一个总的池
            if (pool == null)
                pool = new GameObject("ObjectPool");
            //尝试获取一个子对象池
            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            //如果子对象池不存在，生成一个子对象池
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        //从队列中获取物品并激活
        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;
    }

    //将物品放入池中
    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>());
        }
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
