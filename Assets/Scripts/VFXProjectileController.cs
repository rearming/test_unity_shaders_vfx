using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class VFXProjectileController : MonoBehaviour
{ 
    [SerializeField] private VFXProjectileSettings settings;

    private GameObject vfxGameObject;
    private VisualEffect vfx;
    private readonly int vfxProtectileCenterProp = Shader.PropertyToID("Projectile Center");
    private readonly int vfxProjectileRadiusProp = Shader.PropertyToID("Projectile Radius");
    private readonly int vfxForceBeforeCollisionProp = Shader.PropertyToID("Force before collision");
    private readonly int vfxColorOverLifeProp = Shader.PropertyToID("Color over life");
    private readonly int vfxCollisionBoxCenterProp = Shader.PropertyToID("Collision Box Center");
    private readonly int vfxCollisionBoxExtentsProp = Shader.PropertyToID("Collision Box Extents");

    private CracksPlacer cracksPlacer;
    private new SphereCollider collider;

    private Bounds bounds;
    private Vector3 normal;
    private Vector3? flightDir;

    private void Awake()
    {
        vfxGameObject = new GameObject();
        vfxGameObject.AddComponent<VisualEffect>();
        vfxGameObject.name = "VFX Projectile " + vfxGameObject.GetInstanceID().ToString();
        vfx = vfxGameObject.GetComponent<VisualEffect>();
        vfx.visualEffectAsset = settings.VfxAsset;
        vfx.initialEventName = "";
        collider = GetComponent<SphereCollider>();
        cracksPlacer = GetComponent<CracksPlacer>();
    }

    public void StartFlight(Vector3 dir)
    {
        vfxGameObject.SetActive(true);
        flightDir = dir;
        vfx.SetVector3(vfxProtectileCenterProp, transform.position);
        vfx.SetFloat(vfxProjectileRadiusProp, collider.radius * GetMaxScale());
        vfx.SetGradient(vfxColorOverLifeProp, settings.ColorOverLife);
        bounds = CalculateVfxCollisionBounds();
        vfx.SetVector3(vfxCollisionBoxCenterProp, bounds.center);
        vfx.SetVector3(vfxCollisionBoxExtentsProp, bounds.extents * 2);
        vfx.Play();
    }

    private Bounds CalculateVfxCollisionBounds()
    {
        bounds = new Bounds();
        
        var ray = new Ray(transform.position, flightDir.GetValueOrDefault());
        if (Physics.Raycast(ray, out var hitInfo))
        {
            bounds = hitInfo.collider.bounds;
            normal = hitInfo.normal;
        }
        return bounds;
    }

    private float GetMaxScale()
    {
        return Mathf.Max(transform.localScale.x, Mathf.Max(transform.localScale.y, transform.localScale.z));
    }
    
    void Update()
    {
        transform.position += flightDir.GetValueOrDefault() * (settings.Speed * Time.deltaTime);
        vfx.SetVector3(vfxProtectileCenterProp, transform.position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        vfx.SetVector3(vfxForceBeforeCollisionProp, new Vector3(0, 0, -settings.Speed * 2));
        cracksPlacer.Place(settings.CracksAfterHit, transform.position, normal);
        StartCoroutine(nameof(DisableVfx));
    }

    IEnumerator DisableVfx()
    {
        yield return new WaitForSeconds(settings.TimeBeforeDisable);
        flightDir = null;
        vfx.Stop();
        ObjectPool.Instance.Release(gameObject);
        vfxGameObject.SetActive(false);
    }
}
