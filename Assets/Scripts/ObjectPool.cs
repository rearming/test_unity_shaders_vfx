using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;

    public static ObjectPool Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    private Dictionary<string, LinkedList<GameObject>> _objects = new Dictionary<string, LinkedList<GameObject>>();
    public GameObject GetGameObjectFromPool(GameObject prefab)
    {
        GameObject result;
    
        if (!_objects.ContainsKey(prefab.name))
            _objects[prefab.name] = new LinkedList<GameObject>();
    
        if (_objects[prefab.name].Count > 0)
        {
            result = _objects[prefab.name].First.Value;
            _objects[prefab.name].RemoveFirst();
            result.SetActive(true);
            return result;
        }
        result = Instantiate(prefab);
        result.name = prefab.name;
        return result;
    }

    public void ReturnGameObjectToPool(GameObject gameObj)
    {
        gameObj.SetActive(false);
        if (!_objects.ContainsKey(gameObj.name))
            _objects[gameObj.name] = new LinkedList<GameObject>();
        _objects[gameObj.name].AddFirst(gameObj);
    }
}

