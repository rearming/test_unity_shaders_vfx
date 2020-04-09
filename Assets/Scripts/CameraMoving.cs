using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[SelectionBase]
public class CameraMoving : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSensHoriz;
    [SerializeField] private float rotateSensVert;
    [SerializeField] private float maxVertAngle = 60.0f;

    private Transform _transform;
    private Transform _cameraTransform;

    void Start()
    {
        _transform = transform;
        _cameraTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        _transform.position += GetMovementVector();
        _transform.localEulerAngles = GetRotationY();
        _cameraTransform.localEulerAngles = GetRotationX();
    }

    private Vector3 GetRotationX()
    {
        var cameraRotation = _cameraTransform.localEulerAngles;
        var rotation = cameraRotation;

        rotation.x -= Input.GetAxis("Mouse Y"); // vertical
        rotation.x = Mathf.Lerp(cameraRotation.x, rotation.x, Time.deltaTime * rotateSensVert);
        rotation.x = rotation.x > 180 ? rotation.x - 360 : rotation.x;
        rotation.x = Mathf.Clamp(rotation.x, -maxVertAngle, maxVertAngle);
        return rotation;
    }

    private Vector3 GetRotationY()
    {
        var transformRotation = _transform.localEulerAngles;
        var rotation = transformRotation;
        
        rotation.y += Input.GetAxis("Mouse X"); // horizontal
        rotation.y = Mathf.Lerp(transformRotation.y, rotation.y, Time.deltaTime * rotateSensHoriz);
        return rotation;
    }
    
    private Vector3 GetMovementVector()
    {
        var moveForward = _transform.forward;
        var moveSide = _transform.right;
        var moveVertical = _transform.up;
        
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