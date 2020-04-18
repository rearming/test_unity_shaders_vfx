using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFXProjectileShooter : MonoBehaviour
{
	[SerializeField] private GameObject[] vfxProjectilePrefabs;

	public void Shoot(Vector3 targetPos)
	{
		var shootDir = (targetPos - transform.position).normalized;
		var newProjectile = ObjectPool.Instance.Get(vfxProjectilePrefabs[Random.Range(0, vfxProjectilePrefabs.Length)]);
		
		newProjectile.transform.position = transform.position;
		var controller = newProjectile.GetComponent<VFXProjectileController>();
		controller.StartFlight(shootDir);
		newProjectile.SetActive(true);
	}
}
