// #define UNITY_REMOTE
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

    private Transform cachedTransform;

    private FloatingJoystick joystick;
    
    private Vector3 firstPoint;
    private Vector3 secondPoint;
    private float xAngle;
    private float yAngle;
    private float xAngTemp;
    private float yAngTemp;

    [SerializeField] private RectTransform ignoredTouchZone;
    
    void Start()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        cachedTransform = transform;
#if UNITY_EDITOR && !UNITY_REMOTE 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }
    
    void Update()
    {
        cachedTransform.position += GetMovementVector();
#if UNITY_EDITOR && !UNITY_REMOTE
        cachedTransform.localEulerAngles = MouseRotation();
#else
        TouchRotation();
#endif
    }

    private void TouchRotation()
    {
        if (Input.touchCount > 0)
        {
            if (ignoredTouchZone.rect.Contains(Input.GetTouch(0).position))
                return;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstPoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondPoint = Input.GetTouch(0).position;
                xAngle = xAngTemp + (secondPoint.x - firstPoint.x) * 180.0f / Screen.width;
                yAngle = yAngTemp - (secondPoint.y - firstPoint.y) * 90.0f / Screen.height;
                transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            }
        }
    }
    private Vector3 MouseRotation()
    {
        var cameraRotation = cachedTransform.localEulerAngles;
        var rotation = cameraRotation;

        rotation.x -= Input.GetAxis("Mouse Y") * rotateSensVert; // vertical
        rotation.x = rotation.x > 180 ? rotation.x - 360 : rotation.x;
        rotation.x = Mathf.Clamp(rotation.x, -maxVertAngle, maxVertAngle);
        rotation.y += Input.GetAxis("Mouse X") * rotateSensHoriz; // horizontal
        return rotation;
    }

    private Vector3 GetMovementVector()
    {
        var moveForward = cachedTransform.forward;
        var moveSide = cachedTransform.right;
        var moveVertical = Vector3.up;

        moveForward.y = 0;
        moveSide.y = 0;
        var moveDir = GetMovementDir();
        moveForward *= Time.deltaTime * moveSpeed * moveDir.z;
        moveSide *= Time.deltaTime * moveSpeed * moveDir.x;
        moveVertical *= Time.deltaTime * moveSpeed * GetVerticalMovement();
        return moveForward + moveSide + moveVertical;
    }

    Vector3 GetMovementDir()
    {
        var result = Vector3.zero;
        
#if UNITY_EDITOR && !UNITY_REMOTE
        result.x += Input.GetAxis("Horizontal");
        result.z += Input.GetAxis("Vertical");
#else
        result.x += joystick.Horizontal;
        result.z += joystick.Vertical;
#endif
        return result;
    }
    
    private float GetVerticalMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            return -1f;
        return Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }
}