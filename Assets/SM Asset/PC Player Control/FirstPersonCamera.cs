using System;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    
    public Transform Target;

    public float turnSmoothness = 1.0f;
    public float MouseSensitivity = 5f;

    private float yRotation;
    private float xRotation;

    public Vector3 positionOffset;
    public Transform orientation;
    public Transform cameraPosition;

    // bool started = false;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }
        
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        yRotation += mouseX;
        
        xRotation -= mouseY;
        xRotation = Math.Clamp(xRotation, -80f, 80f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation + 180, 0);

        transform.position = cameraPosition.position;

    }

}
