using System;
using EditorScripts;
using UnityEngine;

namespace Settings
{
	[CreateAssetMenu(fileName = "New Cracks", menuName = "Cracks", order = 0)]
	public class CracksSettings : ScriptableObject
	{
		[Header("Animator Properties")]
		[Tooltip("In seconds")]
		[SerializeField] private float appearSpeed;
		[Tooltip("In seconds")]
		[SerializeField] private float disappearSpeed;

		[Tooltip("Time in seconds before disappearance animation start")]
		[SerializeField] private float timeToDisappear;

		public float AppearSpeed => appearSpeed;
		public float DisappearSpeed => disappearSpeed;
		public float TimeToDisappear => timeToDisappear;
	}
}