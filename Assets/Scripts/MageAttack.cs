using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MageAttack : MonoBehaviour
{
    [SerializeField] private Transform targetsParent;
    [SerializeField] private Transform[] targets;
    private VFXProjectileShooter shooter;

    private int targetIndex;
    
    void Start()
    {
        if (targetsParent != null)
            targets = targetsParent.GetComponentsInChildren<Transform>().Where(t => t.parent != null).ToArray();
        shooter = GetComponentInChildren<VFXProjectileShooter>();
    }

    public void Shoot()
    {
        if (targets == null)
        {
            Debug.LogWarning($"No targets assigned in {gameObject}!");
            return;
        }
        var target = targets[targetIndex];
        var prevRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(prevRotation.x, Quaternion.LookRotation(target.position).eulerAngles.y, prevRotation.z);
        shooter.Shoot(targets[targetIndex].position);
        targetIndex = (targetIndex + 1) % targets.Length;
    }
}
