using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TestPlaceCracks : MonoBehaviour
{
    private Camera mainCamera;
    private CracksPlacer cracksPlacer;
    
    [SerializeField] private GameObject[] crackPrefabs;

    private void Start()
    {
        mainCamera = Camera.main;
        cracksPlacer = GetComponent<CracksPlacer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
            PlaceCracks(Input.mousePosition);
    }

    private void PlaceCracks(Vector3 position)
    {
        var ray = mainCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out var raycastHit))
            cracksPlacer.Place(
                crackPrefabs[Random.Range(0, crackPrefabs.Length)],
                raycastHit.point, raycastHit.normal);
    }
}
