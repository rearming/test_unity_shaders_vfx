using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "New VFX Projectile", menuName = "VFX Projectile", order = 0)]
public class VFXProjectileSettings : ScriptableObject
{
	[SerializeField] private float speed;
	
	[Tooltip("After collision, projectile will disappear in that time")]
	[SerializeField] private float timeBeforeDisable;

	[Tooltip("VFX Will sample this gradient over its life")]
	[SerializeField] private Gradient colorOverLife;

	[SerializeField] private VisualEffectAsset vfxAsset;

	[Tooltip("Cracks that will be placed at collision point")]
	[SerializeField] private GameObject cracksAfterHit;

	public float Speed => speed;
	
	public float TimeBeforeDisable => timeBeforeDisable;
	
	public Gradient ColorOverLife => colorOverLife;

	public VisualEffectAsset VfxAsset => vfxAsset;

	public GameObject CracksAfterHit => cracksAfterHit;
}
