using System;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    
    public Transform Target;
    // public Transform avatar;
    public Transform ikHeadTarget;

    public float turnSmoothness = 1.0f;
    public float MouseSensitivity = 5f;

    private float yRotation;
    private float xRotation;

    public Vector3 positionOffset;
    public Transform orientation;

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
        // print(gameObject.ToString());
        
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        yRotation += mouseX;
        
        xRotation -= mouseY;
        xRotation = Math.Clamp(xRotation, -80f, 80f);
        // transform.position = Target.position;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        // avatar.position = ikHead.position;
        // avatar.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized,  Time.deltaTime * turnSmoothness);
        // Vector3 dir = transform.forward;
        // dir.y = transform.position.y;
        // dir.Normalize();
        // ikHeadTarget.Translate(MouseSensitivity * Time.deltaTime * dir);
        // Quaternion rot = transform.rotation;
        // rot.y += 90;
        // ikHeadTarget.SetPositionAndRotation(transform.position, rot);

    }

}
