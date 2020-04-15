using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CracksPlacer : MonoBehaviour
{
    public void Place(GameObject cracksPrefab, Vector3 position, Vector3 normal)
    {
        var crack = ObjectPool.Instance.Get(cracksPrefab);
        crack.transform.position = position + normal * 0.001f;
        crack.transform.rotation = Quaternion.LookRotation(-normal);
        crack.SetActive(true);
    }
}
