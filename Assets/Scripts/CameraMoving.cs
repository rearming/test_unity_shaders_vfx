using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace MyNamespace
{
    
}

[SelectionBase]
public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSensHoriz;
    [SerializeField] private float rotateSensVert;
    [SerializeField] private float maxVertAngle = 60.0f;

    private Transform _transform;

    void Start()
    {
        _transform = transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        _transform.position += GetMovementVector();
        _transform.localEulerAngles = GetRotation();
    }

    private Vector3 GetRotation()
    {
        var cameraRotation = _transform.localEulerAngles;
        var rotation = cameraRotation;

        rotation.x -= Input.GetAxis("Mouse Y") * rotateSensVert; // vertical
        rotation.x = rotation.x > 180 ? rotation.x - 360 : rotation.x;
        rotation.x = Mathf.Clamp(rotation.x, -maxVertAngle, maxVertAngle);
        rotation.y += Input.GetAxis("Mouse X") * rotateSensHoriz; // horizontal
        return rotation;
    }

    private Vector3 GetMovementVector()
    {
        var moveForward = _transform.forward;
        var moveSide = _transform.right;
        var moveVertical = Vector3.up;
        
        moveForward *= Time.deltaTime * moveSpeed * Input.GetAxis("Vertical");
        moveSide *= Time.deltaTime * moveSpeed * Input.GetAxis("Horizontal");
        moveVertical *= Time.deltaTime * moveSpeed * GetVerticalMovement();
        return moveForward + moveSide + moveVertical;
    }

    private float GetVerticalMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            return -1f;
        return Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }
}