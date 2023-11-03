using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    
    public Transform Target;
    // public Transform avatar;
    public Transform ikHeadTarget;

    public float turnSmoothness = 1.0f;
    public float MouseSensitivity = 5f;

    public Vector3 positionOffset;

    private float vertialRotation;
    private float horizontalRotation;

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }
        // print(gameObject.ToString());
        

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        vertialRotation -= mouseY * MouseSensitivity;
        vertialRotation = Mathf.Clamp(vertialRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        // transform.position = Target.position;
        transform.rotation = Quaternion.Euler(vertialRotation, horizontalRotation, 0);
        // avatar.position = ikHead.position;
        // avatar.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized,  Time.deltaTime * turnSmoothness);
        // Vector3 dir = transform.forward;
        // dir.y = transform.position.y;
        // dir.Normalize();
        // ikHeadTarget.Translate(MouseSensitivity * Time.deltaTime * dir);
        Quaternion rot = transform.rotation;
        rot.y += 90;
        ikHeadTarget.SetPositionAndRotation(transform.position, rot);

    }

}
