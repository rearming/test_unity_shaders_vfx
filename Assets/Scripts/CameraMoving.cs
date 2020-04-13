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

    private Transform cachedTransform;

    private FloatingJoystick joystick; 

    void Start()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        cachedTransform = transform;
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
    
    void Update()
    {
        cachedTransform.position += GetMovementVector();
        cachedTransform.localEulerAngles = GetRotation();
    }

    private Vector3 GetRotation()
    {
        var cameraRotation = cachedTransform.localEulerAngles;
        var rotation = cameraRotation;

        rotation.x -= Input.GetAxis("Vertical") * rotateSensVert; // vertical
        rotation.x = rotation.x > 180 ? rotation.x - 360 : rotation.x;
        rotation.x = Mathf.Clamp(rotation.x, -maxVertAngle, maxVertAngle);
        rotation.y += Input.GetAxis("Horizontal") * rotateSensHoriz; // horizontal
        return rotation;
    }

    private Vector3 GetMovementVector()
    {
        var moveForward = cachedTransform.forward;
        var moveSide = cachedTransform.right;
        var moveVertical = Vector3.up;

        var moveDir = GetMovementDir();
        moveForward *= Time.deltaTime * moveSpeed * moveDir.z;
        moveSide *= Time.deltaTime * moveSpeed * moveDir.x;
        moveVertical *= Time.deltaTime * moveSpeed * GetVerticalMovement();
        return moveForward + moveSide + moveVertical;
    }

    Vector3 GetMovementDir()
    {
        var result = Vector3.zero;
        
        result.x += joystick.Horizontal;
        result.z += joystick.Vertical;
        
        result.x += Input.GetAxis("Horizontal");
        result.z += Input.GetAxis("Vertical");
        
        return result;
    }
    
    private float GetVerticalMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            return -1f;
        return Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }
}