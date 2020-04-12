using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CracksPlacer : MonoBehaviour
{
    [SerializeField] private GameObject crackPrefab;
    
    public void Place(Vector3 position)
    {
        var crack = ObjectPool.Instance.GetGameObjectFromPool(crackPrefab);
        crack.transform.position = position + Vector3.up * 0.01f;
    }
}
