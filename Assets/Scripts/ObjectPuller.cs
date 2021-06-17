using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPuller : MonoBehaviour
{
    [Serializable]
    public struct ObjectInfo
    {
        public enum ObjectType
        {
            GRASS,
            ASPHALT,
            WATER,
            TREE,
            ROCK,
            LILLY_PAD,
            LOG,
            CAR_1,
            CAR_2,
            CAR_3,
            TRAIN,
            RAILWAY,
            SIGNAL_LIGHT,
            COIN
        }
        public ObjectType Type;
        public GameObject Prefab;
        public int StartCount;
    }

    public static ObjectPuller Instance;
    
    [SerializeField]
    private List<ObjectInfo> objectInfos;

    private Dictionary<ObjectInfo.ObjectType, Pool> pools;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        InitPool();
    }

    private void InitPool()
    {
        pools = new Dictionary<ObjectInfo.ObjectType, Pool>();
        var EmptyGO = new GameObject();
        foreach( var objInfo in objectInfos)
        {
            var container = Instantiate(EmptyGO, transform, false);
            container.name = objInfo.Type.ToString();
            pools[objInfo.Type] = new Pool(container.transform);

            for (int i = 0; i < objInfo.StartCount; i++)
            {
                var go = InstantiateGameObj(objInfo.Type, container.transform);
                pools[objInfo.Type].Objects.Enqueue(go);
            }
        }
        Destroy(EmptyGO);
    }

    private GameObject InstantiateGameObj(ObjectInfo.ObjectType type, Transform parent)
    {
        var go = Instantiate(objectInfos.Find(x => x.Type == type).Prefab, parent);
        go.SetActive(false);
        return go;
    }

    public GameObject GetObject(ObjectInfo.ObjectType type)
    {
        var obj = pools[type].Objects.Count > 0 ?
            pools[type].Objects.Dequeue() : InstantiateGameObj(type, pools[type].Container);
        //var obj = InstantiateGameObj(type, pools[type].Container);
        //Debug.Log(type + "  " + obj.GetComponent<IPooledObject>().GetType());
        obj.SetActive(true);
        return obj;
    }

    public void DestroyObject(GameObject obj)
    {
        pools[obj.GetComponent<IPooledObject>().Type].Objects.Enqueue(obj);
        obj.SetActive(false);
    }
}
