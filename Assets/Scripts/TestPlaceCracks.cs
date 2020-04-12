using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestPlaceCracks : MonoBehaviour
{
    private Camera mainCamera;
    private CracksPlacer cracksPlacer;

    private void Start()
    {
        mainCamera = Camera.main;
        cracksPlacer = GetComponent<CracksPlacer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var raycastHit))
                cracksPlacer.Place(raycastHit.point);
                
        }
    }
}
