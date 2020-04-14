using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CracksPlacer : MonoBehaviour
{
    [SerializeField] private GameObject cracksControllerPrefab;
    
    public void Place(Vector3 position, Vector3 normal)
    {
        var crack = ObjectPool.Instance.Get(cracksControllerPrefab);
        crack.transform.position = position + normal * 0.001f;
        crack.transform.rotation = Quaternion.LookRotation(-normal);
    }
}
