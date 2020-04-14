using System;
using System.Collections;
using GenericScripts;
using Settings;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class CracksController : MonoBehaviour
{
	[SerializeField] private CracksSettings settings;
	private Animator animator;
	private bool appeared;
	
	private new Renderer renderer;
	
	private static readonly int SeedFieldId = Shader.PropertyToID("Vector1_8FC82852");
	private static readonly int AppearBoolId = Animator.StringToHash("Appear");
	private static readonly int AppearSpeedId = Animator.StringToHash("Appear Speed");
	private static readonly int DisappearSpeedId = Animator.StringToHash("Disappear Speed");

	private void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetFloat(AppearSpeedId, 1 / settings.AppearSpeed);
		animator.SetFloat(DisappearSpeedId, 1 / settings.DisappearSpeed);
		renderer = GetComponentInChildren<Renderer>();
		Appear();
	}

	private void OnEnable()
	{
		if (animator != null)
			Appear();
	}
	
	private void Appear()
	{
		var rotation = transform.rotation.eulerAngles;
		transform.localEulerAngles = new Vector3(rotation.x, rotation.y, Random.rotation.eulerAngles.y);
		renderer.material.SetFloat(SeedFieldId, Random.Range(0, 10000));
		animator.SetBool(AppearBoolId, true);
		StartCoroutine(nameof(DelayedDisappear));
	}

	private IEnumerator DelayedDisappear()
	{
		yield return new WaitForSeconds(settings.TimeToDisappear);
		animator.SetBool(AppearBoolId, false);
		yield return new WaitForSeconds(settings.DisappearSpeed);
		ObjectPool.Instance.Release(gameObject);
	}
}