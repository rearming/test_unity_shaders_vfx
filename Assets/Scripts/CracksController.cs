using System;
using System.Collections;
using GenericScripts;
using Settings;
using UnityEngine;

public class CracksController : MonoBehaviour
{
	[SerializeField] private CracksSettings settings;
	[SerializeField] private KeyCode activationKey;
	private Animator animator = null;
	private bool appeared;
	
	private void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetFloat("Appear Speed", 1 / settings.AppearSpeed);
		animator.SetFloat("Disappear Speed", 1 / settings.DisappearSpeed);
		Appear();
	}

	private void OnEnable()
	{
		if (animator != null)
			Appear();
	}
	
	private void Appear()
	{
		animator.SetBool("Appear", true);
		StartCoroutine(nameof(DelayedDisappear));
	}

	IEnumerator DelayedDisappear()
	{
		yield return new WaitForSeconds(settings.TimeToDisappear);
		animator.SetBool("Appear", false);
		ObjectPool.Instance.ReturnGameObjectToPool(gameObject);
	}
}